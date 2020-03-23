using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeytecAdministración.Models
{
    public class TablaMaquinas
    {
        public int Id { get; set; }
        public string MachineAlias { get; set; }
        public string Sn { get; set; }
        public string Instancia { get; set; }
        public int? IdSucursal { get; set; }
        public int MachineNumber { get; set; }
        public int? EstCantUsuarios { get; set; }
        public int? EstCantHuellas { get; set; }
        public int? EstCantRostros { get; set; }
        public string EstVersionFw { get; set; }
        public DateTime? EstUltimoReporte { get; set; }
        public int Estado { get; set; }
        public int? TraPendiente { get; set; }
        public int? PerfilPendiente { get; set; }
        public int? CarasPendiente { get; set; }
        public int? HuellasPendiente { get; set; }
        public int? ReinicioPendiente { get; set; }
        public int? OtrasPendiente { get; set; }
        public int? EliminadosPendiente { get; set; }
        public int? DescargaPendiente { get; set; }
        public int? IdRegion { get; set; }
    }
}