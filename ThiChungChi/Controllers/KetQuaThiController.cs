using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using ThiChungChi.Models;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class KetQuaThiController : BaseController
    {
        public IActionResult Index(string id, string id_cuoc_thi, string term, long ngay_thi_tu, long ngay_thi_den, int page = 1, List<int> thuoc_tinh = null)
        {
            try
            {
                ViewBag.ngay_thi_tu = ngay_thi_tu;
                ViewBag.ngay_thi_den = ngay_thi_den;
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.KET_QUA_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                ViewBag.thuoc_tinh_search = BuildThuocTinhForSearch(lst_thuoc_tinh);
                ViewBag.Term = term;
                ViewBag.id_cuoc_thi = id_cuoc_thi;
                var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" }, is_admin);
                ViewBag.cuoc_thi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                ViewBag.PageSize = page_size;
                ViewBag.Page = page;
                ViewBag.tt = thuoc_tinh;
                var lst_ket_qua_thi = KetQuaThiRepository.Instance.Search(term, id_cuoc_thi, user(), ngay_thi_tu, ngay_thi_den, thuoc_tinh, page, out var total_recs,
                    out string msg, page_size, is_admin);
                var lst_thuoc_tinh_kq_thi = lst_ket_qua_thi.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();
                var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_kq_thi.Distinct(), LoaiThuocTinh.KET_QUA_THI);
                List<KetQuaThiMap> lst_map = new List<KetQuaThiMap>();
                if (!string.IsNullOrEmpty(id))
                {
                    var ket_qua_thi_da_thi = KetQuaThiRepository.Instance.LayDanhSachKetQuaThi(id, user());
                    foreach (var item in ket_qua_thi_da_thi)
                    {
                        var tai_khoan = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(item?.id_tai_khoan);
                        var cuoc_thi = CuocThiRepository.Instance.GetById(item?.id_cuoc_thi);
                        KetQuaThiMap map = new KetQuaThiMap(item, tai_khoan, cuoc_thi, lst_tt_by_gia_tri);
                        lst_map.Add(map);
                    }
                    ViewBag.Total = lst_map.Count;
                    return View(lst_map);
                }
                else
                {
                    foreach (var item in lst_ket_qua_thi)
                    {
                        var tai_khoan = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(item?.id_tai_khoan);
                        var cuoc_thi = CuocThiRepository.Instance.GetById(item?.id_cuoc_thi);
                        KetQuaThiMap map = new KetQuaThiMap(item, tai_khoan, cuoc_thi, lst_tt_by_gia_tri);
                        lst_map.Add(map);
                    }
                    ViewBag.Total = total_recs;
                    return View(lst_map);
                }
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
                var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                var all_tai_khoan = TaiKhoanRepository.Instance.GetAll(new List<string>(), new[] { "username", "username" });
                ViewBag.TaiKhoan = all_tai_khoan.Select(o => new SelectListItem { Value = o.username, Text = o.username }).ToList();
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.KET_QUA_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create(KetQuaThi kq)
        {
            try
            {
                if (is_admin)
                {
                    //kq.ngay_thi = Convert.ToInt32(XMedia.XUtil.TimeInEpoch(cv_ngay_thi));
                    SetMetaData(kq, false);
                    var ket_qua_thi = KetQuaThiRepository.Instance.Index(kq);
                    if (ket_qua_thi)
                    {
                        SetAlert("Thêm kết quả thi thành công", "success");
                    }
                    else
                    {
                        SetAlert("Thêm kết quả thi thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Thêm kết quả thi thất bại, bạn không có quyền", "error");
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
            if (!is_admin)
                return RedirectToAction("Index");
            var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.KET_QUA_THI, user(), is_admin);
            ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
            var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
            ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
            var all_tai_khoan = TaiKhoanRepository.Instance.GetAll(new List<string>(), new[] { "id", "username" });
            ViewBag.TaiKhoan = all_tai_khoan.Select(o => new SelectListItem { Value = o.username, Text = o.username }).ToList();
            var kq = KetQuaThiRepository.Instance.GetById(id);
            ViewBag.ngay_thi = kq.ngay_thi;
            return View(kq);
        }

        [HttpPost]
        public IActionResult Edit(KetQuaThi kq)
        {
            try
            {
                if (is_admin)
                {
                    //kq.ngay_thi = Convert.ToInt32(XMedia.XUtil.TimeInEpoch(Convert.ToDateTime(ngay_thi)));
                    SetMetaData(kq, true);
                    var ket_qua_thi = KetQuaThiRepository.Instance.Update(kq);
                    if (ket_qua_thi)
                    {
                        SetAlert("Sửa kết quả thi thành công", "success");
                    }
                    else
                    {
                        SetAlert("Sửa kết quả thi thất bại", "error");
                    }

                    if (ModelState.IsValid)
                    {
                        var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.KET_QUA_THI, user(), is_admin);
                        ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                        var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                        ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                        var all_tai_khoan = TaiKhoanRepository.Instance.GetAll(new List<string>(), new[] { "id", "username" });
                        ViewBag.TaiKhoan = all_tai_khoan.Select(o => new SelectListItem { Value = o.username, Text = o.username }).ToList();
                    }
                }
                else
                {
                    SetAlert("Sửa kết quả thi thất bại, bạn không có quyền", "error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        public IActionResult Details(string id)
        {
            var kq = KetQuaThiRepository.Instance.GetById(id);
            var cuoc_thi = CuocThiRepository.Instance.GetById(kq.id_cuoc_thi)?.ten;
            kq.id_cuoc_thi = cuoc_thi;
            return View(kq);
        }

        public IActionResult Delete(string id)
        {
            if (is_admin)
            {
                var kq = KetQuaThiRepository.Instance.Delete(id);
                if (kq)
                {
                    SetAlert("Xóa thành công", "success");
                }
                else
                {
                    SetAlert("Xóa thất bại", "error");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult LuuThuocTinh(string id_obj, List<int> thuoc_tinh)
        {
            var tt_ket_qua = KetQuaThiRepository.Instance.UpdateThuocTinh(id_obj, thuoc_tinh);
            if (tt_ket_qua)
            {
                SetAlert("Thành công", "success");
            }
            else
            {
                SetAlert("Thất bại", "error");
            }
            return RedirectToAction("Index");
        }

        private string Replace(string Source, string Find, string Replace)
        {
            string result = "";
            if (!string.IsNullOrEmpty(Source))
            {
                int Place = Source.IndexOf(Find);
                result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            }

            return result;
        }

        public IActionResult TaiKhoanChungChi(string id)
        {
            var tkcc = TaiKhoanChungChiRepository.Instance.GetById(id);
            try
            {
                if (is_admin || tkcc?.id_tai_khoan == user() || allow_create)
                {
                    var ket_qua_thi = KetQuaThiRepository.Instance.GetById(id);
                    if (ket_qua_thi != null)
                    {
                        string id_cuoc_thi = ket_qua_thi.id_cuoc_thi;
                        ViewBag.diem_thi = ket_qua_thi.diem_thi;
                        var cuoc_thi = CuocThiRepository.Instance.GetById(id_cuoc_thi);
                        ViewBag.ten_cuoc_thi = cuoc_thi?.ten;
                        string fullname = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(tkcc.id_tai_khoan).fullname;
                        ViewBag.fullname = fullname;
                        string ngay_cap = XMedia.XUtil.EpochToTimeString(tkcc.ngay_cap, "dd/MM/yyyy");
                        string noi_dung_chung_chi = ChungChiRepository.Instance.GetById(cuoc_thi.id_chung_chi)?.noi_dung;
                        string hang_cuoc_thi_text = "";

                        var hang_cuoc_thi = cuoc_thi.kieu_ket_qua.FirstOrDefault(x => x.max > ket_qua_thi.diem_thi && x.min <= ket_qua_thi.diem_thi);
                        if (hang_cuoc_thi != null)
                        {
                            hang_cuoc_thi_text = hang_cuoc_thi.ten;
                        }

                        var replacements = new[]{
                            new{Find="[DIEMTHI]", Replace = tkcc.noi_dung},
                            new{Find="[CUOCTHI]", Replace = cuoc_thi.ten},
                            new{Find="[TEN]", Replace = fullname},
                            new{Find="[NGAYCAP]" , Replace = ngay_cap},
                            new{Find="[HANG]" , Replace = hang_cuoc_thi_text}
                        };
                        

                        foreach (var set in replacements)
                        {
                            noi_dung_chung_chi = noi_dung_chung_chi.Replace(set.Find, set.Replace);
                        }

                        tkcc.noi_dung = noi_dung_chung_chi;

                        if (tkcc == null)
                            SetAlert("Cuộc thi này chưa có cấu hình kết quả", "error");
                        return View(tkcc);
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View(tkcc);
        }

        [HttpPost]
        public IActionResult TaiKhoanChungChi(TaiKhoanChungChi tkcc)
        {
            if (is_admin || tkcc.id_tai_khoan == user() || allow_create)
            {
                if (tkcc != null)
                {
                    SetMetaData(tkcc, true);
                    if (TaiKhoanChungChiRepository.Instance.Update(tkcc))
                    {
                        SetAlert("Lưu thành công", "success");
                    }
                    else
                    {
                        SetAlert("Lưu thất bại", "error");
                    }
                }
                return View(tkcc);
            }
            return RedirectToAction("Index");
        }

        public IActionResult KetQuaCuocThi(string id, int page = 1)
        {
            try
            {
                long total_recs = 0;
                if (is_admin || allow_create)
                {
                    
                    ViewBag.id_cuoc_thi = id;
                    page_size = 50;
                    ViewBag.PageSize = page_size;
                    ViewBag.Page = page;
                    ViewBag.ten_cuoc_thi = CuocThiRepository.Instance.GetById(id)?.ten;
                    List<KetQuaThi> lst_ket_qua_thi = new List<KetQuaThi>(); 
                    if(is_admin)
                        lst_ket_qua_thi = KetQuaThiRepository.Instance.Search("", id, user(), 0, 0, new List<int>(), page, out total_recs,
                            out string msg, page_size, is_admin);
                    if(allow_create)
                        lst_ket_qua_thi = KetQuaThiRepository.Instance.Search("", id, "", 0, 0, new List<int>(), page, out total_recs,
                            out string msg, page_size, false);
                    var lst_thuoc_tinh_kq_thi = lst_ket_qua_thi.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();
                    var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_kq_thi.Distinct(), LoaiThuocTinh.KET_QUA_THI);
                    List<KetQuaThiMap> lst_map = new List<KetQuaThiMap>();
                    ViewBag.dem_thi_sinh = lst_ket_qua_thi.Select(x => x.id_tai_khoan).Distinct().ToList().Count;
                    foreach (var item in lst_ket_qua_thi)
                    {
                        var tai_khoan = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(item?.id_tai_khoan);
                        var cuoc_thi = CuocThiRepository.Instance.GetById(item?.id_cuoc_thi);
                        KetQuaThiMap map = new KetQuaThiMap(item, tai_khoan, cuoc_thi, lst_tt_by_gia_tri);
                        lst_map.Add(map);
                    }
                    ViewBag.Total = total_recs;
                    return View(lst_map.OrderByDescending(x => x.diem_thi));
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }
    }
}