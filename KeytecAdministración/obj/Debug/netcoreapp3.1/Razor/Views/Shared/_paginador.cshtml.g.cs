#pragma checksum "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5b956acbb432f238bcbad230c35868c38a7bec7a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__paginador), @"mvc.1.0.view", @"/Views/Shared/_paginador.cshtml")]
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
#line 1 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\_ViewImports.cshtml"
using KeytecAdministración;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\_ViewImports.cshtml"
using KeytecAdministración.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5b956acbb432f238bcbad230c35868c38a7bec7a", @"/Views/Shared/_paginador.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6b0c0e01a18d4934a01e039c67a171fb4e5cc5e7", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__paginador : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<KeytecAdministración.Models.BaseModelo>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 3 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
  

    var cantidadPaginas = (int)Math.Ceiling((double)Model.TotalDeRegistros / Model.RegistrosPorPagina);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<!--Funcionalidad: Anterior y siguiente-->\n \n\n    <ul class=\"pagination\">\n");
#nullable restore
#line 12 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
         if (Model.PaginaActual == 1)
        {
            Model.ValoresQueryString["pagina"] = 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item disabled\"><span class=\"page-link\">");
#nullable restore
#line 15 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                                                              Write(Html.ActionLink("Anterior", null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\n");
#nullable restore
#line 16 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
        }
        else
        {
            Model.ValoresQueryString["pagina"] = Model.PaginaActual - 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li>");
#nullable restore
#line 20 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
           Write(Html.ActionLink("Anterior", null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\n");
#nullable restore
#line 21 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
#nullable restore
#line 23 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
         if (Model.PaginaActual == cantidadPaginas)
        {
            Model.ValoresQueryString["pagina"] = cantidadPaginas;

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li page-item disabled style=\"color:black;\">");
#nullable restore
#line 26 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                                                   Write(Html.ActionLink("Siguiente", null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\n");
#nullable restore
#line 27 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
        }
        else
        {
            Model.ValoresQueryString["pagina"] = Model.PaginaActual + 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item\" style=\"color:black\"><span class=\"page-link\"> ");
#nullable restore
#line 31 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                                                                          Write(Html.ActionLink("Siguiente", null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\n");
#nullable restore
#line 32 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </ul>\n\n\n<!--Funcionalidad: Páginas-->\n\n");
#nullable restore
#line 38 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
  
    int Inicial = 1;
    var radio = 3;
    var cantidadMaximaDePaginas = radio * 2 + 1;
    int Final = (cantidadPaginas > cantidadMaximaDePaginas) ? cantidadMaximaDePaginas : cantidadPaginas;
    if (Model.PaginaActual > radio + 1)
    {
        Inicial = Model.PaginaActual - radio;
        if (cantidadPaginas > Model.PaginaActual + radio)
        {
            Final = Model.PaginaActual + radio;
        }
        else
        {
            Final = cantidadPaginas;
        }
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\n<ul class=\"pagination\">\n");
#nullable restore
#line 59 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
      Model.ValoresQueryString["pagina"] = 1; 

#line default
#line hidden
#nullable disable
            WriteLiteral("    <li><span class=\"page-link\">");
#nullable restore
#line 60 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                           Write(Html.ActionLink("Primera", null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\n");
#nullable restore
#line 61 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
     for (int i = Inicial; i <= Final; i++)
    {
        Model.ValoresQueryString["pagina"] = i;
        if (i == Model.PaginaActual)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"active\"><span class=\"sr-only\">");
#nullable restore
#line 66 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                                                Write(Html.ActionLink(i.ToString(), null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\n");
#nullable restore
#line 67 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li><span class=\"page-link\">");
#nullable restore
#line 70 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                                   Write(Html.ActionLink(i.ToString(), null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></li>\n");
#nullable restore
#line 71 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
        }
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 73 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
      Model.ValoresQueryString["pagina"] = cantidadPaginas; 

#line default
#line hidden
#nullable disable
            WriteLiteral("    <li><span class=\"page-link\">");
#nullable restore
#line 74 "C:\Users\Alfonso\Desktop\Keytec2020\KeytecAdministración\Views\Shared\_paginador.cshtml"
                           Write(Html.ActionLink("Ultima", null, Model.ValoresQueryString));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span></li>
</ul>
<script src=""https://code.jquery.com/jquery-3.4.1.min.js"" crossorigin=""anonymous""></script>
<script src=""https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"" crossorigin=""anonymous""></script>
<script src=""js/scripts.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.8.0/Chart.min.js"" crossorigin=""anonymous""></script>
<script src=""assets/demo/chart-area-demo.js""></script>
<script src=""wwwroot/assets/demo/chart-bar-demo.js""></script>
<script src=""https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"" crossorigin=""anonymous""></script>
<script src=""https://cdn.datatables.net/1.10.20/js/dataTables.bootstrap4.min.js"" crossorigin=""anonymous""></script>
<script src=""assets/demo/datatables-demo.js""></script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<KeytecAdministración.Models.BaseModelo> Html { get; private set; }
    }
}
#pragma warning restore 1591
