using System;
using System.Collections.Generic;

namespace KeytecAdministración.Models
{
    public partial class Machines
    {
        public int Id { get; set; }
        public string MachineAlias { get; set; }
        public int ConnectType { get; set; }
        public string Ip { get; set; }
        public int? SerialPort { get; set; }
        public int? Port { get; set; }
        public int? Baudrate { get; set; }
        public int MachineNumber { get; set; }
        public bool IsHost { get; set; }
        public bool Enabled { get; set; }
        public string CommPassword { get; set; }
        public short? Uilanguage { get; set; }
        public short? DateFormat { get; set; }
        public short? InOutRecordWarn { get; set; }
        public short? Idle { get; set; }
        public short? Voice { get; set; }
        public short? Managercount { get; set; }
        public short? Usercount { get; set; }
        public short? Fingercount { get; set; }
        public short? SecretCount { get; set; }
        public string FirmwareVersion { get; set; }
        public string ProductType { get; set; }
        public short? LockControl { get; set; }
        public short? Purpose { get; set; }
        public int? ProduceKind { get; set; }
        public string Sn { get; set; }
        public string PhotoStamp { get; set; }
        public int? IsIfChangeConfigServer2 { get; set; }
        public int? Pushver { get; set; }
        public string IsAndroid { get; set; }
        public int? DisEstado { get; set; }
        public int? DisEnrolador { get; set; }
        public string Instancia { get; set; }
        public int? DisTipo { get; set; }
        public string Tipo { get; set; }
        public int? IdSucursal { get; set; }
        public int? ZonaId { get; set; }
        public int? MacCara { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public int? DisCantidadMarcas { get; set; }
        public string SnServicio { get; set; }
        public string EndpointDestino { get; set; }
        public int? IdRegion { get; set; }
    }
}
