using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KeytecAdministración.Models
{
    public partial class ProductionsContext : DbContext
    {
        public ProductionsContext()
        {
        }

        public ProductionsContext(DbContextOptions<ProductionsContext> options)
            : base(options)
        {
        }

       
       
        public virtual DbSet<Machines> Machines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=keycloud-prod.database.windows.net;Initial Catalog=Produccion;User ID=appkey;Password=Kkdbc36de$;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                  

            modelBuilder.Entity<Machines>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("aaaaaMachines_PK")
                    .IsClustered(false);

                entity.HasIndex(e => new { e.Sn, e.Instancia })
                    .HasName("nci_wi_Machines_A1EC767933545B34ACEAA62ADCA5686C");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommPassword).HasMaxLength(12);

                entity.Property(e => e.DateFormat).HasDefaultValueSql("((-1))");

                entity.Property(e => e.DisCantidadMarcas).HasColumnName("DIS_CANTIDAD_MARCAS");

                entity.Property(e => e.DisEnrolador).HasColumnName("DIS_ENROLADOR");

                entity.Property(e => e.DisEstado).HasColumnName("DIS_ESTADO");

                entity.Property(e => e.DisTipo).HasColumnName("DIS_TIPO");

                entity.Property(e => e.EndpointDestino)
                    .HasColumnName("ENDPOINT_DESTINO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fingercount)
                    .HasColumnName("fingercount")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.FirmwareVersion).HasMaxLength(20);

                entity.Property(e => e.IdRegion).HasColumnName("id_region");

                entity.Property(e => e.IdSucursal).HasColumnName("ID_SUCURSAL");

                entity.Property(e => e.Idle).HasDefaultValueSql("((-1))");

                entity.Property(e => e.InOutRecordWarn).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Instancia)
                    .HasColumnName("INSTANCIA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(20);

                entity.Property(e => e.IsAndroid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsIfChangeConfigServer2).HasDefaultValueSql("((0))");

                entity.Property(e => e.Latitud)
                    .HasColumnName("LATITUD")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LockControl).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Longitud)
                    .HasColumnName("LONGITUD")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MacCara).HasColumnName("MAC_CARA");

                entity.Property(e => e.MachineAlias)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.Managercount)
                    .HasColumnName("managercount")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.PhotoStamp)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Port).HasDefaultValueSql("((1))");

                entity.Property(e => e.ProduceKind).HasDefaultValueSql("((1))");

                entity.Property(e => e.ProductType).HasMaxLength(20);

                entity.Property(e => e.Purpose).HasDefaultValueSql("((1))");

                entity.Property(e => e.Pushver)
                    .HasColumnName("pushver")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SecretCount).HasDefaultValueSql("((-1))");

                entity.Property(e => e.SerialPort).HasDefaultValueSql("((1))");

                entity.Property(e => e.Sn)
                    .HasColumnName("sn")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SnServicio)
                    .HasColumnName("SN_SERVICIO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("TIPO")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Uilanguage)
                    .HasColumnName("UILanguage")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.Usercount)
                    .HasColumnName("usercount")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.Voice).HasDefaultValueSql("((-1))");

                entity.Property(e => e.ZonaId).HasColumnName("ZONA_ID");
            });

           

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
