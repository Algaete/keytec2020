using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeytecAdministración.Models
{
    public class BaseModelo
    {
        public int PaginaActual { get; set; }
        public int TotalDeRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
        public RouteValueDictionary ValoresQueryString { get; set; }
        public RouteValueDictionary ValoresQueryString2 { get; set; }
    }
}
