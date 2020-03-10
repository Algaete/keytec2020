using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class Inicializacion
    {
        public int? IniId { get; set; }
        public int? IniEstado { get; set; }
        public string IniModelo { get; set; }
        public string IniSucursal { get; set; }
        public string IniApiKey { get; set; }
        public string IniLogo { get; set; }
        public string IniFondo { get; set; }
        public string IniSn { get; set; }
    }
}
