using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ThiChungChiAPI.Controllers;
using ThiChungChiES;
using ThiChungChiModels;

namespace ThiChungChi.Controllers
{
    public class TaiKhoanController : BaseController
    {
        public IActionResult Index(string term, int page = 1)
        {
            var list_tai_khoan =
                TaiKhoanRepository.Instance.Search(term, user(), page, out long total_recs, out string msg, page_size, is_admin);
            ViewBag.Total = total_recs;
            ViewBag.PageSize = page_size;
            ViewBag.Page = page;
            ViewBag.Term = term;
            return View(list_tai_khoan);
        }

        public IActionResult Create()
        {
            if (!is_admin)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(TaiKhoan tk)
        {
            try
            {
                if (is_admin)
                {
                    if (!TaiKhoanRepository.Instance.KiemTraTaiKhoanExist(tk.username))
                    {
                        SetMetaData(tk, false);
                        var acc = TaiKhoanRepository.Instance.Index(tk);
                        if (acc)
                        {
                            SetAlert("Thêm thành công", "success");
                        }
                        else
                        {
                            SetAlert("Thêm thất bại", "error");
                        }
                    }
                    else
                    {
                        SetAlert("Tài khoản đã tồn tại", "error");
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

        public IActionResult Edit(string id)
        {
            //if (!is_admin)
            //    return RedirectToAction("Index");
            var tai_khoan = TaiKhoanRepository.Instance.GetById(id);
            if (is_admin || tai_khoan?.username == user())
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.role = tai_khoan.role;
                    return View(tai_khoan);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(TaiKhoan tk)
        {
            SetMetaData(tk, true);
            if (!is_admin)
                tk.role = (int)Role.USER;
            var tai_khoan = TaiKhoanRepository.Instance.Update(tk);
            if (tai_khoan)
            {
                SetAlert("Sửa thành công", "success");
            }
            else
            {
                SetAlert("Sửa thất bại", "error");
            }

            return View();
        }

        public IActionResult Delete(string id)
        {
            if (is_admin)
            {
                var tai_khoan = TaiKhoanRepository.Instance.Delete(id);
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                TaiKhoan tk = new TaiKhoan() { username = username, email = "admin@chungchi.vn", id = username };

                string res = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = true, data = tk });

                var obj = JToken.Parse(res);
                if (obj != null)
                {
                    if (obj["success"].ToObject<bool>())
                    {
                        var account = obj["data"].ToObject<TaiKhoan>();
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, account.id));
                        identity.AddClaim(new Claim(ClaimTypes.Name, account.username));
                        identity.AddClaim(new Claim(ClaimTypes.GivenName, account.username));
                        identity.AddClaim(new Claim(ClaimTypes.Email, !string.IsNullOrEmpty(account.email) ? account.email : ""));
                        var find_user = TaiKhoanRepository.Instance.GetTaiKhoanByUsername(account.username);
                        if (find_user != null)
                        {
                            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, find_user.role.ToString()));
                            find_user.dang_nhap_cuoi = (int)XMedia.XUtil.TimeInEpoch(DateTime.Now);
                            TaiKhoanRepository.Instance.Update(find_user);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(account.username))
                            {
                                account.username = account.username.Trim();
                                TaiKhoanRepository.Instance.Index(account);
                            }
                        }
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddYears(1),
                            AllowRefresh = true
                        });
                        string ret = HttpContext.Request.Form["ReturnUrl"].ToString();
                        if (!string.IsNullOrEmpty(ret))
                            return Redirect(ret);
                        else
                            return Redirect("/");
                    }
                    else
                    {
                        ViewBag.error = obj["msg"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = $"{ex.Message}";
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}