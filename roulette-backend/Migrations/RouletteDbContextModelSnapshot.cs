﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Roulette.App.Model.Database;

#nullable disable

namespace Roulette.App.Migrations
{
    [DbContext(typeof(RouletteDbContext))]
    partial class RouletteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Roulette.App.Model.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("OutcomeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OutcomeId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Roulette.App.Model.PlayerAccount", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.HasKey("Username");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Roulette.App.Model.Result", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Number")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("GameResults");
                });

            modelBuilder.Entity("Roulette.App.Model.Wager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BetResultId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<bool?>("IncludedInBalance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("False");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("Winnings")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BetResultId");

                    b.HasIndex("GameId");

                    b.HasIndex("Username");

                    b.ToTable("Wagers");
                });

            modelBuilder.Entity("Roulette.App.Model.Game", b =>
                {
                    b.HasOne("Roulette.App.Model.Result", "Outcome")
                        .WithMany()
                        .HasForeignKey("OutcomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Outcome");
                });

            modelBuilder.Entity("Roulette.App.Model.Wager", b =>
                {
                    b.HasOne("Roulette.App.Model.Result", "BetResult")
                        .WithMany()
                        .HasForeignKey("BetResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Roulette.App.Model.Game", "Game")
                        .WithMany("Wagers")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Roulette.App.Model.PlayerAccount", "PlayerAccount")
                        .WithMany("Wagers")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BetResult");

                    b.Navigation("Game");

                    b.Navigation("PlayerAccount");
                });

            modelBuilder.Entity("Roulette.App.Model.Game", b =>
                {
                    b.Navigation("Wagers");
                });

            modelBuilder.Entity("Roulette.App.Model.PlayerAccount", b =>
                {
                    b.Navigation("Wagers");
                });
#pragma warning restore 612, 618
        }
    }
}
