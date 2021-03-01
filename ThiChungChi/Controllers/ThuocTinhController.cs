using Microsoft.AspNetCore.Mvc;
using System;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class ThuocTinhController : BaseController
    {
        public IActionResult Index(string term = "", int loai = -1, int page = 1)
        {
            try
            {
                ViewBag.Loai = loai;
                ViewBag.Term = term;
                ViewBag.PageSize = page_size;
                ViewBag.Page = page;
                var lst_thuoc_tinh = ThuocTinhRepository.Instance.Search(term, loai,user(), page, out var total_recs,
                    out string msg, page_size, is_admin);
                ViewBag.Total = total_recs;
                return View(lst_thuoc_tinh);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        #region Tạo mới

        public IActionResult Create()
        {
            if (!is_admin)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ThuocTinh tt)
        {
            try
            {
                if (is_admin)
                {

                    SetMetaData(tt, false);
                    var kq = ThuocTinhRepository.Instance.Index(tt, out string msg);
                    if (kq)
                    {
                        SetAlert("Thêm thuộc tính thành công", "success");
                    }
                    else
                    {
                        SetAlert("Thêm thuộc tính thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Bạn không có quyền!", "error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View();
        }

        #endregion Tạo mới

        #region Sửa thuộc tính

        public IActionResult Edit(string id)
        {
            if (!is_admin)
                return RedirectToAction("Index", "Home");
            var thuoc_tinh = ThuocTinhRepository.Instance.GetById(id);
            return View(thuoc_tinh);
        }

        [HttpPost]
        public IActionResult Edit(ThuocTinh tt)
        {
            try
            {
                if (is_admin)
                {
                    SetMetaData(tt, true);
                    var kq = ThuocTinhRepository.Instance.Update(tt);
                    if (kq)
                    {
                        SetAlert("Sửa thuộc tính thành công", "success");
                    }
                    else
                    {
                        SetAlert("Sửa thuộc tính thất bại", "error");
                    }
                }
                else
                {
                    SetAlert("Bạn không có quyền ", "error");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            return View();
        }

        #endregion Sửa thuộc tính

        #region Xóa thuộc tính

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (is_admin)
            {
                var kq = ThuocTinhRepository.Instance.Delete(id);
                if (!kq)
                {
                    SetAlert("Xóa thuộc tính thất bại", "error");
                }
            }
            return RedirectToAction("Index");

        }

        #endregion Xóa thuộc tính

        public IActionResult Details(string id)
        {
            var chi_tiet = ThuocTinhRepository.Instance.GetById(id);
            return View(chi_tiet);
        }
    }
}