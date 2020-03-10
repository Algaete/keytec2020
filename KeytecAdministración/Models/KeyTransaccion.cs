using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class KeyTransaccion
    {
        public int TraId { get; set; }
        public string TraIp { get; set; }
        public int? TraRut { get; set; }
        public string TraNombre { get; set; }
        public string TraClave { get; set; }
        public int? TraPrivilegio { get; set; }
        public int? TraNumtarjeta { get; set; }
        public int? TraActivo { get; set; }
        public int? TraIndicededo { get; set; }
        public string TraDatoshuella { get; set; }
        public string TraDatoscara { get; set; }
        public int? TraLargodatoscara { get; set; }
        public int? TraEstado { get; set; }
        public string TraDetalle { get; set; }
        public DateTime? TraHora { get; set; }
        public string Instancia { get; set; }
        public int? TraTerminal { get; set; }
        public int? TraTransaccion { get; set; }
        public int? IdSucursal { get; set; }
        public int? ResCodigo { get; set; }
        public string ResDetalle { get; set; }
        public int? Machineid { get; set; }
        public string Machinealias { get; set; }
    }
}
