#pragma checksum "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "88ea2c0f3569b20adcb4071d8283097a22070698"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Paging), @"mvc.1.0.view", @"/Views/Shared/Paging.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88ea2c0f3569b20adcb4071d8283097a22070698", @"/Views/Shared/Paging.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"294febe7b546febb0fa3c6bffafdc5c827f3b846", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Paging : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
  
    var pageNumber = ViewBag.Page as int? ?? 1;
    var pageSize = ViewBag.PageSize as int?;
    var totalItem = ViewBag.Total as int? ?? ViewBag.Total;
    var totalPage = (int)Math.Ceiling((decimal)((double)totalItem / pageSize));
    var loai = Model.loai as int? ?? 0;
    var term = Model.term;


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 11 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
 if (pageNumber <= totalPage)
{
    var ext_space = false;

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"pagin\">\r\n\r\n        <ul class=\"number-page\">\r\n\r\n");
#nullable restore
#line 18 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
             if (pageNumber > 1)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li>\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 535, "\"", 618, 1);
#nullable restore
#line 21 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
WriteAttributeValue("", 542, Url.Action("Index", new {term = term, page = pageNumber - 1, loai = @loai}), 542, 76, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"icon-pre\">\r\n                        <i class=\"fal fa-angle-left\">Trước</i>\r\n                    </a>\r\n                </li>\r\n");
#nullable restore
#line 25 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 27 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
             if (totalPage > 4)
            {
                if (pageNumber - 1 > 0)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <li>\r\n                        <a");
            BeginWriteAttribute("href", " href=\"", 929, "\"", 1011, 1);
#nullable restore
#line 32 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
WriteAttributeValue("", 936, Url.Action("Index", new {term = term,page = pageNumber - 1, loai = @loai}), 936, 75, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 1012, "\"", 1020, 0);
            EndWriteAttribute();
            WriteLiteral(" class=\"nb-txt nb-hover \">");
#nullable restore
#line 32 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                                                                                                                                            Write(pageNumber - 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                    </li>\r\n");
#nullable restore
#line 34 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                }
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                 for (int i = 1; i <= totalPage; i++)
                {
                    if (i > 2 && i < totalPage - 1)
                    {
                        if (!ext_space)
                        {
                            ext_space = true;

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li><span");
            BeginWriteAttribute("title", " title=\"", 1418, "\"", 1426, 0);
            EndWriteAttribute();
            WriteLiteral(" class=\"nb-txt nb-hover font-weight-bold page-current\">");
#nullable restore
#line 42 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                                                                                                Write(pageNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n");
#nullable restore
#line 43 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                            if (pageNumber < totalPage)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <li>\r\n                                    <a");
            BeginWriteAttribute("href", " href=\"", 1671, "\"", 1753, 1);
#nullable restore
#line 46 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
WriteAttributeValue("", 1678, Url.Action("Index", new {term = term,page = pageNumber + 1, loai = @loai}), 1678, 75, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 1754, "\"", 1762, 0);
            EndWriteAttribute();
            WriteLiteral(" class=\"nb-txt nb-hover \">");
#nullable restore
#line 46 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                                                                                                                                                        Write(pageNumber + 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                                </li>\r\n");
#nullable restore
#line 48 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li>\r\n                                <b");
            BeginWriteAttribute("class", " class=\"", 1950, "\"", 1958, 0);
            EndWriteAttribute();
            WriteLiteral(">/ ");
#nullable restore
#line 50 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                                         Write(totalPage);

#line default
#line hidden
#nullable disable
            WriteLiteral(" &nbsp;</b>\r\n                            </li>\r\n");
#nullable restore
#line 52 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                        }
                    }
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 54 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                 
            }
            else
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 58 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                 for (int i = 1; i <= totalPage; i++)
                {
                    if (i > 2 && i < totalPage - 1)
                    {
                        if (!ext_space)
                        {
                            ext_space = true;

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li><span>...</span></li>\r\n");
#nullable restore
#line 66 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                        }
                    }
                    else
                    {
                        if (pageNumber == i)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li><span");
            BeginWriteAttribute("title", " title=\"", 2666, "\"", 2674, 0);
            EndWriteAttribute();
            WriteLiteral(" class=\"nb-txt nb-hover font-weight-bold page-current\">");
#nullable restore
#line 72 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                                                                                                Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\r\n");
#nullable restore
#line 73 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li>\r\n                                <a");
            BeginWriteAttribute("href", " href=\"", 2898, "\"", 2967, 1);
#nullable restore
#line 77 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
WriteAttributeValue("", 2905, Url.Action("Index", new {term = term,page = i, loai = @loai}), 2905, 62, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 2968, "\"", 2976, 0);
            EndWriteAttribute();
            WriteLiteral(" class=\"nb-txt nb-hover \">");
#nullable restore
#line 77 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                                                                                                                                      Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                            </li>\r\n");
#nullable restore
#line 79 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                        }
                    }
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 81 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
                 
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 84 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
             if (pageNumber < totalPage)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li>\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 3233, "\"", 3316, 1);
#nullable restore
#line 87 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
WriteAttributeValue("", 3240, Url.Action("Index", new {term = term, page = pageNumber + 1, loai = @loai}), 3240, 76, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"icon-pre\">\r\n                        <i class=\"fal fa-angle-right\">Tiếp</i>\r\n                    </a>\r\n                </li>\r\n");
#nullable restore
#line 91 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </ul>\r\n    </div>\r\n");
#nullable restore
#line 94 "F:\ThiChungChi\ThiChungChi\Views\Shared\Paging.cshtml"

}

#line default
#line hidden
#nullable disable
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
