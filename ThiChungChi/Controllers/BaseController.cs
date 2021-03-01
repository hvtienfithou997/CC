using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using ThiChungChiModels;

namespace ThiChungChiAPI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public int page_size = 20;
        private bool _is_admin;
        private bool _allow_create;
        public bool is_admin
        {
            get
            {
                try
                {
                    var ro = (int)Role.ADMIN;
                    if (User != null && User.Identity.IsAuthenticated)
                        _is_admin = User.IsInRole(ro.ToString());
                }
                catch (Exception)
                {
                }
                return _is_admin;
            }
        }

        public bool allow_create
        {
            get
            {
                try
                {
                    var ro = (int)Role.CREATE;
                    if (User != null && User.Identity.IsAuthenticated)
                        _allow_create = User.IsInRole(ro.ToString());
                }
                catch (Exception)
                {
                }
                return _allow_create;
            }
        }

        public BaseController()
        {
        }

        protected string user()
        {
            return HttpContext.User.Identity.Name;
        }

        protected void SetAlert(string msg, string type)
        {
            TempData["msg"] = msg;
            switch (type)
            {
                case "success":
                    TempData["msg_type"] = "alert-success";
                    break;
                case "warning":
                    TempData["msg_type"] = "alert-warning";
                    break;
                case "error":
                    TempData["msg_type"] = "alert-danger";
                    break;
            }
        }

        protected void SetMetaData(dynamic obj, bool is_update)
        {
            if (is_update)
            {
                obj.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
                obj.nguoi_sua = HttpContext.User.Identity.Name;
            }
            else
            {
                obj.ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
                obj.nguoi_tao = HttpContext.User.Identity.Name;
                obj.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
                obj.nguoi_sua = HttpContext.User.Identity.Name;
            }
        }

        protected string BuildThuocTinh(dynamic obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul class='list-groups list-items'>");

            foreach (var tt in obj)
            {
                sb.AppendFormat($"<li class='list-group-item'><input type='checkbox' class='' name='thuoc_tinh' value='{tt.gia_tri}'> {tt.ten}</li>");
            }
            sb.Append("</ul>");

            return sb.ToString();
        }

        protected string BuildThuocTinhForSearch(dynamic obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='row'>");

            foreach (var tt in obj)
            {
                sb.AppendFormat($"<div class='col-md-4'><input type='checkbox' class='' name='thuoc_tinh' value='{tt.gia_tri}'> {tt.ten}</div>");
            }
            sb.Append("</div>");

            return sb.ToString();
        }

        




    }

}