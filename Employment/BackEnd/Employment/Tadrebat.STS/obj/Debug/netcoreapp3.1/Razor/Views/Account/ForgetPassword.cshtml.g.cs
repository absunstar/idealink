#pragma checksum "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Account\ForgetPassword.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02ad6c9ee9aa89e589d135d9912fdd31888e11c8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_ForgetPassword), @"mvc.1.0.view", @"/Views/Account/ForgetPassword.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"02ad6c9ee9aa89e589d135d9912fdd31888e11c8", @"/Views/Account/ForgetPassword.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"57b49bb18fbe88f2fb7e20eb130e64338d7f2c37", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Account_ForgetPassword : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IdentityServer4.Quickstart.UI.ModelForgetPassword>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_ValidationSummary", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"container-fluid\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "02ad6c9ee9aa89e589d135d9912fdd31888e11c83468", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
    <div class=""row justify-content-md-center my-5"">
        <div class=""col col-lg-5"">
            <div class=""card"">

                <h6 class=""card-header color-text-01 bb-color-01 text-center text-uppercase"">
                    Forgotten your password?
                </h6>

                <div class=""card-body pt-3 pb-5"">
                    <p class=""mb-3 text-center"">
                        Enter your email address and we'll send you a link to reset your
                        password
                    </p>
");
#nullable restore
#line 18 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Account\ForgetPassword.cshtml"
                     using (Html.BeginForm("ForgetPassword", "Account"))
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <fieldset>\r\n                            <div class=\"form-group md-form\">\r\n                                <div class=\"prefix\"><i class=\"fal fa-at\"></i></div>\r\n                                ");
#nullable restore
#line 23 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Account\ForgetPassword.cshtml"
                           Write(Html.EditorFor(model => model.Email, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                ");
#nullable restore
#line 24 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Account\ForgetPassword.cshtml"
                           Write(Html.ValidationMessageFor(model => model.Email, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </div>\r\n                            <center>\r\n                                <button class=\"btn btn-outline-color-01 btn-rounded\" name=\"button\" value=\"ForgetPassword\">Send</button>\r\n                                ");
#nullable restore
#line 28 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Account\ForgetPassword.cshtml"
                           Write(Html.ActionLink("Cancel", "Login", "Account", null, new { @class = "btn btn-outline-color-01 btn-rounded " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </center>\r\n                        </fieldset>\r\n");
#nullable restore
#line 31 "C:\Users\absunstar\Desktop\FinalHandOver-idealink\Employment\BackEnd\Employment\Tadrebat.STS\Views\Account\ForgetPassword.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </div>\r\n\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IdentityServer4.Quickstart.UI.ModelForgetPassword> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
