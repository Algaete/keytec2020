using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class EstadoDispositivos
    {
        public string EstSn { get; set; }
        public int? EstEstado { get; set; }
        public DateTime? EstUltimoReporte { get; set; }
        public string EstIp { get; set; }
        public int? EstCantUsuarios { get; set; }
        public int? EstCantHuellas { get; set; }
        public int? EstCantRostros { get; set; }
        public int? EstSucursal { get; set; }
        public string EstHost { get; set; }
        public string EstVersionFw { get; set; }
        public int? EstCantMarcas { get; set; }
        public int? EstVersionAlgoritmoHuella { get; set; }
        public int? EstVersionAlgoritmoRostro { get; set; }
        public int? EstCantFuncionesSoportadas { get; set; }
        public int? EstCantRostrosEnrolamiento { get; set; }
        public string EstEstadoCarga { get; set; }
        public string EstBateriaRestante { get; set; }
        public string EstTeamViewerId { get; set; }
        public DateTime? EstUltimaActualizacionHora { get; set; }
    }
}
