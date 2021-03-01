using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Web.Mvc;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;
using Microsoft.AspNetCore.Authorization;

namespace ThiChungChi.Controllers
{
    public class TaiKhoanChungChiController : BaseController
    {
        public IActionResult Index()
        {
            //var tai_khoan = TaiKhoanChungChiRepository.Instance
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.CuocThi = CuocThiRepository.Instance.GetAll(new List<string>(), new string[] { "id", "ten" }).Select(o => new SelectListItem { Value = o.id, Text = o.ten }).ToList();
            ViewBag.TaiKhoan = TaiKhoanRepository.Instance.GetAll(new List<string>(), new string[] { "id", "username" });
            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Create(TaiKhoanChungChi tkcc)
        {
            SetMetaData(tkcc, false);
            var tai_khoan_cc = TaiKhoanChungChiRepository.Instance.Index(tkcc);
            if (tai_khoan_cc)
            {
                SetAlert($"Tạo chứng chỉ cho tài khoản {tkcc.id_tai_khoan} thành công", "success");
            }
            else
            {
                SetAlert("Tạo mới thất bại", "error");
            }
            return View();
        }

        public IActionResult Edit(string id)
        {
            var tkcc = TaiKhoanChungChiRepository.Instance.GetById(id);
            return View(tkcc);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Edit(TaiKhoanChungChi tkcc)
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ViewCC(string id)
        {
            var tkcc = TaiKhoanChungChiRepository.Instance.GetById(id);
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View(tkcc);
        }

    }
}