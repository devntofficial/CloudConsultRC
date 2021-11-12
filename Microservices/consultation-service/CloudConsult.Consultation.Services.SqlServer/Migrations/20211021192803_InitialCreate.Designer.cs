﻿// <auto-generated />
using System;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    [DbContext(typeof(ConsultationDbContext))]
    [Migration("20211021192803_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CloudConsult.Consultation.Domain.Entities.DoctorAvailability", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BookedPatientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BookingDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Date")
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
