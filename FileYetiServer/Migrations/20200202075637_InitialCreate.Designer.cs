﻿// <auto-generated />
using System;
using FileYetiServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FileYetiServer.Migrations
{
    [DbContext(typeof(FileYetiServerDbContext))]
    [Migration("20200202075637_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("FileYetiServer.Models.TransferJob", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChunkSizeBytes");

                    b.Property<Guid>("JobGuid");

                    b.Property<DateTime>("LastChunkRecieved");

                    b.Property<int>("Status");

                    b.Property<int>("TotalChunks");

                    b.Property<int>("TotalChunksReceived");

                    b.HasKey("JobId");

                    b.ToTable("TransferJobs");
                });
#pragma warning restore 612, 618
        }
    }
}
