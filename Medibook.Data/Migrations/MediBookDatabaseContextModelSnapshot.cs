﻿// <auto-generated />
using System;
using MediBook.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediBook.Data.Migrations
{
    [DbContext(typeof(MediBookDatabaseContext))]
    partial class MediBookDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("MediBook.Core.Models.AppointmentSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("DurationInMins")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("MedicalPractitionerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("MedicalPractitionerId");

                    b.ToTable("AppointmentSessions");
                });

            modelBuilder.Entity("MediBook.Core.Models.AppointmentSlot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("AppointmentDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("AppointmentDurationInMins")
                        .HasColumnType("int");

                    b.Property<int?>("AppointmentSessionId")
                        .HasColumnType("int");

                    b.Property<int>("AppointmentState")
                        .HasColumnType("int");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentSessionId");

                    b.HasIndex("PatientId");

                    b.ToTable("AppointmentSlots");
                });

            modelBuilder.Entity("MediBook.Core.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Title")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MediBook.Core.Models.JobDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("JobDescriptions");
                });

            modelBuilder.Entity("MediBook.Core.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Address2")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("County")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("HealthAndCare")
                        .HasColumnType("bigint");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Title")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("MediBook.Core.Models.PatientNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("MedicalPractitionerId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("MedicalPractitionerId");

                    b.HasIndex("PatientId");

                    b.ToTable("PatientNotes");
                });

            modelBuilder.Entity("MediBook.Core.Models.PatientsMedicalPractitioner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("MedicalPractitionerId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicalPractitionerId");

                    b.HasIndex("PatientId");

                    b.ToTable("PatientsMedicalPractitioners");
                });

            modelBuilder.Entity("MediBook.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("JobDescriptionId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("JobDescriptionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MediBook.Core.Models.AppointmentSession", b =>
                {
                    b.HasOne("MediBook.Core.Models.Employee", null)
                        .WithMany("AppointmentSessions")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("MediBook.Core.Models.User", "MedicalPractitioner")
                        .WithMany()
                        .HasForeignKey("MedicalPractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalPractitioner");
                });

            modelBuilder.Entity("MediBook.Core.Models.AppointmentSlot", b =>
                {
                    b.HasOne("MediBook.Core.Models.AppointmentSession", null)
                        .WithMany("AppointmentSlots")
                        .HasForeignKey("AppointmentSessionId");

                    b.HasOne("MediBook.Core.Models.Patient", "Patient")
                        .WithMany("AppointmentSlots")
                        .HasForeignKey("PatientId");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MediBook.Core.Models.PatientNote", b =>
                {
                    b.HasOne("MediBook.Core.Models.Employee", null)
                        .WithMany("PatientNotes")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("MediBook.Core.Models.User", "MedicalPractitioner")
                        .WithMany()
                        .HasForeignKey("MedicalPractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediBook.Core.Models.Patient", "Patient")
                        .WithMany("PatientNotes")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalPractitioner");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MediBook.Core.Models.PatientsMedicalPractitioner", b =>
                {
                    b.HasOne("MediBook.Core.Models.Employee", "MedicalPractitioner")
                        .WithMany()
                        .HasForeignKey("MedicalPractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediBook.Core.Models.Patient", "Patient")
                        .WithMany("PatientsMedicalPractitioners")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalPractitioner");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MediBook.Core.Models.User", b =>
                {
                    b.HasOne("MediBook.Core.Models.Employee", "EmployeeDetails")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediBook.Core.Models.JobDescription", "JobDescription")
                        .WithMany()
                        .HasForeignKey("JobDescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmployeeDetails");

                    b.Navigation("JobDescription");
                });

            modelBuilder.Entity("MediBook.Core.Models.AppointmentSession", b =>
                {
                    b.Navigation("AppointmentSlots");
                });

            modelBuilder.Entity("MediBook.Core.Models.Employee", b =>
                {
                    b.Navigation("AppointmentSessions");

                    b.Navigation("PatientNotes");
                });

            modelBuilder.Entity("MediBook.Core.Models.Patient", b =>
                {
                    b.Navigation("AppointmentSlots");

                    b.Navigation("PatientNotes");

                    b.Navigation("PatientsMedicalPractitioners");
                });
#pragma warning restore 612, 618
        }
    }
}
