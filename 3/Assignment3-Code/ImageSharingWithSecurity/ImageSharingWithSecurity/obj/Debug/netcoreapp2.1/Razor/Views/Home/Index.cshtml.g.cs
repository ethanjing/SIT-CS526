#pragma checksum "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "df4c46b3a0b27e5a216b4b21f0e3ca91ee524649"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 1 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\_ViewImports.cshtml"
using ImageSharingWithSecurity;

#line default
#line hidden
#line 2 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\_ViewImports.cshtml"
using ImageSharingWithSecurity.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"df4c46b3a0b27e5a216b4b21f0e3ca91ee524649", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2866ac68f4b151f85647edc6a447cdf66bd31a17", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Index.cshtml"
  
    ViewBag.Title = "Index";

#line default
#line hidden
            BeginContext(37, 6, true);
            WriteLiteral("\r\n<h2>");
            EndContext();
            BeginContext(44, 13, false);
#line 5 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Index.cshtml"
Write(ViewBag.Title);

#line default
#line hidden
            EndContext();
            BeginContext(57, 21, true);
            WriteLiteral("</h2>\r\n\r\n<p>Welcome, ");
            EndContext();
            BeginContext(79, 16, false);
#line 7 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Index.cshtml"
       Write(ViewBag.Username);

#line default
#line hidden
            EndContext();
            BeginContext(95, 7, true);
            WriteLiteral("!</p>\r\n");
            EndContext();
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
