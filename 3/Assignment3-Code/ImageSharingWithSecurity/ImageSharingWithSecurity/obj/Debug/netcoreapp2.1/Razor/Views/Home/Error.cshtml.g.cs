#pragma checksum "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "45c60503d71074f3c84c71f53fdc4ae784fc3e96"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Error), @"mvc.1.0.view", @"/Views/Home/Error.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Error.cshtml", typeof(AspNetCore.Views_Home_Error))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"45c60503d71074f3c84c71f53fdc4ae784fc3e96", @"/Views/Home/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2866ac68f4b151f85647edc6a447cdf66bd31a17", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ImageSharingWithSecurity.Models.ErrorViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Error.cshtml"
  
    ViewData["Title"] = "Error";

#line default
#line hidden
            BeginContext(41, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(98, 120, true);
            WriteLiteral("\r\n<h1 class=\"text-danger\">Error.</h1>\r\n<h2 class=\"text-danger\">An error occurred while processing your request.</h2>\r\n\r\n");
            EndContext();
#line 10 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Error.cshtml"
 if (Model.ShowRequestId)
{

#line default
#line hidden
            BeginContext(248, 52, true);
            WriteLiteral("    <p>\r\n        <strong>Request ID:</strong> <code>");
            EndContext();
            BeginContext(301, 15, false);
#line 13 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Error.cshtml"
                                      Write(Model.RequestId);

#line default
#line hidden
            EndContext();
            BeginContext(316, 19, true);
            WriteLiteral("</code>\r\n    </p>\r\n");
            EndContext();
#line 15 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Error.cshtml"
}

#line default
#line hidden
            BeginContext(338, 53, true);
            WriteLiteral("\r\n<h3>Development Mode</h3>\r\n<p>\r\n    Error Message: ");
            EndContext();
            BeginContext(392, 11, false);
#line 19 "C:\Users\jing\Desktop\SIT\cs526\3\Assignment3-Code\ImageSharingWithSecurity\ImageSharingWithSecurity\Views\Home\Error.cshtml"
              Write(Model.ErrId);

#line default
#line hidden
            EndContext();
            BeginContext(403, 402, true);
            WriteLiteral(@"
</p>
<p>
    <strong>Development environment should not be enabled in deployed applications</strong>, as it can result in sensitive information from exceptions being displayed to end users. For local debugging, development environment can be enabled by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>, and restarting the application.
</p>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ImageSharingWithSecurity.Models.ErrorViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
