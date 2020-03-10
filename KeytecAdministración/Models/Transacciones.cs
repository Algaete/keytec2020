using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class Transacciones
    {
        public int TraId { get; set; }
        public int? TraTipo { get; set; }
        public int? TraEstado { get; set; }
        public string TraDetalle { get; set; }
        public string TraSn { get; set; }
        public string TraMensaje { get; set; }
        public DateTime? TraHoraInicio { get; set; }
        public DateTime? TraHoraFin { get; set; }
        public string TraData1 { get; set; }
        public DateTime? TraHoraActualizacion { get; set; }
        public int? TraPrioridad { get; set; }
        public string TraOrigen { get; set; }
    }
}
