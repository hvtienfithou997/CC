using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ThiChungChi.Models;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class ChungChiController : BaseController
    {
        public IActionResult Index(string term = "", int page = 1, List<int> thuoc_tinh = null)
        {
            try
            {
                List<CCMap> lst_map = new List<CCMap>();
                ViewBag.Term = term;
                ViewBag.tt = thuoc_tinh;
                ViewBag.PageSize = page_size;
                ViewBag.Page = page;
                var lst_chung_chi = ChungChiRepository.Instance.Search(term, user(), thuoc_tinh, page, out var total_recs,
                    out string msg, page_size, is_admin);
                ViewBag.Total = total_recs;

                var lst_thuoc_tinh_chung_chi = lst_chung_chi.Where(x => x.thuoc_tinh != null).SelectMany(x => x.thuoc_tinh).ToList();

                var lst_tt_by_gia_tri = ThuocTinhRepository.Instance.GetManyByGiaTri(lst_thuoc_tinh_chung_chi.Distinct(), LoaiThuocTinh.CHUNG_CHI);

                var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CHUNG_CHI, user(), is_admin);
                ViewBag.thuoc_tinh_search = BuildThuocTinhForSearch(lst_thuoc_tinh);
                ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
                foreach (var item in lst_chung_chi)
                {
                    CCMap map = new CCMap(item, lst_tt_by_gia_tri);
                    lst_map.Add(map);
                }
                return View(lst_map);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        public IActionResult Create()
        {
            var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CHUNG_CHI, user(), is_admin);
            ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
            return View();
        }

        [HttpPost]
        public IActionResult Create(ChungChi cc)
        {
            try
            {
                if (is_admin)
                {
                    SetMetaData(cc, false);
                    var chung_chi = ChungChiRepository.Instance.Index(cc);
                    if (chung_chi)
                    {
                        SetAlert("Thêm chứng chỉ thành công", "success");
                    }
                    else
                    {
                        SetAlert("Thêm chứng chỉ thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Bạn không có quyền tạo mới", "error");
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
            var lst_thuoc_tinh = ThuocTinhRepository.Instance.GetThuocTinhByLoai(LoaiThuocTinh.CHUNG_CHI, user(), is_admin);
            ViewBag.thuoc_tinh = BuildThuocTinh(lst_thuoc_tinh);
            var chung_chi = ChungChiRepository.Instance.GetById(id);
            ViewBag.tt = chung_chi.thuoc_tinh;
            return View(chung_chi);
        }

        [HttpPost]
        public IActionResult Edit(ChungChi cc)
        {
            try
            {
                SetMetaData(cc, true);
                var chung_chi = ChungChiRepository.Instance.Update(cc);
                if (chung_chi)
                {
                    SetAlert("Sửa chứng chỉ thành công", "success");
                }
                else
                {
                    SetAlert("Sửa chứng chỉ thất bại", "error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            try
            {
                var kq = ChungChiRepository.Instance.Delete(id);
                if (!kq)
                {
                    SetAlert("Xóa chứng chỉ thất bại", "error");
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
            var chi_tiet = ChungChiRepository.Instance.GetById(id);
            if (chi_tiet.thuoc_tinh == null)
            {
                chi_tiet.thuoc_tinh = new List<int>();
            }
            var lst_thuoc_tinh =
                ThuocTinhRepository.Instance.GetManyByGiaTri(chi_tiet.thuoc_tinh, LoaiThuocTinh.CHUNG_CHI);

            CCMap map = new CCMap(chi_tiet, lst_thuoc_tinh);

            return View(map);
        }

        [HttpPost]
        public IActionResult LuuChungChi(string id_obj, List<int> thuoc_tinh)
        {
            try
            {
                var gt = ChungChiRepository.Instance.UpdateThuocTinh(id_obj, thuoc_tinh);
                if (gt)
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
    }
}