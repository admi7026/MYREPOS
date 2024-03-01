﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ordering.API.Data;

#nullable disable

namespace Ordering.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240301073837_InitDatabase")]
    partial class InitDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ordering.API.Data.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int>("Id"), 10L, null, null, null, null, null);

                    b.Property<string>("Note")
                        .HasColumnType("text")
                        .HasColumnName("note");

                    b.Property<DateTimeOffset>("OrderDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("order_date");

                    b.Property<int>("StateId")
                        .HasColumnType("integer")
                        .HasColumnName("state_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("StateId")
                        .HasDatabaseName("ix_orders_state_id");

                    b.ToTable("orders", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Note = "Test order",
                            OrderDate = new DateTimeOffset(new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)),
                            StateId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("Ordering.API.Data.Entities.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int>("Id"), 10L, null, null, null, null, null);

                    b.Property<int>("OrderId")
                        .HasColumnType("integer")
                        .HasColumnName("order_id");

                    b.Property<int>("Price")
                        .HasColumnType("integer")
                        .HasColumnName("price");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<string>("ProductName")
                        .HasColumnType("text")
                        .HasColumnName("product_name");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("Id")
                        .HasName("pk_order_details");

                    b.HasIndex("OrderId", "ProductId")
                        .IsUnique()
                        .HasDatabaseName("ix_order_details_order_id_product_id");

                    b.ToTable("order_details", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrderId = 1,
                            Price = 1000,
                            ProductId = 1,
                            ProductName = "iPhone 15 Promax",
                            Quantity = 1
                        },
                        new
                        {
                            Id = 2,
                            OrderId = 1,
                            Price = 500,
                            ProductId = 8,
                            ProductName = "Galaxy Watch",
                            Quantity = 2
                        });
                });

            modelBuilder.Entity("Ordering.API.Data.Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int>("Id"), 10L, null, null, null, null, null);

                    b.Property<string>("StateName")
                        .HasColumnType("text")
                        .HasColumnName("state_name");

                    b.HasKey("Id")
                        .HasName("pk_states");

                    b.ToTable("states", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StateName = "Processing"
                        },
                        new
                        {
                            Id = 2,
                            StateName = "Success"
                        },
                        new
                        {
                            Id = 3,
                            StateName = "Fail"
                        });
                });

            modelBuilder.Entity("Ordering.API.Data.Entities.Order", b =>
                {
                    b.HasOne("Ordering.API.Data.Entities.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_states_state_id");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Ordering.API.Data.Entities.OrderDetail", b =>
                {
                    b.HasOne("Ordering.API.Data.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_details_orders_order_id");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ordering.API.Data.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
