using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class DeThiController : BaseController
    {
        public IActionResult Index(string term, int page = 1)
        {
            if (!is_admin)
                return RedirectToAction("Index", "Home");
            ViewBag.Term = term;
            var lst_de_thi = DeThiTypingRepository.Instance.Search(term, user(), page, out long total_recs,
                out string msg, page_size, is_admin);
            ViewBag.Total = total_recs;
            ViewBag.PageSize = page_size;
            ViewBag.Page = page;
            return View(lst_de_thi);
        }

        public IActionResult Create()
        {
            if (!is_admin)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(DeThiTyping de_thi)
        {
            if (!is_admin)
                return RedirectToAction("Index", "Home");

            if (!string.IsNullOrEmpty(de_thi.ten))
            {
                var strRegex = Regex.Replace(de_thi.word, @"[\""”“()!?:…,.]+", "");
                de_thi.word = strRegex.Replace(" ", "|").ToLower().Trim();
                SetMetaData(de_thi, false);
                var dk = DeThiTypingRepository.Instance.Index(de_thi);
                if (dk)
                {
                    SetAlert("Tạo đề thành công", "success");
                }
                else
                {
                    SetAlert("Tạo đề thất bai", "error");
                }
            }
            return View();
        }

        public IActionResult Edit(string id)
        {
            if (!is_admin)
                return RedirectToAction("Index", "Home");
            var de = DeThiTypingRepository.Instance.GetById(id);
            ViewBag.ten = de.ten;
            var words_split = de.word.Replace("|", " ");
            de.word = words_split;
            return View(de);
        }

        [HttpPost]
        public IActionResult Edit(DeThiTyping de)
        {
            if (is_admin)
            {
                if (!string.IsNullOrEmpty(de.word))
                {
                    var strRegex = Regex.Replace(de.word, @"[\""”“()!?:…,.]+", "");
                    de.word = strRegex.Replace(" ", "|").ToLower();

                    SetMetaData(de, true);
                    var de_thi = DeThiTypingRepository.Instance.Update(de);
                    if (de_thi)
                    {
                        SetAlert("Sửa đề thành công", "success");
                    }
                    else
                    {
                        SetAlert("Sửa đề thất bai", "error");
                    }
                }
            }

            return View();
        }

        public IActionResult Delete(string id)
        {
            if (is_admin)
            {
                var kq = DeThiTypingRepository.Instance.Delete(id);
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
    }
}