using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TwitterSentimentsCore.Models;

namespace TwitterSentimentsCore.Migrations
{
    [DbContext(typeof(RequestDbContext))]
    [Migration("20160818165423_RequestsMigration")]
    partial class RequestsMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TwitterSentimentsCore.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<double>("Result");

                    b.Property<string>("TwitterHandle");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });
        }
    }
}
