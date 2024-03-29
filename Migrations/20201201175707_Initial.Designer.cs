﻿// <auto-generated />
using System;
using IEIPaperSearch.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IEIPaperSearch.Migrations
{
    [DbContext(typeof(PaperSearchContext))]
    [Migration("20201201175707_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("IEIPaperSearch.Models.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("JournalId")
                        .HasColumnType("int");

                    b.Property<int?>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Volume")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JournalId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Journal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Journals");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surnames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Submission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Submission");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Submission");
                });

            modelBuilder.Entity("PersonSubmission", b =>
                {
                    b.Property<int>("AuthorOfId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.HasKey("AuthorOfId", "AuthorsId");

                    b.HasIndex("AuthorsId");

                    b.ToTable("PersonSubmission");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Article", b =>
                {
                    b.HasBaseType("IEIPaperSearch.Models.Submission");

                    b.Property<string>("EndPage")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Article_EndPage");

                    b.Property<int>("PublishedInId")
                        .HasColumnType("int");

                    b.Property<string>("StartPage")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Article_StartPage");

                    b.HasIndex("PublishedInId");

                    b.HasDiscriminator().HasValue("Article");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Book", b =>
                {
                    b.HasBaseType("IEIPaperSearch.Models.Submission");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Book");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.InProceedings", b =>
                {
                    b.HasBaseType("IEIPaperSearch.Models.Submission");

                    b.Property<string>("Conference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Edition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartPage")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("InProceedings");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Issue", b =>
                {
                    b.HasOne("IEIPaperSearch.Models.Journal", "Journal")
                        .WithMany("Issues")
                        .HasForeignKey("JournalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Journal");
                });

            modelBuilder.Entity("PersonSubmission", b =>
                {
                    b.HasOne("IEIPaperSearch.Models.Submission", null)
                        .WithMany()
                        .HasForeignKey("AuthorOfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IEIPaperSearch.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Article", b =>
                {
                    b.HasOne("IEIPaperSearch.Models.Issue", "PublishedIn")
                        .WithMany("Articles")
                        .HasForeignKey("PublishedInId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PublishedIn");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Issue", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("IEIPaperSearch.Models.Journal", b =>
                {
                    b.Navigation("Issues");
                });
#pragma warning restore 612, 618
        }
    }
}
