using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class KeyStandbyDeamon
    {
        public int Uid { get; set; }
        public string Sn { get; set; }
        public string Dni { get; set; }
        public int? Estado { get; set; }
        public string Mensaje { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Tries { get; set; }
    }
}
