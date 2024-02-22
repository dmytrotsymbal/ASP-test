﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using test.Data;

#nullable disable

namespace test.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240213091223_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("test.Models.AccessEvent", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventTimestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("EventId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RoomId");

                    b.ToTable("AccessEvents");
                });

            modelBuilder.Entity("test.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("test.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("test.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RoomDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("test.Models.AccessEvent", b =>
                {
                    b.HasOne("test.Models.Employee", "Employee")
                        .WithMany("AccessEvents")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("test.Models.Room", "Room")
                        .WithMany("AccessEvents")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("test.Models.Employee", b =>
                {
                    b.HasOne("test.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("test.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("test.Models.Employee", b =>
                {
                    b.Navigation("AccessEvents");
                });

            modelBuilder.Entity("test.Models.Room", b =>
                {
                    b.Navigation("AccessEvents");
                });
#pragma warning restore 612, 618
        }
    }
}