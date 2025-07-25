﻿// <auto-generated />
using System;
using API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(ProjectVContext))]
    partial class ProjectVContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Models.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdClient"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("ClientType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("IdClient");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("API.Models.Company", b =>
                {
                    b.Property<int>("IdCompany")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCompany"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdOwner")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("IdCompany");

                    b.HasIndex("IdOwner")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("API.Models.CompanyClients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdCompany")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCompany");

                    b.ToTable("CompanyClients");
                });

            modelBuilder.Entity("API.Models.Reschedule", b =>
                {
                    b.Property<int>("IdReschedule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReschedule"));

                    b.Property<int>("IdSchedule")
                        .HasColumnType("int");

                    b.Property<string>("RescheduleReason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdReschedule");

                    b.HasIndex("IdSchedule");

                    b.ToTable("Reschedules");
                });

            modelBuilder.Entity("API.Models.Schedule", b =>
                {
                    b.Property<int>("IdSchedule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSchedule"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("BookedtAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdCompany")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RescheduledtAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Staus")
                        .HasColumnType("int");

                    b.HasKey("IdSchedule");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCompany");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("API.Models.ScheduleServices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdSchedule")
                        .HasColumnType("int");

                    b.Property<int>("IdService")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdSchedule");

                    b.HasIndex("IdService");

                    b.ToTable("ScheduleServices");
                });

            modelBuilder.Entity("API.Models.Service", b =>
                {
                    b.Property<int>("IdService")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdService"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("IdEmployee")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("IdService");

                    b.HasIndex("IdEmployee");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("API.Models.Token", b =>
                {
                    b.Property<int>("IdToken")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdToken"));

                    b.Property<DateTime>("Expires_In")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<string>("Refresh_Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Used")
                        .HasColumnType("bit");

                    b.HasKey("IdToken");

                    b.HasIndex("IdClient")
                        .IsUnique();

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("API.Models.Company", b =>
                {
                    b.HasOne("API.Models.Client", "Owner")
                        .WithOne("OwnedCompany")
                        .HasForeignKey("API.Models.Company", "IdOwner")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("API.Models.CompanyClients", b =>
                {
                    b.HasOne("API.Models.Client", "Client")
                        .WithMany("CompanyClients")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Models.Company", "Company")
                        .WithMany("CompanyClients")
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("API.Models.Reschedule", b =>
                {
                    b.HasOne("API.Models.Schedule", "Schedule")
                        .WithMany("Reschedules")
                        .HasForeignKey("IdSchedule")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("API.Models.Schedule", b =>
                {
                    b.HasOne("API.Models.Client", "Client")
                        .WithMany("Schedules")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Models.Company", "Company")
                        .WithMany("Schedules")
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("API.Models.ScheduleServices", b =>
                {
                    b.HasOne("API.Models.Schedule", "Schedule")
                        .WithMany("ScheduleServices")
                        .HasForeignKey("IdSchedule")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Models.Service", "Service")
                        .WithMany("ScheduleServices")
                        .HasForeignKey("IdService")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("API.Models.Service", b =>
                {
                    b.HasOne("API.Models.Client", "Employee")
                        .WithMany("ServicesProvides")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("API.Models.Token", b =>
                {
                    b.HasOne("API.Models.Client", "Client")
                        .WithOne("Token")
                        .HasForeignKey("API.Models.Token", "IdClient")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("API.Models.Client", b =>
                {
                    b.Navigation("CompanyClients");

                    b.Navigation("OwnedCompany");

                    b.Navigation("Schedules");

                    b.Navigation("ServicesProvides");

                    b.Navigation("Token")
                        .IsRequired();
                });

            modelBuilder.Entity("API.Models.Company", b =>
                {
                    b.Navigation("CompanyClients");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("API.Models.Schedule", b =>
                {
                    b.Navigation("Reschedules");

                    b.Navigation("ScheduleServices");
                });

            modelBuilder.Entity("API.Models.Service", b =>
                {
                    b.Navigation("ScheduleServices");
                });
#pragma warning restore 612, 618
        }
    }
}
