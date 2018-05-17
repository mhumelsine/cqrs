﻿// <auto-generated />
using Isf.Core.Cqrs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Isf.Core.Migrations
{
    [DbContext(typeof(EventDbContext))]
    [Migration("20180517012153_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Isf.Core.Cqrs.DomainEvent", b =>
                {
                    b.Property<long>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AggregateRootId");

                    b.Property<string>("EventData");

                    b.Property<string>("EventName");

                    b.Property<int>("EventSequence");

                    b.Property<DateTime>("EventTimestamp");

                    b.Property<string>("UserCreated");

                    b.HasKey("EventId");

                    b.ToTable("DomainEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
