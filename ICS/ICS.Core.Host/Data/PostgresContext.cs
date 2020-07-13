using ICS.Core.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Core.Host.Data
{
    public class PostgresContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<ClusterToken> ClusterTokens { get; set; }
        public DbSet<ClusterKeepAlive> ClusterKeepAlives { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<ParkingKeepAlive> ParkingKeepAlives { get; set; }
        public DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public DbSet<ParkingPlaceKeepAlive> ParkingPlaceKeepAlives { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<SessionParking> SessionParkings { get; set; }
        public DbSet<ParkingConfiguration> ParkingConfigurations { get; set; }
        public DbSet<CredentialCard> CredentialCards { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages {get;set; }
        public DbSet<Participant> Participants { get; set; }

        public PostgresContext(DbContextOptions<PostgresContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Core");
            modelBuilder.Entity<Worker>()
                .HasMany(x => x.ControlledСlusters)
                .WithOne(x => x.Supervisor);
            modelBuilder.Entity<Cluster>()
                .HasOne(x => x.Token)
                .WithOne(x => x.Cluster)
                .HasForeignKey<ClusterToken>(x => x.ClusterUuid)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Cluster>()
                .HasMany(x => x.Parkings)
                .WithOne(x => x.Cluster);
            modelBuilder.Entity<Parking>()
                .HasMany(x => x.ParkingPlaces)
                .WithOne(x => x.Parking)
                .HasForeignKey(x => x.ParkingUuid)
                .HasPrincipalKey(x => x.Uuid)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Cluster>()
                .HasMany(x => x.KeepAlives)
                .WithOne(x => x.Cluster)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Parking>()
                .HasMany(x => x.ParkingKeepAlives)
                .WithOne(x => x.Parking)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ParkingPlace>()
                .HasMany(x => x.ParkingPlaceKeepAlives)
                .WithOne(x => x.ParkingPlace)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>()
                .HasMany(x => x.ServiceGroups)
                .WithOne(x => x.Client)
                .HasForeignKey(x => x.ClientUuid);
            modelBuilder.Entity<ServiceGroup>()
                .HasMany(x => x.Clients)
                .WithOne(x => x.ServiceGroup)
                .HasForeignKey(x => x.ServiceGroupUuid);
            modelBuilder.Entity<SessionParking>()
                .HasOne(x => x.ParkingPlace)
                .WithMany(x => x.SessionParkings);
            modelBuilder.Entity<SessionParking>()
                .HasOne(x => x.ServiceGroup)
                .WithMany(x => x.SessionParkings);
            modelBuilder.Entity<SessionParking>()
                .HasMany(x => x.SessionChanges)
                .WithOne(x => x.SessionParking)
                .HasForeignKey(x=>x.SessionParkingUuid);
            modelBuilder.Entity<Parking>()
                .HasMany(x => x.ParkingConfigurations)
                .WithOne(x => x.Parking);
            modelBuilder.Entity<Worker>()
                .HasMany(x => x.ParkingConfigurations)
                .WithOne(x => x.Modifying);
            modelBuilder.Entity<CredentialCard>()
                .HasOne(x => x.Client)
                .WithOne(x => x.CredentialCard);
            modelBuilder.Entity<SessionParking>()
                .HasMany(x => x.Dialogs)
                .WithOne(x => x.Session);
            modelBuilder.Entity<Dialog>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.Dialog);
            modelBuilder.Entity<Dialog>()
                .HasMany(x => x.Participants)
                .WithOne(x => x.Dialog)
                .HasForeignKey(x => x.DialogUuid);
            modelBuilder.Entity<Participant>()
                .HasMany(x => x.Dialogs)
                .WithOne(x => x.Participant)
                .HasForeignKey(x => x.ParticipantUuid);

            modelBuilder.Entity<Worker>()
                .HasIndex(x => x.Uuid)
                .IsUnique(true);
            modelBuilder.Entity<Cluster>()
                .HasIndex(x => x.Uuid)
                .IsUnique(true);
            modelBuilder.Entity<Parking>()
                .HasIndex(x => x.Uuid)
                .IsUnique(true);
            modelBuilder.Entity<Client>()
                .HasIndex(x => x.Uuid)
                .IsUnique(true);
            modelBuilder.Entity<ServiceGroup>()
                .HasIndex(x => x.Uuid)
                .IsUnique(true);
            modelBuilder.Entity<ClusterToken>()
                .HasIndex(x => x.Value)
                .IsUnique(true);
            modelBuilder.Entity<ParkingPlace>()
                .HasIndex(x => new { x.ParkingUuid, x.Level, x.Serial })
                .IsUnique(true);
            modelBuilder.Entity<ClientServiceGroup>()
                .HasIndex(x => new { x.ClientUuid, x.ServiceGroupUuid })
                .IsUnique(true);
            modelBuilder.Entity<SessionChange>()
                .HasIndex(x => new { x.SessionCondition, x.SessionParkingUuid })
                .IsUnique(true);
            modelBuilder.Entity<CredentialCard>()
                .HasIndex(x => x.Rfid)
                .IsUnique(true);
            modelBuilder.Entity<DialogParticipant>()
                .HasIndex(x => new { x.DialogUuid, x.ParticipantUuid })
                .IsUnique();

            modelBuilder.Entity<CredentialCard>()
                .Property(x => x.CardNumber).HasMaxLength(18).ValueGeneratedOnAdd();
            modelBuilder.Entity<CredentialCard>()
                .Property(x => x.Rfid).HasMaxLength(36).ValueGeneratedOnAdd();
        }
    }
}
