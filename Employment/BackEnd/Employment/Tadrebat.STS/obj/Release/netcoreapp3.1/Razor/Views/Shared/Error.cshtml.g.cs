#pragma checksum "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1c8d66e95d5afa2a608bb7f470163009119e6610"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Error), @"mvc.1.0.view", @"/Views/Shared/Error.cshtml")]
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
#line 1 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\_ViewImports.cshtml"
using IdentityServer4.Quickstart.UI;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1c8d66e95d5afa2a608bb7f470163009119e6610", @"/Views/Shared/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"57b49bb18fbe88f2fb7e20eb130e64338d7f2c37", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ErrorViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
  
    var error = Model?.Error?.Error;
    var errorDescription = Model?.Error?.ErrorDescription;
    var request_id = Model?.Error?.RequestId;

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"container-fluid\">\r\n\r\n    <div class=\"alert alert-danger\">\r\n        <strong>Error</strong>\r\n        Sorry, there was an error\r\n\r\n");
#nullable restore
#line 14 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
         if (error != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <strong>\r\n                <em>\r\n                    : ");
#nullable restore
#line 18 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
                 Write(error);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </em>\r\n            </strong>\r\n");
#nullable restore
#line 21 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"

            if (errorDescription != null)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div>");
#nullable restore
#line 24 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
                Write(errorDescription);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 25 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
            }
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n\r\n");
#nullable restore
#line 29 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
     if (request_id != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"request-id\">Request Id: ");
#nullable restore
#line 31 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
                                       Write(request_id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 32 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Shared\Error.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ErrorViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591