using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class KeyZonas
    {
        public int ZonId { get; set; }
        public string ZonNombre { get; set; }
        public string Instancia { get; set; }
        public int? IdSucursal { get; set; }
        public int? ZonCantidadRelojes { get; set; }
    }
}
