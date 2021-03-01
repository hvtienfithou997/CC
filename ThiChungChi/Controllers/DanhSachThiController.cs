using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ThiChungChi.Models;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace ThiChungChi.Controllers
{
    public class DanhSachThiController : BaseController
    {
        public IActionResult Index(string id_cuoc_thi, long ngay_thi_tu, long ngay_thi_den, string term, int page = 1, List<int> thuoc_tinh = null)
        {
            if (is_admin || allow_create)
            {
                try
                {
                    //DanhSachThiRepository.Instance.dele();
                    long total_recs = 0;
                    ViewBag.ngay_thi_tu = ngay_thi_tu;
                    ViewBag.ngay_thi_den = ngay_thi_den;
                    ViewBag.id_cuoc_thi = id_cuoc_thi;
                    List<CuocThi> all_cuoc_thi = new List<CuocThi>();
                    if (is_admin)
                        all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" }, is_admin);
                    else
                        all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" }, user(), false);
                    ViewBag.cuoc_thi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                    var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.DANH_SACH_THI, user(), is_admin);
                    ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                    ViewBag.thuoc_tinh_search = BuildThuocTinhForSearch(lst_thuoc_tinh);
                    ViewBag.Term = term;
                    ViewBag.PageSize = page_size;
                    ViewBag.Page = page;
                    ViewBag.tt = thuoc_tinh;
                    ViewBag.user = user();
                    bool exam = false, is_expired;                    
                    List<DanhSachThi> lst_danh_sach_thi = new List<DanhSachThi>();
                    if (is_admin)
                        lst_danh_sach_thi = DanhSachThiRepository.Instance.Search(term, id_cuoc_thi, ngay_thi_tu, ngay_thi_den, user(), thuoc_tinh, page, out total_recs, out string msg, page_size, is_admin);
                    else
                        lst_danh_sach_thi = DanhSachThiRepository.Instance.Search(term, id_cuoc_thi, ngay_thi_tu, ngay_thi_den, "", thuoc_tinh, page, out total_recs, out string msg, page_size, false);

                    var lst_thuoc_tinh_cuoc_thi = lst_danh_sach_thi.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();
                    var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_cuoc_thi.Distinct(), LoaiThuocTinh.DANH_SACH_THI);
                    List<DanhSachThiMap> lst_map = new List<DanhSachThiMap>();
                    bool start = false;
                    if (lst_danh_sach_thi.Count > 0)
                    {
                        var hour = (24 - DateTime.Now.Hour) - 2;
                        var add_h = DateTime.Now.AddHours(hour).AddMinutes(59).AddSeconds(59);
                        long ngay_hien_tai = XMedia.XUtil.TimeInEpoch(add_h);
                        var day_now = add_h.Day;
                        foreach (var item in lst_danh_sach_thi)
                        {
                            var cuoc_thi = CuocThiRepository.Instance.GetById(item?.id_cuoc_thi);
                            var day_kt = XMedia.XUtil.EpochToTime(cuoc_thi.ngay_ket_thuc).Day;
                            is_expired = ngay_hien_tai > cuoc_thi.ngay_ket_thuc && day_now != day_kt;
                            var tai_khoan = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(item?.id_tai_khoan);
                            if (cuoc_thi != null && tai_khoan != null)
                                exam = KetQuaThiRepository.Instance.TimKetQuaThi(cuoc_thi.id, tai_khoan.username);
                            DanhSachThiMap map = new DanhSachThiMap(cuoc_thi, tai_khoan, item, lst_tt_by_gia_tri, exam, is_expired, start, false);
                            lst_map.Add(map);
                        }
                    }

                    ViewBag.Total = total_recs;
                    return View(lst_map.OrderByDescending(x => x.ngay_bat_dau));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return View();
            }

            return RedirectToAction("MyExam");
        }

        public IActionResult MyExam(string id_cuoc_thi, string term, long ngay_thi_tu, long ngay_thi_den, string pass, List<int> thuoc_tinh = null, int page = 1)
        {
            try
            {
                ViewBag.ngay_thi_tu = ngay_thi_tu;
                ViewBag.ngay_thi_den = ngay_thi_den;
                ViewBag.id_cuoc_thi = id_cuoc_thi;
                ViewBag.Term = term;
                ViewBag.PageSize = page_size;
                ViewBag.Page = page;
                if (!string.IsNullOrEmpty(pass))
                {
                    switch (pass)
                    {
                        case "wrong":
                            SetAlert("Mật khẩu bạn vừa nhập không chính xác, vui lòng nhập lại!", "error");
                            break;

                        default:
                            SetAlert("Lỗi xảy ra", "error");
                            return View();
                    }
                }
                ViewBag.ten = user();
                List<CuocThi> lst_cuoc_thi = new List<CuocThi>();
                var ds_thi_cuoc_thi =
                    DanhSachThiRepository.Instance.GetAll(new List<string>(), new string[] { "id_cuoc_thi", "id_tai_khoan" }, user(), is_admin);
                foreach (var item in ds_thi_cuoc_thi)
                {
                    lst_cuoc_thi.Add(CuocThiRepository.Instance.GetById(item.id_cuoc_thi));
                }

                lst_cuoc_thi = lst_cuoc_thi.Where(xx => xx != null).ToList();

                ViewBag.cuoc_thi =
                    lst_cuoc_thi.Where(x => !string.IsNullOrEmpty(x.id)).Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();

                List<DanhSachThiMap> lst_map = new List<DanhSachThiMap>();
                var list = DanhSachThiRepository.Instance.Search(term, id_cuoc_thi, ngay_thi_tu, ngay_thi_den, user(), thuoc_tinh, page, out var total_recs, out string msg, page_size, is_admin);
                var lst_thuoc_tinh_cuoc_thi = list.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();
                var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_cuoc_thi.Distinct(), LoaiThuocTinh.DANH_SACH_THI);
                
                var current = DateTime.Now.Date;
                long _current = XMedia.XUtil.TimeInEpoch(current);                
                if (list.Count > 0)
                {
                    bool is_expired, exam, start;                    
                    var hour = (24 - DateTime.Now.Hour) - 2;
                    var add_h = DateTime.Now.AddHours(hour).AddMinutes(59).AddSeconds(59);
                    long ngay_hien_tai = XMedia.XUtil.TimeInEpoch(add_h);
                    var day_now = add_h.Day;

                    foreach (var item in list)
                    {
                        var cuoc_thi = CuocThiRepository.Instance.GetById(item.id_cuoc_thi);
                        start = _current < cuoc_thi.ngay_bat_dau;
                        var day_kt = XMedia.XUtil.EpochToTime(cuoc_thi.ngay_ket_thuc).Day;
                        is_expired = ngay_hien_tai > cuoc_thi.ngay_ket_thuc && day_now != day_kt;
                        var tai_khoan = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(item.id_tai_khoan);
                        exam = KetQuaThiRepository.Instance.TimKetQuaThi(item.id_cuoc_thi, item.id_tai_khoan);
                        bool het_luot_thi = KetQuaThiRepository.Instance.DemKetQuaThi(item.id_cuoc_thi, item.id_tai_khoan) >= cuoc_thi.so_lan_thi_lai;
                        DanhSachThiMap map = new DanhSachThiMap(cuoc_thi, tai_khoan, item, lst_tt_by_gia_tri, exam, is_expired, start, het_luot_thi);
                        lst_map.Add(map);
                    }
                }
                ViewBag.Total = total_recs;
                return View(lst_map.OrderByDescending(x => x.ngay_bat_dau));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View();
        }

        public IActionResult Create()
        {
            if (is_admin)
            {
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.DANH_SACH_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                ViewBag.CuocThi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" }).Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                ViewBag.TaiKhoan = TaiKhoanRepository.Instance.GetAll(new List<string>(), new string[] { "id", "username" });
                return View();
            }

            return RedirectToAction("MyExam");
        }

        [HttpPost]
        public IActionResult Create(DanhSachThi ds, List<string> list_tai_khoan_need)
        {
            try
            {
                if (is_admin)
                {
                    if (list_tai_khoan_need.Count > 0)
                    {
                        foreach (var str in list_tai_khoan_need)
                        {
                            var find_tk = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(ds.id_cuoc_thi, str);
                            if (find_tk == null)
                            {
                                ds.id_tai_khoan = str;
                                SetMetaData(ds, false);
                                var danh_sach_thi = DanhSachThiRepository.Instance.Index(ds);
                                if (danh_sach_thi)
                                {
                                    SetAlert("Thêm danh sách thành công", "success");
                                }
                                else
                                {
                                    SetAlert("Thêm danh sách thất bại", "error");
                                }
                            }
                            else
                            {
                                SetAlert($"Tài khoản {str} đã có trong danh sách thi", "error");
                            }
                        }
                    }
                    else
                    {
                        SetAlert("Thêm danh sách thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Bạn không có quyền tạo!", "error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View();
        }

        public IActionResult Edit(string id)
        {
            if (is_admin)
            {
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.DANH_SACH_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                ViewBag.CuocThi = CuocThiRepository.Instance.GetAll(new List<string>(), new[] { "id", "ten" }).Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                ViewBag.TaiKhoan = TaiKhoanRepository.Instance.GetAll(new List<string>(), new[] { "id", "username" }).Select(o => new SelectListItem { Value = o.id, Text = o.username }).ToList();
                var ds_thi = DanhSachThiRepository.Instance.GetById(id);
                ViewBag.tt = ds_thi.thuoc_tinh;
                return View(ds_thi);
            }
            return RedirectToAction("MyExam");
        }

        [HttpPost]
        public IActionResult Edit(DanhSachThi ds)
        {
            try
            {
                if (is_admin)
                {
                    SetMetaData(ds, true);
                    var danh_sach = DanhSachThiRepository.Instance.Update(ds);
                    if (danh_sach)
                    {
                        SetAlert("Sửa danh sách thành công", "success");
                    }
                    else
                    {
                        SetAlert("Sửa danh sách thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Bạn không có quyền sửa!", "error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        public IActionResult Delete(string id)
        {
            try
            {
                if (is_admin)
                {
                    var kq = DanhSachThiRepository.Instance.Delete(id);
                    if (!kq)
                    {
                        SetAlert("Xóa danh sách thất bại", "error");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            var danh_sach = DanhSachThiRepository.Instance.GetById(id);
            var ten_cuoc_thi = CuocThiRepository.Instance.GetById(danh_sach.id_cuoc_thi).ten;
            danh_sach.id_cuoc_thi = ten_cuoc_thi;
            return View(danh_sach);
        }

        [HttpPost]
        public IActionResult LuuChungChi(string id_obj, List<int> thuoc_tinh)
        {
            var ds_thi = DanhSachThiRepository.Instance.UpdateThuocTinh(id_obj, thuoc_tinh);
            if (ds_thi)
            {
                SetAlert("Thành công", "success");
            }
            else
            {
                SetAlert("Thất bại", "error");
            }
            return RedirectToAction("Index");
        }

        public JsonResult getTaiKhoan(string term, int page = 1, string id = "")
        {
            var list_tai_khoan =
                TaiKhoanRepository.Instance.Search(term, "", page, out long total_recs, out string msg, page_size);
            return Json(new { data = list_tai_khoan, total = total_recs, page_size = page_size, page = page });
        }

        public JsonResult getTaiKhoanByIdCuocThi(string id)
        {
            List<DanhSach> lst_map = new List<DanhSach>();
            var list_tai_khoan = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id);
            var lst_thuoc_tinh_cuoc_thi = list_tai_khoan.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();
            //var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_cuoc_thi.Distinct(), LoaiThuocTinh.DANH_SACH_THI);
            foreach (var item in list_tai_khoan)
            {
                var cuoc_thi = CuocThiRepository.Instance.GetById(item?.id_cuoc_thi);
                var tai_khoan = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(item?.id_tai_khoan);
                DanhSach map = new DanhSach(cuoc_thi, tai_khoan, item);
                lst_map.Add(map);
            }

            return Json(new { data = lst_map });
        }
    }
}