#pragma checksum "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d3edb67bfbb6a1cf982e9c9c6f5c5b9fca2af51d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ChungChi_Details), @"mvc.1.0.view", @"/Views/ChungChi/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\ThiChungChi\ThiChungChi\Views\_ViewImports.cshtml"
using ThiChungChiAPI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\ThiChungChi\ThiChungChi\Views\_ViewImports.cshtml"
using ThiChungChiAPI.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d3edb67bfbb6a1cf982e9c9c6f5c5b9fca2af51d", @"/Views/ChungChi/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"294febe7b546febb0fa3c6bffafdc5c827f3b846", @"/Views/_ViewImports.cshtml")]
    public class Views_ChungChi_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ThiChungChi.Models.CCMap>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
  
    ViewData["Title"] = "Details";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Chi tiết chứng chỉ</h1>\r\n\r\n<div>\r\n    <hr />\r\n    <dl class=\"row\">\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 14 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ten));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 17 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayFor(model => model.ten));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 20 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.noi_dung));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 23 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayFor(model => model.noi_dung));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 27 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.mau_chung_chi));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 30 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayFor(model => model.mau_chung_chi));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 33 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.thuoc_tinh));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n");
#nullable restore
#line 36 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
             if (Model.thuoc_tinh != null)
            {
                foreach (var lst in Model.thuoc_tinh)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <p>-- ");
#nullable restore
#line 40 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
                     Write(lst.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n");
#nullable restore
#line 41 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
                }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 45 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ngay_tao));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 48 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(XMedia.XUtil.EpochToTimeStringShortFomart(Model.ngay_tao));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 51 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ngay_sua));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 54 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(XMedia.XUtil.EpochToTimeStringShortFomart(Model.ngay_sua));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 57 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.nguoi_tao));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 60 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayFor(model => model.nguoi_tao));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 63 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.nguoi_sua));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 66 "F:\ThiChungChi\ThiChungChi\Views\ChungChi\Details.cshtml"
       Write(Html.DisplayFor(model => model.nguoi_sua));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n</div>\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d3edb67bfbb6a1cf982e9c9c6f5c5b9fca2af51d9008", async() => {
                WriteLiteral("Quay lại danh sách");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ThiChungChi.Models.CCMap> Html { get; private set; }
    }
}
#pragma warning restore 1591