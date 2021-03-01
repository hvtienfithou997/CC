using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class TypingController : BaseController
    {
        public IActionResult Exam(string cuoc_thi, string pass_thi)
        {
            //if (!string.IsNullOrEmpty(cuoc_thi))

            ViewBag.cuoc_thi = cuoc_thi;
            ViewBag.tai_khoan = user();

            if (check_luot_thi(cuoc_thi))
                return RedirectToAction("MyExam", "DanhSachThi");

            if (!string.IsNullOrEmpty(cuoc_thi))
            {
                if (check_password(cuoc_thi, pass_thi))
                {
                    var de_thi = CuocThiRepository.Instance.GetById(cuoc_thi);
                    ViewBag.de_thi = de_thi.id_de_thi;
                    ViewBag.ten = de_thi.ten;

                    ViewBag.noi_dung = CuocThiRepository.Instance.GetById(cuoc_thi).noi_dung;
                }
                else
                {
                    SetAlert("Sai mật khẩu cuộc thi", "error");
                    return RedirectToAction("MyExam", "DanhSachThi", new { user = user(), pass = "wrong" });
                }
            }

            return View();
        }

        public bool check_password(string id, string pass_thi)
        {
            pass_thi = !string.IsNullOrEmpty(pass_thi) ? pass_thi : string.Empty;
            var cuoc_thi = CuocThiRepository.Instance.GetById(id);
            if (cuoc_thi != null)
            {
                if (string.IsNullOrEmpty(cuoc_thi.pass_thi))
                {
                    cuoc_thi.pass_thi = string.Empty;
                }

                return cuoc_thi.pass_thi.Equals(pass_thi, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public bool check_luot_thi(string cuoc_thi)
        {
            var so_lan_thi = CuocThiRepository.Instance.GetById(cuoc_thi)?.so_lan_thi_lai;
            var dem_ket_qua = KetQuaThiRepository.Instance.DemKetQuaThi(cuoc_thi, user());
            return dem_ket_qua >= so_lan_thi;
        }

        public JsonResult get_words(string de_thi, string cuoc_thi)
        {
            DeThiTyping get_de;
            if (!string.IsNullOrEmpty(de_thi))
            {
                get_de = check_luot_thi(cuoc_thi) ? null : DeThiTypingRepository.Instance.GetById(de_thi);
            }
            else
            {
                get_de = DeThiTypingRepository.Instance.GetRandomDeThi();
                get_de.time = Convert.ToInt32(XMedia.XUtil.ConfigurationManager.AppSetting["Times"]);
            }

            return Json(new { data = get_de });
        }

        public JsonResult auswertung(string cuoc_thi, string time_complete, string sz, string ez, string wordlist,
            string user_input, string backspace_counter, string afk_timer, string speedtest_id, string mode,
            string word_correct, string word_wrong)
        {
            try
            {
                var correct = word_correct;
                var wrong = word_wrong;
                var wpm = user_input.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                var count_wpm = wpm.Count;
                var total_count = 0;
                var find_de_thi =
                    DeThiTypingRepository.Instance.GetById(CuocThiRepository.Instance.GetById(cuoc_thi)?.id_de_thi);
                if (find_de_thi != null)
                {
                    total_count = find_de_thi.word.Replace("|", " ")
                        .Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().Count;
                }

                //mức độ hoàn thành
                var muc_do_hoan_thanh = Convert.ToDouble(correct) / Convert.ToDouble(total_count);
                // tỉ lệ chính xác
                var ti_le_go_dung = Convert.ToDouble(correct) / Convert.ToDouble(count_wpm);
                var ket_qua_thi_json = new
                {
                    //diem = (Convert.ToInt32(correct) * diem_1_tu),
                    diem = $"{muc_do_hoan_thanh * 100:F2}",
                    so_tu_tren_phut = count_wpm,
                    thoi_gian_hoan_thanh = time_complete,
                    chinh_xac = correct,
                    sai = wrong,
                    ti_le_go_dung = ti_le_go_dung * 100,
                    nguoi_dung_nhap = user_input,
                    dem_nut_xoa = backspace_counter,
                    tong_so_tu_tren_phut = count_wpm,
                    muc_do_hoan_thanh = muc_do_hoan_thanh * 100
                };

                string json_data = JsonConvert.SerializeObject(ket_qua_thi_json);
                if (!string.IsNullOrEmpty(cuoc_thi))
                {
                    if (!check_luot_thi(cuoc_thi))
                    {
                        var diem = $"{muc_do_hoan_thanh * 100:F2}";
                        InsertKetQuaThi(cuoc_thi, user(), json_data, Convert.ToDouble(diem));
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<h3>Kết quả </h3>");
                sb.Append("<table class='table table-striped' id='result-table'>");
                sb.Append("<tbody>");
                    sb.Append(
                        $"<tr id='wpm'><td class='name'>Số từ đã gõ: </td><td class='value'> <strong>{count_wpm}</strong></td></tr>");
                if (find_de_thi != null)
                    sb.Append(
                        $"<tr id='point'><td class='name'>Điểm đạt được: </td><td class='value'> <strong>{muc_do_hoan_thanh * 100:F2}</strong></td></tr>");
                sb.Append(
                    $"<tr id='correct'><td class='name'>Số từ gõ đúng: </td><td class='value'> <strong>{correct}</strong></td></tr>");
                sb.Append(
                    $"<tr id='wrong'><td class='name'>Số từ gõ sai: </td><td class='value'> <strong>{wrong}</strong></td></tr>");
                if (find_de_thi != null)
                    sb.Append(
                        $"<tr id='ratio'><td class='name'>Mức độ hoàn thành: </td><td class='value'> <strong>{muc_do_hoan_thanh * 100:F2}%</strong></td></tr>");
                sb.Append(
                    $"<tr id='time'><td class='name'>Thời gian hoàn thành (giây): </td><td class='value'> <strong>{time_complete}</strong></td></tr>");
                sb.Append("/<tbody>");
                sb.Append("<table>");

                return Json(new { result = sb.ToString() });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Json(new { result = string.Empty });
        }

        public IActionResult InsertKetQuaThi(string cuoc_thi, string tai_khoan, string json_ket_qua_thi, double diem_thi)
        {
            KetQuaThi kq = new KetQuaThi
            {
                id_tai_khoan = tai_khoan,
                id_cuoc_thi = cuoc_thi,
                diem_thi = diem_thi,
                ket_qua_thi_json = json_ket_qua_thi,
                ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now),
                ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now),
                nguoi_tao = tai_khoan,
                nguoi_sua = tai_khoan
            };

            if (KetQuaThiRepository.Instance.Index(kq, out string id))
            {
                var get_kieu_ket_qua = CuocThiRepository.Instance.GetById(kq.id_cuoc_thi)?.kieu_ket_qua;
                if (get_kieu_ket_qua != null)
                {
                    var noi_dung_kieu_du_lieu = get_kieu_ket_qua
                        .FirstOrDefault(x => x.min < kq.diem_thi && x.max > kq.diem_thi)?.ten;
                    if (string.IsNullOrEmpty(noi_dung_kieu_du_lieu))
                        noi_dung_kieu_du_lieu = string.Empty;
                    TaiKhoanChungChi tkcc = new TaiKhoanChungChi
                    {
                        id_ket_qua_thi = id,
                        id_tai_khoan = tai_khoan,
                        ngay_cap = kq.ngay_thi,
                        id = id,
                        noi_dung = noi_dung_kieu_du_lieu + $"(Điểm đạt được: {kq.diem_thi})",
                        ngay_tao = kq.ngay_tao,
                        ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now),
                        nguoi_tao = tai_khoan,
                        nguoi_sua = tai_khoan
                    };

                    TaiKhoanChungChiRepository.Instance.Index(tkcc);
                }
                return Json(new { msg = "Tạo kết quả thành công" });
            }

            return Json(new { msg = "Tạo kết quả thất bại" });
        }
    }
}