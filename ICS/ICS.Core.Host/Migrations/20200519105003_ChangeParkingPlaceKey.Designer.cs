﻿// <auto-generated />
using System;
using System.Net;
using ICS.Core.Host.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20200519105003_ChangeParkingPlaceKey")]
    partial class ChangeParkingPlaceKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Core")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PastName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("RFID")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ClientServiceGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<int>("ServiceGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ServiceGroupId");

                    b.HasIndex("ClientId", "ServiceGroupId")
                        .IsUnique();

                    b.ToTable("ClientServiceGroup");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.Cluster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<IPAddress>("IPAddress")
                        .HasColumnType("inet");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<int?>("SupervisorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SupervisorId");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ClusterKeepAlive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClusterId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClusterId");

                    b.ToTable("ClusterKeepAlives");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ClusterToken", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Value")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("ClusterTokens");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.Parking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClusterId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ClusterId");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Parkings");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MaxNumberDay")
                        .HasColumnType("integer");

                    b.Property<int>("MaxNumberPlaces")
                        .HasColumnType("integer");

                    b.Property<int?>("ModifyingId")
                        .HasColumnType("integer");

                    b.Property<int?>("ParkingId")
                        .HasColumnType("integer");

                    b.Property<int>("ReservationAllowed")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ModifyingId");

                    b.HasIndex("ParkingId");

                    b.ToTable("ParkingConfigurations");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingKeepAlive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ParkingCondition")
                        .HasColumnType("integer");

                    b.Property<int?>("ParkingId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ParkingId");

                    b.ToTable("ParkingKeepAlives");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingPlace", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("ParkingId")
                        .HasColumnType("integer");

                    b.Property<int>("Serial")
                        .HasColumnType("integer");

                    b.Property<int?>("ServiceGroupId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("ServiceGroupId");

                    b.HasIndex("ParkingId", "Level", "Serial")
                        .IsUnique();

                    b.ToTable("ParkingPlaces");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingPlaceKeepAlive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ParkingCondition")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ParkingPlaceUuid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ParkingPlaceUuid");

                    b.ToTable("ParkingPlaceKeepAlives");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ServiceGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("ServiceGroups");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.SessionChange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ParkingPlaceKeepAliveId")
                        .HasColumnType("integer");

                    b.Property<int>("SessionCondition")
                        .HasColumnType("integer");

                    b.Property<int>("SessionParkingId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ParkingPlaceKeepAliveId");

                    b.HasIndex("SessionParkingId");

                    b.HasIndex("SessionCondition", "SessionParkingId")
                        .IsUnique();

                    b.ToTable("SessionChange");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.SessionParking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("ParkingPlaceUuid")
                        .HasColumnType("uuid");

                    b.Property<int?>("ServiceGroupId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParkingPlaceUuid");

                    b.HasIndex("ServiceGroupId");

                    b.ToTable("SessionParkings");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PastName")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Supervisors");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ClientServiceGroup", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Client", "Client")
                        .WithMany("ServiceGroups")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ICS.Core.Host.Data.Entities.ServiceGroup", "ServiceGroup")
                        .WithMany("Clients")
                        .HasForeignKey("ServiceGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.Cluster", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Worker", "Supervisor")
                        .WithMany("ControlledСlusters")
                        .HasForeignKey("SupervisorId");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ClusterKeepAlive", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Cluster", "Cluster")
                        .WithMany("KeepAlives")
                        .HasForeignKey("ClusterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ClusterToken", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Cluster", "Cluster")
                        .WithOne("Token")
                        .HasForeignKey("ICS.Core.Host.Data.Entities.ClusterToken", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.Parking", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Cluster", "Cluster")
                        .WithMany("Parkings")
                        .HasForeignKey("ClusterId");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingConfiguration", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Worker", "Modifying")
                        .WithMany("ParkingConfigurations")
                        .HasForeignKey("ModifyingId");

                    b.HasOne("ICS.Core.Host.Data.Entities.Parking", "Parking")
                        .WithMany("ParkingConfigurations")
                        .HasForeignKey("ParkingId");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingKeepAlive", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Parking", "Parking")
                        .WithMany("ParkingKeepAlives")
                        .HasForeignKey("ParkingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingPlace", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.Parking", "Parking")
                        .WithMany("ParkingPlaces")
                        .HasForeignKey("ParkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ICS.Core.Host.Data.Entities.ServiceGroup", "ServiceGroup")
                        .WithMany()
                        .HasForeignKey("ServiceGroupId");
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.ParkingPlaceKeepAlive", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.ParkingPlace", "ParkingPlace")
                        .WithMany("ParkingPlaceKeepAlives")
                        .HasForeignKey("ParkingPlaceUuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.SessionChange", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.ParkingPlaceKeepAlive", "ParkingPlaceKeepAlive")
                        .WithMany()
                        .HasForeignKey("ParkingPlaceKeepAliveId");

                    b.HasOne("ICS.Core.Host.Data.Entities.SessionParking", "SessionParking")
                        .WithMany("SessionChanges")
                        .HasForeignKey("SessionParkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ICS.Core.Host.Data.Entities.SessionParking", b =>
                {
                    b.HasOne("ICS.Core.Host.Data.Entities.ParkingPlace", "ParkingPlace")
                        .WithMany("SessionParkings")
                        .HasForeignKey("ParkingPlaceUuid");

                    b.HasOne("ICS.Core.Host.Data.Entities.ServiceGroup", "ServiceGroup")
                        .WithMany("SessionParkings")
                        .HasForeignKey("ServiceGroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
