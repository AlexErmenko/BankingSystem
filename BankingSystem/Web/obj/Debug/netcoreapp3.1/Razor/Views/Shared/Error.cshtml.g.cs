#pragma checksum "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\Shared\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5417516e588923247d766bd2f3b3c4acbf3c0d0c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Error), @"mvc.1.0.view", @"/Views/Shared/Error.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 2 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\_ViewImports.cshtml"
using ApplicationCore.Entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\_ViewImports.cshtml"
using Web.ViewModels.BankAccount;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\_ViewImports.cshtml"
using React.AspNet;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\Shared\Error.cshtml"
using System;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5417516e588923247d766bd2f3b3c4acbf3c0d0c", @"/Views/Shared/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"78b9c648680585b7739e687958d1e38cf09fc9d0", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Models.ErrorViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\Shared\Error.cshtml"
  
	ViewData[index: "Title"] = "Error";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1 class=\"text-danger\">Error.</h1>\r\n<h2 class=\"text-danger\">An error occurred while processing your request.</h2>\r\n\r\n");
#nullable restore
#line 10 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\Shared\Error.cshtml"
 if(Model.ShowRequestId)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<p>\r\n        <strong>Request ID:</strong> <code>");
#nullable restore
#line 13 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\Shared\Error.cshtml"
                                      Write(Model.RequestId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</code>\r\n    </p>\r\n");
#nullable restore
#line 15 "E:\PROJECTS\ASPNetCore\BankingSystem\BankingSystem\Web\Views\Shared\Error.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h3>Development Mode</h3>
<p>
    Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.
</p>
<p>
    <strong>The Development environment shouldn't be enabled for deployed applications.</strong>
    It can result in displaying sensitive information from exceptions to end users.
    For local debugging, enable the <strong>Development</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>
    and restarting the app.
</p>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Models.ErrorViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
