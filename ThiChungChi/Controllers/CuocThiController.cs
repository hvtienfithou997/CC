using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using ThiChungChi.Models;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class CuocThiController : BaseController
    {
        public IActionResult Index(string term, long ngay_thi_tu, long ngay_thi_den, int page = 1, List<int> thuoc_tinh = null)
        {
            if (is_admin || allow_create)
            {
                try
                {
                    long total_recs = 0;
                    ViewBag.ngay_thi_tu = ngay_thi_tu;
                    ViewBag.ngay_thi_den = ngay_thi_den;
                    var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CUOC_THI, user(), is_admin);
                    ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                    ViewBag.Term = term;
                    ViewBag.PageSize = page_size;
                    ViewBag.Page = page;
                    ViewBag.tt = thuoc_tinh;
                    var get_user_danh_sach_thi = DanhSachThiRepository.Instance.GetDanhSachThiByIdTaiKhoan(user());
                    var lst_should_id = get_user_danh_sach_thi.Select(x => x.id_cuoc_thi);
                    List<CuocThi> lst_cuoc_thi = new List<CuocThi>();
                    if (is_admin)
                    {
                        lst_cuoc_thi = CuocThiRepository.Instance.Search(term, new List<string>(), ngay_thi_tu, ngay_thi_den, user(), thuoc_tinh, page, out total_recs, out var msg, page_size, is_admin);
                    }
                    if (allow_create)
                    {
                        lst_cuoc_thi = CuocThiRepository.Instance.Search(term, new List<string>(), ngay_thi_tu, ngay_thi_den, user(), thuoc_tinh, page, out total_recs, out var msg, page_size, false);
                    }
                    var dic_cuoc_thi = lst_cuoc_thi.ToDictionary(x => x.id, y => y.ten);

                    ViewBag.thuoc_tinh_search = BuildThuocTinhForSearch(lst_thuoc_tinh);
                    List<CuocThiMap> lst_map = new List<CuocThiMap>();
                    var lst_thuoc_tinh_cuoc_thi = lst_cuoc_thi.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();
                    var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_cuoc_thi.Distinct(), LoaiThuocTinh.CUOC_THI);
                    if (lst_cuoc_thi.Count > 0)
                    {
                        var hour = (24 - DateTime.Now.Hour) - 2;
                        var add_h = DateTime.Now.AddHours(hour).AddMinutes(59).AddSeconds(59);
                        long ngay_hien_tai = XMedia.XUtil.TimeInEpoch(add_h);
                        var day_now = add_h.Day;
                        foreach (var item in lst_cuoc_thi)
                        {
                            var day_kt = XMedia.XUtil.EpochToTime(item.ngay_ket_thuc).Day;
                            bool expired = ngay_hien_tai > item.ngay_ket_thuc && day_now != day_kt;
                            var de_thi = DeThiTypingRepository.Instance.GetById(item.id_de_thi)?.ten;
                            CuocThiMap map = new CuocThiMap(item, lst_tt_by_gia_tri, dic_cuoc_thi, de_thi, expired);
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
            return RedirectToAction("MyExam", "DanhSachThi");
        }

        public IActionResult Create()
        {
            if (is_admin || allow_create)
            {
                var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" }, is_admin);
                ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CUOC_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                var de_thi = DeThiTypingRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                ViewBag.de_thi = de_thi.Select(x => new SelectListItem { Value = x.id, Text = x.ten }).ToList();
                var chung_chi = ChungChiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                ViewBag.chung_chi = chung_chi.Select(x => new SelectListItem { Value = x.id, Text = x.ten }).ToList();
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create([FromBody] object value)
        {
            string msg = "";
            bool success = false;

            var ct = JsonConvert.DeserializeObject<CuocThi>(value.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (ct == null || !ModelState.IsValid)
            {
                var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CUOC_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                return View(ct);
            }
            try
            {
                if (is_admin || allow_create)
                {
                    if (string.IsNullOrEmpty(ct.ten))
                    {
                        SetAlert("Chưa nhập tên cuộc thi", "error");
                        msg = "Chưa nhập tên cuộc thi";
                        success = false;
                    }
                    else
                    {
                        SetMetaData(ct, false);
                        if (CuocThiRepository.Instance.Index(ct))
                        {
                            SetAlert("Thêm cuộc thi thành công", "success");
                            msg = "Thêm cuộc thi thành công";
                            success = true;
                        }
                        else
                        {
                            SetAlert("Thêm cuộc thi thất bại", "error");
                            msg = "Thêm cuộc thi thất bại";
                            success = false;
                        }
                    }
                }
                else
                {
                    SetAlert("Thêm cuộc thi thất bại, bạn không có quyền!!!", "error");
                }
                return Json(new { newUrl = Url.Action("Create", "CuocThi"), msg = msg, success = success });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View();
        }

        public IActionResult Edit(string id)
        {
            if (is_admin || allow_create)
            {
                var cuoc_thi = CuocThiRepository.Instance.GetById(id);
                ViewBag.ngay_bd = cuoc_thi.ngay_bat_dau;
                ViewBag.ngay_kt = cuoc_thi.ngay_ket_thuc;
                ViewBag.loai = (int)cuoc_thi.loai_cuoc_thi;
                var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>() { id }, new string[] { "id", "ten" });
                ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CUOC_THI, user(), is_admin);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                ViewBag.id = id;
                var de_thi = DeThiTypingRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                ViewBag.de_thi = de_thi.Select(x => new SelectListItem { Value = x.id, Text = x.ten }).ToList();
                var tk = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id);
                ViewBag.tai_khoan_cuoc_thi = tk;
                ViewBag.tk = tk.Select(x => x.id_tai_khoan).ToList();
                ViewBag.tt = cuoc_thi.thuoc_tinh;
                var chung_chi = ChungChiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                ViewBag.chung_chi = chung_chi.Select(x => new SelectListItem { Value = x.id, Text = x.ten }).ToList();
                return View(cuoc_thi);
            }
            return RedirectToAction("MyExam", "DanhSachThi");
        }

        [HttpPost]
        public IActionResult Edit(string id, [FromBody] object value)
        {
            string msg = "";
            bool success = false;
            try
            {
                var ct = JsonConvert.DeserializeObject<CuocThi>(value.ToString());
                var obj = JToken.Parse(value.ToString());
                var list_tai_khoan_need = obj["list_tai_khoan_need"].Values<string>().ToList();
                ViewBag.id = id;
                ct.id = id;
                if (is_admin || allow_create)
                {
                    if (string.IsNullOrEmpty(ct.id_cha))
                        ct.id_cha = string.Empty;
                    var danh_sach_thi = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(ct.id);
                    var tai_khoan_danh_sach_thi = danh_sach_thi.Select(x => x.id_tai_khoan);
                    var except = tai_khoan_danh_sach_thi.Except(list_tai_khoan_need).ToList();

                    foreach (var ex in except)
                    {
                        var ds = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(ct.id, ex)?.id;
                        DanhSachThiRepository.Instance.Delete(ds);
                    }

                    if (list_tai_khoan_need.Count > 0)
                    {
                        var ds_thi = new DanhSachThi();
                        foreach (var tai_khoan in list_tai_khoan_need)
                        {
                            var danh_sach_thi_existed = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(ct.id, tai_khoan);
                            if (danh_sach_thi_existed == null)
                            {
                                ds_thi.id_cuoc_thi = ct.id;
                                ds_thi.id_tai_khoan = tai_khoan;
                                SetMetaData(ds_thi, false);
                                DanhSachThiRepository.Instance.Index(ds_thi);
                            }
                        }
                    }
                    else
                    {
                        foreach (var ds in danh_sach_thi)
                        {
                            DanhSachThiRepository.Instance.Delete(ds.id);
                        }
                    }

                    SetMetaData(ct, true);
                    ViewBag.ngay_bd = ct.ngay_bat_dau;
                    ViewBag.ngay_kt = ct.ngay_ket_thuc;
                    if (CuocThiRepository.Instance.Update(ct))
                    {
                        SetAlert("Sửa cuộc thi thành công", "success");
                        msg = "Sửa cuộc thi thành công";
                        success = true;
                    }
                    else
                    {
                        SetAlert("Sửa cuộc thi thất bại", "error");
                        msg = "Sửa cuộc thi thất bại";
                        success = false;
                    }
                    if (ModelState.IsValid)
                    {
                        var all_cuoc_thi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                        ViewBag.CuocThi = all_cuoc_thi.Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
                        var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CUOC_THI, user(), is_admin);
                        ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                        var de_thi = DeThiTypingRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" });
                        ViewBag.de_thi = de_thi.Select(x => new SelectListItem { Value = x.id, Text = x.ten }).ToList();
                        //SetAlert("Sửa cuộc thi thất bại", "error");
                        //return View((CuocThi)null);
                    }
                }
                else
                {
                    SetAlert("Sửa cuộc thi thất bại, bạn không có quyền!!!", "error");
                }

                return Json(new { newUrl = Url.Action("Edit", "CuocThi"), msg = msg, success = success });
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
                if (is_admin || allow_create)
                {
                    var kq = CuocThiRepository.Instance.Delete(id);
                    if (!kq)
                    {
                        SetAlert("Xóa cuộc thi thất bại", "error");
                    }
                    else
                    {
                        var danh_sach_thi = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id);
                        if (danh_sach_thi.Count > 0)
                        {
                            foreach (var ds in danh_sach_thi)
                            {
                                DanhSachThiRepository.Instance.Delete(ds.id);
                            }
                        }
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
            var chi_tiet_cuoc_thi = CuocThiRepository.Instance.GetById(id);
            var danh_muc_cha = CuocThiRepository.Instance.GetById(chi_tiet_cuoc_thi.id_cha)?.ten;
            chi_tiet_cuoc_thi.id_cha = !string.IsNullOrEmpty(danh_muc_cha) ? danh_muc_cha : "--- Chưa có ---";
            chi_tiet_cuoc_thi.pass_thi = !string.IsNullOrEmpty(chi_tiet_cuoc_thi.pass_thi)
                ? chi_tiet_cuoc_thi.pass_thi
                : "--- Chưa đặt mật khẩu ---";
            return View(chi_tiet_cuoc_thi);
        }

        [HttpPost]
        public IActionResult LuuChungChi(string id_obj, List<int> thuoc_tinh)
        {
            try
            {
                var ct = CuocThiRepository.Instance.UpdateThuocTinh(id_obj, thuoc_tinh);
                if (ct)
                {
                    SetAlert("Thành công", "success");
                }
                else
                {
                    SetAlert("Thất bại", "error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index");
        }

        public IActionResult GanUngVien(string id)
        {
            ViewBag.ten = CuocThiRepository.Instance.GetById(id)?.ten;
            var tk = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id);
            ViewBag.tai_khoan_cuoc_thi = tk;
            ViewBag.tk = tk.Select(x => x.id_tai_khoan).ToList();
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public IActionResult GanUngVien(string id, List<string> list_tai_khoan_need)
        {
            ViewBag.id = id;
            if (is_admin || allow_create)
            {
                var danh_sach_thi = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id);
                var tai_khoan_danh_sach_thi = danh_sach_thi.Select(x => x.id_tai_khoan);
                var except = tai_khoan_danh_sach_thi.Except(list_tai_khoan_need).ToList();

                foreach (var ex in except)
                {
                    var ds = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id, ex)?.id;
                    DanhSachThiRepository.Instance.Delete(ds);
                }

                if (list_tai_khoan_need.Count > 0)
                {
                    var ds_thi = new DanhSachThi();
                    foreach (var tai_khoan in list_tai_khoan_need)
                    {
                        var danh_sach_thi_existed = DanhSachThiRepository.Instance.GetDanhSachThiByIdCuocThi(id, tai_khoan);
                        if (danh_sach_thi_existed == null)
                        {
                            ds_thi.id_cuoc_thi = id;
                            ds_thi.id_tai_khoan = tai_khoan;
                            SetMetaData(ds_thi, false);
                            DanhSachThiRepository.Instance.Index(ds_thi);
                        }
                    }
                    SetAlert("Thêm thành công!", "success");
                }
                else
                {
                    foreach (var ds in danh_sach_thi)
                    {
                        DanhSachThiRepository.Instance.Delete(ds.id);
                    }
                    SetAlert("Bạn chưa chọn ứng viên nào, nên sẽ xóa hết danh sách", "warning");
                }
            }
            else
            {
                SetAlert("Thêm ứng viên thất bại, bạn không có quyền!!!", "error");
            }
            return View();
        }
    }
}