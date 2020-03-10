using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class HistoricoEstadoDispositivos
    {
        public int HisId { get; set; }
        public string HisSn { get; set; }
        public int? HisSucursal { get; set; }
        public DateTime? HisHoraDesconexion { get; set; }
        public string HisDetalle { get; set; }
        public string HisIp { get; set; }
    }
}
