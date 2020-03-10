using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KeytecAdministración.Models
{
    public partial class TransaccionesContext : DbContext
    {
        public TransaccionesContext()
        {
        }

        public TransaccionesContext(DbContextOptions<TransaccionesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EstadoDispositivos> EstadoDispositivos { get; set; }
        public virtual DbSet<HistoricoEstadoDispositivos> HistoricoEstadoDispositivos { get; set; }
        public virtual DbSet<Inicializacion> Inicializacion { get; set; }
        public virtual DbSet<KeyStandbyDeamon> KeyStandbyDeamon { get; set; }
        public virtual DbSet<KeyTransaccion> KeyTransaccion { get; set; }
        public virtual DbSet<Transacciones> Transacciones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("initial catalog=Transacciones; Data Source= keycloud-prod.database.windows.net; Connection Timeout=30; User Id = appkey; Password=Kkdbc36de$; Min Pool Size=20; Max Pool Size=200;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstadoDispositivos>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ESTADO_DISPOSITIVOS");

                entity.HasIndex(e => e.EstSn)
                    .HasName("indx_estadoDispo");

                entity.Property(e => e.EstBateriaRestante)
                    .HasColumnName("EST_BATERIA_RESTANTE")
                    .HasMaxLength(250);

                entity.Property(e => e.EstCantFuncionesSoportadas).HasColumnName("EST_CANT_FUNCIONES_SOPORTADAS");

                entity.Property(e => e.EstCantHuellas).HasColumnName("EST_CANT_HUELLAS");

                entity.Property(e => e.EstCantMarcas).HasColumnName("EST_CANT_MARCAS");

                entity.Property(e => e.EstCantRostros).HasColumnName("EST_CANT_ROSTROS");

                entity.Property(e => e.EstCantRostrosEnrolamiento).HasColumnName("EST_CANT_ROSTROS_ENROLAMIENTO");

                entity.Property(e => e.EstCantUsuarios).HasColumnName("EST_CANT_USUARIOS");

                entity.Property(e => e.EstEstado).HasColumnName("EST_ESTADO");

                entity.Property(e => e.EstEstadoCarga)
                    .HasColumnName("EST_ESTADO_CARGA")
                    .HasMaxLength(250);

                entity.Property(e => e.EstHost)
                    .HasColumnName("EST_HOST")
                    .HasMaxLength(250);

                entity.Property(e => e.EstIp)
                    .HasColumnName("EST_IP")
                    .HasMaxLength(50);

                entity.Property(e => e.EstSn)
                    .HasColumnName("EST_SN")
                    .HasMaxLength(50);

                entity.Property(e => e.EstSucursal).HasColumnName("EST_SUCURSAL");

                entity.Property(e => e.EstTeamViewerId)
                    .HasColumnName("EST_TEAM_VIEWER_ID")
                    .HasMaxLength(250);

                entity.Property(e => e.EstUltimaActualizacionHora)
                    .HasColumnName("EST_ULTIMA_ACTUALIZACION_HORA")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstUltimoReporte)
                    .HasColumnName("EST_ULTIMO_REPORTE")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstVersionAlgoritmoHuella).HasColumnName("EST_VERSION_ALGORITMO_HUELLA");

                entity.Property(e => e.EstVersionAlgoritmoRostro).HasColumnName("EST_VERSION_ALGORITMO_ROSTRO");

                entity.Property(e => e.EstVersionFw)
                    .HasColumnName("EST_VERSION_FW")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<HistoricoEstadoDispositivos>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HISTORICO_ESTADO_DISPOSITIVOS");

                entity.Property(e => e.HisDetalle)
                    .HasColumnName("HIS_DETALLE")
                    .HasMaxLength(250);

                entity.Property(e => e.HisHoraDesconexion)
                    .HasColumnName("HIS_HORA_DESCONEXION")
                    .HasColumnType("datetime");

                entity.Property(e => e.HisId)
                    .HasColumnName("HIS_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.HisIp)
                    .HasColumnName("HIS_IP")
                    .HasMaxLength(250);

                entity.Property(e => e.HisSn)
                    .HasColumnName("HIS_SN")
                    .HasMaxLength(250);

                entity.Property(e => e.HisSucursal).HasColumnName("HIS_SUCURSAL");
            });

            modelBuilder.Entity<Inicializacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INICIALIZACION");

                entity.Property(e => e.IniApiKey)
                    .HasColumnName("INI_API_KEY")
                    .HasMaxLength(50);

                entity.Property(e => e.IniEstado).HasColumnName("INI_ESTADO");

                entity.Property(e => e.IniFondo).HasColumnName("INI_FONDO");

                entity.Property(e => e.IniId).HasColumnName("INI_ID");

                entity.Property(e => e.IniLogo).HasColumnName("INI_LOGO");

                entity.Property(e => e.IniModelo)
                    .HasColumnName("INI_MODELO")
                    .HasMaxLength(50);

                entity.Property(e => e.IniSn)
                    .HasColumnName("INI_SN")
                    .HasMaxLength(50);

                entity.Property(e => e.IniSucursal)
                    .HasColumnName("INI_SUCURSAL")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<KeyStandbyDeamon>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("KEY_STANDBY_DEAMON");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Dni)
                    .HasColumnName("DNI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("ESTADO");

                entity.Property(e => e.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mensaje)
                    .HasColumnName("MENSAJE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sn)
                    .HasColumnName("SN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tries).HasColumnName("TRIES");
            });

            modelBuilder.Entity<KeyTransaccion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("KEY_TRANSACCION");

                entity.HasIndex(e => new { e.Machinealias, e.Machineid, e.TraActivo, e.TraClave, e.TraDatoscara, e.TraDatoshuella, e.TraDetalle, e.TraId, e.TraIndicededo, e.TraIp, e.TraLargodatoscara, e.TraNombre, e.TraNumtarjeta, e.TraPrivilegio, e.TraRut, e.TraTerminal, e.IdSucursal, e.Instancia, e.TraEstado, e.TraTransaccion })
                    .HasName("nci_wi_KEY_TRANSACCION_0ACB30643C242224C55920C2F7CA59D4");

                entity.Property(e => e.IdSucursal).HasColumnName("ID_SUCURSAL");

                entity.Property(e => e.Instancia)
                    .HasColumnName("INSTANCIA")
                    .HasMaxLength(20);

                entity.Property(e => e.Machinealias)
                    .HasColumnName("MACHINEALIAS")
                    .HasMaxLength(250);

                entity.Property(e => e.Machineid).HasColumnName("MACHINEID");

                entity.Property(e => e.ResCodigo).HasColumnName("RES_CODIGO");

                entity.Property(e => e.ResDetalle).HasColumnName("RES_DETALLE");

                entity.Property(e => e.TraActivo).HasColumnName("TRA_ACTIVO");

                entity.Property(e => e.TraClave)
                    .HasColumnName("TRA_CLAVE")
                    .HasMaxLength(250);

                entity.Property(e => e.TraDatoscara)
                    .HasColumnName("TRA_DATOSCARA")
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.TraDatoshuella)
                    .HasColumnName("TRA_DATOSHUELLA")
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.TraDetalle)
                    .HasColumnName("TRA_DETALLE")
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.TraEstado).HasColumnName("TRA_ESTADO");

                entity.Property(e => e.TraHora)
                    .HasColumnName("TRA_HORA")
                    .HasColumnType("datetime");

                entity.Property(e => e.TraId)
                    .HasColumnName("TRA_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TraIndicededo).HasColumnName("TRA_INDICEDEDO");

                entity.Property(e => e.TraIp)
                    .HasColumnName("TRA_IP")
                    .HasMaxLength(250);

                entity.Property(e => e.TraLargodatoscara).HasColumnName("TRA_LARGODATOSCARA");

                entity.Property(e => e.TraNombre)
                    .HasColumnName("TRA_NOMBRE")
                    .HasMaxLength(250);

                entity.Property(e => e.TraNumtarjeta).HasColumnName("TRA_NUMTARJETA");

                entity.Property(e => e.TraPrivilegio).HasColumnName("TRA_PRIVILEGIO");

                entity.Property(e => e.TraRut).HasColumnName("TRA_RUT");

                entity.Property(e => e.TraTerminal).HasColumnName("TRA_TERMINAL");

                entity.Property(e => e.TraTransaccion).HasColumnName("TRA_TRANSACCION");
            });

            modelBuilder.Entity<Transacciones>(entity =>
            {
                entity.HasKey(e => e.TraId)
                    .HasName("PK__TRANSACC__8637A444EF3A756D");

                entity.ToTable("TRANSACCIONES");

                entity.HasIndex(e => new { e.TraSn, e.TraEstado })
                    .HasName("SN_ESTADO_TRAN");

                entity.HasIndex(e => new { e.TraSn, e.TraEstado, e.TraTipo })
                    .HasName("SN_ESTADO_TIPO_TRAN");

                entity.HasIndex(e => new { e.TraDetalle, e.TraHoraInicio, e.TraMensaje, e.TraPrioridad, e.TraEstado, e.TraSn, e.TraTipo })
                    .HasName("nci_wi_TRANSACCIONES_C39321A21426A9893E00EDE5C3A50386");

                entity.Property(e => e.TraId).HasColumnName("TRA_ID");

                entity.Property(e => e.TraData1).HasColumnName("TRA_DATA_1");

                entity.Property(e => e.TraDetalle)
                    .HasColumnName("TRA_DETALLE")
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.TraEstado).HasColumnName("TRA_ESTADO");

                entity.Property(e => e.TraHoraActualizacion)
                    .HasColumnName("TRA_HORA_ACTUALIZACION")
                    .HasColumnType("datetime");

                entity.Property(e => e.TraHoraFin)
                    .HasColumnName("TRA_HORA_FIN")
                    .HasColumnType("datetime");

                entity.Property(e => e.TraHoraInicio)
                    .HasColumnName("TRA_HORA_INICIO")
                    .HasColumnType("datetime");

                entity.Property(e => e.TraMensaje)
                    .HasColumnName("TRA_MENSAJE")
                    .HasMaxLength(300);

                entity.Property(e => e.TraOrigen)
                    .HasColumnName("TRA_ORIGEN")
                    .HasMaxLength(150);

                entity.Property(e => e.TraPrioridad).HasColumnName("TRA_PRIORIDAD");

                entity.Property(e => e.TraSn)
                    .HasColumnName("TRA_SN")
                    .HasMaxLength(30);

                entity.Property(e => e.TraTipo).HasColumnName("TRA_TIPO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
