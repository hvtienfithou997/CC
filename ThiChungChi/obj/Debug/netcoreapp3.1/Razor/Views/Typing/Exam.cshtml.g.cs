#pragma checksum "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8719588e56288cc18387c0332cb174d22107b4a9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Typing_Exam), @"mvc.1.0.view", @"/Views/Typing/Exam.cshtml")]
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
#nullable restore
#line 1 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
using System.Security.Cryptography;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8719588e56288cc18387c0332cb174d22107b4a9", @"/Views/Typing/Exam.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"294febe7b546febb0fa3c6bffafdc5c827f3b846", @"/Views/_ViewImports.cshtml")]
    public class Views_Typing_Exam : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/typing.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/speedtest.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
  
    ViewData["Title"] = "Exam";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n");
#nullable restore
#line 8 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
     if (!string.IsNullOrEmpty(ViewBag.ten))
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3>Cuộc thi: ");
#nullable restore
#line 10 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
                 Write(ViewBag.ten);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 11 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3>Luyện kỹ năng gõ phím</h3>\r\n");
#nullable restore
#line 15 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n");
#nullable restore
#line 19 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
         if (!string.IsNullOrEmpty(ViewBag.noi_dung))
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <p>");
#nullable restore
#line 21 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
          Write(Html.Raw(ViewBag.noi_dung));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n");
#nullable restore
#line 22 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>\r\n<div class=\"row main-layout\">\r\n    <div class=\"col-sm-12 col-md-12 col-lg-12\" id=\"speedtest-main\">\r\n\r\n        <span id=\"config_input_key\" value=\"32\" style=\"display:none;\"></span>\r\n        <span id=\"speedtest_mode\"");
            BeginWriteAttribute("value", " value=\"", 732, "\"", 740, 0);
            EndWriteAttribute();
            WriteLiteral(@" style=""display:none;""></span>
        <div id=""on-top"">
        </div>

        <div class=""row d-none"" style=""margin-bottom: 8px;"">
            <div class=""col-md-6 col-sm-8 col-xs-12"">
                <div class=""btn-group"" style=""margin-left:-12px; margin-right: 20px;""><a href=""/login"" class=""btn btn-primary"" role=""button"">Đăng nhập</a></div>
                <div class=""btn-group"" style=""margin-left:-12px;""><a class=""btn btn-md btn-success"" href=""#"" id=""switch-typing-test-language"">Tiếng Việt <span class=""caret""></span></a></div>
");
            WriteLiteral(@"            </div>
            <div class=""col-md-6 col-sm-4 hidden-xs"">
                <label class=""control-label"" style=""margin-top: 10px; font-size:0.9em; font-weight:normal;""></label>
            </div>
        </div>

        <div id=""reload-box"">
            <div id=""words"" class=""row""><div id=""row1""></div></div>

            <div id=""input-row"" class=""row"">
                <div style=""max-width: 620px; margin: auto;"">
                    <div style=""margin-right: 137px;""><input type=""text"" class=""form-control"" id=""inputfield""");
            BeginWriteAttribute("value", " value=\"", 2002, "\"", 2010, 0);
            EndWriteAttribute();
            WriteLiteral(" dir=\"ltr\"");
            BeginWriteAttribute("placeholder", " placeholder=\"", 2021, "\"", 2035, 0);
            EndWriteAttribute();
            WriteLiteral(@" autocomplete=""off"" autocorrect=""off"" autocapitalize=""off"" spellcheck=""false"" /></div>
                    <div style=""float:right; width: 130px; margin-top: -50px;"">
                        <div style=""width: 70px; float: left; margin-right: 7px; cursor: pointer; cursor: hand;"" id=""timer""");
            BeginWriteAttribute("class", " class=\"", 2328, "\"", 2336, 0);
            EndWriteAttribute();
            WriteLiteral(@" title=""click to show/hide countdown"">1:00</div>
                        <div style=""width: 50px; float: left; /*margin-top: -5px;*/display: block;"">
                        </div>
                    </div>
                    <div style=""float:right; padding-right: 58px; padding-top: 5px;"">
                        <button type=""button"" class=""btn btn-primary btn-lg d-none"" id=""reload-btn""><span class=""glyphicon glyphicon-refresh"">Làm lại</span></button>
                        <button type=""button"" class=""btn btn-primary btn-lg"" disabled id=""complete-btn"" onclick=""completeTest()""><span");
            BeginWriteAttribute("class", " class=\"", 2937, "\"", 2945, 0);
            EndWriteAttribute();
            WriteLiteral(@">Nộp bài</span></button>
                    </div>
                </div>
            </div>
        </div>

        <div id=""inputstream"" style=""display:none;""></div>
        <div id=""wordlist"" style=""display:none;""></div>
    </div>
</div>

<div id=""auswertung-result"" class=""col-md-12"" style=""display: block;"">
");
#nullable restore
#line 69 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
     if (ViewBag.cuoc_thi != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <input id=\'cuoc_thi\' class=\"d-none\"");
            BeginWriteAttribute("value", " value=\"", 3359, "\"", 3384, 1);
#nullable restore
#line 71 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
WriteAttributeValue("", 3367, ViewBag.cuoc_thi, 3367, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
#nullable restore
#line 72 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"

    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 74 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
     if (ViewBag.tai_khoan != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <input id=\'tai_khoan\' class=\"d-none\"");
            BeginWriteAttribute("value", " value=\"", 3485, "\"", 3511, 1);
#nullable restore
#line 76 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
WriteAttributeValue("", 3493, ViewBag.tai_khoan, 3493, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
#nullable restore
#line 77 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 78 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
     if (ViewBag.de_thi != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <input id=\'de_thi\' class=\"d-none\"");
            BeginWriteAttribute("value", " value=\"", 3604, "\"", 3627, 1);
#nullable restore
#line 80 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
WriteAttributeValue("", 3612, ViewBag.de_thi, 3612, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
#nullable restore
#line 81 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8719588e56288cc18387c0332cb174d22107b4a911450", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
#nullable restore
#line 84 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8719588e56288cc18387c0332cb174d22107b4a913305", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
#nullable restore
#line 85 "F:\ThiChungChi\ThiChungChi\Views\Typing\Exam.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
