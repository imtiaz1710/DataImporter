#pragma checksum "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\Dashboard\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e883d3b57e4fa2b237d23b44565446b477a2ce67"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_Index), @"mvc.1.0.view", @"/Views/Dashboard/Index.cshtml")]
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
#line 1 "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\_ViewImports.cshtml"
using DataImporter;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\_ViewImports.cshtml"
using DataImporter.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e883d3b57e4fa2b237d23b44565446b477a2ce67", @"/Views/Dashboard/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"25578fd916149676d26c6a8b992de2915f402918", @"/Views/_ViewImports.cshtml")]
    public class Views_Dashboard_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DataImporter.Models.Dashboard.DashboardListModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\Dashboard\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""content-header"">
    <div class=""container-fluid"">
        <div class=""row mb-2"">
            <div class=""col-sm-6"">
                <h1 class=""m-0"">Dashboard</h1>
            </div><!-- /.col -->
            <div class=""col-sm-6"">
                <ol class=""breadcrumb float-sm-right"">
                    <li class=""breadcrumb-item""><a href=""#"">Home</a></li>
                    <li class=""breadcrumb-item active"">Dashboard</li>
                </ol>
            </div><!-- /.col -->
        </div><!-- /.row -->
    </div><!-- /.container-fluid -->
</div>

<section class=""content"">
    <div class=""container-fluid"">
        <!-- Small boxes (Stat box) -->
        <div class=""row"">
            <div class=""col-lg-4 col-6"">
                <!-- small box -->
                <div class=""small-box bg-info"">
                    <div class=""inner"">
                        <h3>");
#nullable restore
#line 30 "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\Dashboard\Index.cshtml"
                       Write(Model.GroupCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n\r\n                        <p>Groups</p>\r\n                    </div>\r\n                    <div class=\"icon\">\r\n                        <i class=\"ion\"></i>\r\n                    </div>\r\n");
            WriteLiteral(@"                </div>
            </div>
            <!-- ./col -->
            <div class=""col-lg-4 col-6"">
                <!-- small box -->
                <div class=""small-box bg-success"">
                    <div class=""inner"">
                        <h3>");
#nullable restore
#line 45 "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\Dashboard\Index.cshtml"
                       Write(Model.PendingImportCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n\r\n                        <p>Pending Imports</p>\r\n                    </div>\r\n                    <div class=\"icon\">\r\n                        <i class=\"ion\"></i>\r\n                    </div>\r\n");
            WriteLiteral(@"                </div>
            </div>
            <!-- ./col -->
            <div class=""col-lg-4 col-6"">
                <!-- small box -->
                <div class=""small-box bg-warning"">
                    <div class=""inner"">
                        <h3>");
#nullable restore
#line 60 "G:\RepoForInterview\DataImporter\DataImporter\DataImporter\Views\Dashboard\Index.cshtml"
                       Write(Model.PendingExportCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n\r\n                        <p>Pending Exports</p>\r\n                    </div>\r\n                    <div class=\"icon\">\r\n                        <i class=\"ion\"></i>\r\n                    </div>\r\n");
            WriteLiteral("                </div>\r\n            </div>\r\n            <!-- ./col -->\r\n            <!-- ./col -->\r\n        </div>\r\n        <!-- /.row -->\r\n    </div><!-- /.container-fluid -->\r\n</section>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DataImporter.Models.Dashboard.DashboardListModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
