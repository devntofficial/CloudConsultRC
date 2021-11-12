﻿// <auto-generated />
using System;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    [DbContext(typeof(ConsultationDbContext))]
    partial class ConsultationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CloudConsult.Consultation.Domain.Entities.ConsultationBookingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingEndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("BookingStartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DiagnosisReportId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsConsultationComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDiagnosisReportGenerated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPaymentComplete")
                        .HasColumnType("bit");

                    b.Property<string>("PatentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TimeSlotId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ConsultationBookings");
                });

            modelBuilder.Entity("CloudConsult.Consultation.Domain.Entities.DoctorAvailabilityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BookedPatientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BookingDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBooked")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TimeSlotEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeSlotStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DoctorAvailabilities");
                });
#pragma warning restore 612, 618
        }
    }
}
