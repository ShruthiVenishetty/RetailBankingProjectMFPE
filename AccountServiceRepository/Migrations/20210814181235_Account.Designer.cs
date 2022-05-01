﻿// <auto-generated />
using System;
using AccountServiceRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccountServiceRepository.Migrations
{
    [DbContext(typeof(AccountDbContext))]
    [Migration("20210814181235_Account")]
    partial class Account
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccountServiceRepository.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountTypeCode")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.HasKey("AccountId");

                    b.HasIndex("AccountTypeCode");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("AccountServiceRepository.Models.AccountType", b =>
                {
                    b.Property<int>("AccountTypeCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountTypeCode");

                    b.ToTable("AccountType");

                    b.HasData(
                        new
                        {
                            AccountTypeCode = 200,
                            AccountTypeName = "Savings"
                        },
                        new
                        {
                            AccountTypeCode = 201,
                            AccountTypeName = "Current"
                        });
                });

            modelBuilder.Entity("AccountServiceRepository.Models.Statement", b =>
                {
                    b.Property<int>("StatementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("ClosingBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ValueDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Withdrawl")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("chqNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatementId");

                    b.HasIndex("AccountId");

                    b.ToTable("Statement");
                });

            modelBuilder.Entity("AccountServiceRepository.Models.Account", b =>
                {
                    b.HasOne("AccountServiceRepository.Models.AccountType", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountType");
                });

            modelBuilder.Entity("AccountServiceRepository.Models.Statement", b =>
                {
                    b.HasOne("AccountServiceRepository.Models.Account", "Account")
                        .WithMany("Statements")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("AccountServiceRepository.Models.Account", b =>
                {
                    b.Navigation("Statements");
                });
#pragma warning restore 612, 618
        }
    }
}
