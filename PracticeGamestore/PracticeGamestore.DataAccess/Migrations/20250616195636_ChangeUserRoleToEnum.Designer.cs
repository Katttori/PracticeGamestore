﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PracticeGamestore.DataAccess;

#nullable disable

namespace PracticeGamestore.DataAccess.Migrations
{
    [DbContext(typeof(GamestoreDbContext))]
    [Migration("20250616195636_ChangeUserRoleToEnum")]
    partial class ChangeUserRoleToEnum
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Blacklist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("country_id");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("user_email");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.ToTable("blacklists", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<int>("CountryStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("status");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("countries", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("creation_date")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("game_id");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("path");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("Path")
                        .IsUnique();

                    b.ToTable("files", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<int>("AgeRating")
                        .HasColumnType("int")
                        .HasColumnName("age_rating");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("description");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("key");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.Property<byte[]>("Picture")
                        .HasMaxLength(1048576)
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("picture");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("price");

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("publisher_id");

                    b.Property<double>("Rating")
                        .HasColumnType("float")
                        .HasColumnName("rating");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("release_date");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("PublisherId");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.GameGenre", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("game_id");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("genre_id");

                    b.HasKey("GameId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("game_genre", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.GameOrder", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("game_id");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("order_id");

                    b.HasKey("GameId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("game_order", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.GamePlatform", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("game_id");

                    b.Property<Guid>("PlatformId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("platform_id");

                    b.HasKey("GameId", "PlatformId");

                    b.HasIndex("PlatformId");

                    b.ToTable("game_platform", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("parent_id");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("genres", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("93206b87-891b-4f3f-b268-e82feb26df84"),
                            Description = "Strategic thinking and planning games",
                            Name = "Strategy"
                        },
                        new
                        {
                            Id = new Guid("611f7b0e-9c05-47b8-89a4-c528edd600b3"),
                            Description = "Role-playing games",
                            Name = "RPG"
                        },
                        new
                        {
                            Id = new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26"),
                            Description = "Sports simulation and arcade games",
                            Name = "Sports"
                        },
                        new
                        {
                            Id = new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d"),
                            Description = "Fast-paced action games",
                            Name = "Action"
                        },
                        new
                        {
                            Id = new Guid("29c43dea-db4d-4478-8139-2aaef71eb32e"),
                            Description = "Brain teasers and skill-based games",
                            Name = "Puzzle & Skill"
                        },
                        new
                        {
                            Id = new Guid("e0b6048d-d86d-47d7-b2a3-5c614990980f"),
                            Description = "Real-time strategy",
                            Name = "RTS",
                            ParentId = new Guid("93206b87-891b-4f3f-b268-e82feb26df84")
                        },
                        new
                        {
                            Id = new Guid("3fe6ec31-f70a-404f-b884-06ac521bad66"),
                            Description = "Turn-based strategy",
                            Name = "TBS",
                            ParentId = new Guid("93206b87-891b-4f3f-b268-e82feb26df84")
                        },
                        new
                        {
                            Id = new Guid("4f2c45b1-aea8-4bf9-88a7-e9df8cdaeed8"),
                            Description = "Racing games",
                            Name = "Races",
                            ParentId = new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26")
                        },
                        new
                        {
                            Id = new Guid("0bb2e1df-8ae9-4999-8d35-639f93b74d21"),
                            Description = "Rally racing",
                            Name = "Rally",
                            ParentId = new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26")
                        },
                        new
                        {
                            Id = new Guid("cd05cba5-25e4-4a65-addd-08992f0437fc"),
                            Description = "Arcade sports",
                            Name = "Arcade",
                            ParentId = new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26")
                        },
                        new
                        {
                            Id = new Guid("e19f99a4-4b03-416a-a5ac-0619d7d1b593"),
                            Description = "Formula racing",
                            Name = "Formula",
                            ParentId = new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26")
                        },
                        new
                        {
                            Id = new Guid("7261de7f-4655-4764-afa3-6abddc1be100"),
                            Description = "Off-road racing",
                            Name = "Off-road",
                            ParentId = new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26")
                        },
                        new
                        {
                            Id = new Guid("ddc23d35-c235-4a39-ab49-fc73b1f552f7"),
                            Description = "First-person shooter",
                            Name = "FPS",
                            ParentId = new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d")
                        },
                        new
                        {
                            Id = new Guid("b2d7ee44-b010-4472-8485-cf6d84d23026"),
                            Description = "Third-person shooter",
                            Name = "TPS",
                            ParentId = new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d")
                        },
                        new
                        {
                            Id = new Guid("bfc4335b-adc3-4e11-a4ef-e572605cdaf0"),
                            Description = "Action adventure games",
                            Name = "Adventure",
                            ParentId = new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d")
                        });
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("status");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("total");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("user_email");

                    b.HasKey("Id");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Platform", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("platforms", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("7ad0515a-5e7f-4e21-a5ad-0bbcb331bab3"),
                            Description = "",
                            Name = "Android"
                        },
                        new
                        {
                            Id = new Guid("96dde723-c0d6-431f-a22e-504df437d12b"),
                            Description = "",
                            Name = "IOS"
                        },
                        new
                        {
                            Id = new Guid("75952c63-2087-4280-a3a7-c678f370686d"),
                            Description = "",
                            Name = "Windows"
                        },
                        new
                        {
                            Id = new Guid("2d8e221a-2c53-43d1-b9a6-6e18b0c4fc50"),
                            Description = "",
                            Name = "VR"
                        });
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.Property<string>("PageUrl")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("page_url");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("PageUrl")
                        .IsUnique();

                    b.ToTable("publishers", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("birth_date");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("country_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("phone_number");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("role");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("status");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Blacklist", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Country", "Country")
                        .WithMany("Blacklists")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.File", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Game", "Game")
                        .WithMany("Files")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Game", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Publisher", "Publisher")
                        .WithMany("Games")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.GameGenre", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Game", "Game")
                        .WithMany("GameGenres")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PracticeGamestore.DataAccess.Entities.Genre", "Genre")
                        .WithMany("GameGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.GameOrder", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Game", "Game")
                        .WithMany("GameOrders")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PracticeGamestore.DataAccess.Entities.Order", "Order")
                        .WithMany("GameOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.GamePlatform", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Game", "Game")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PracticeGamestore.DataAccess.Entities.Platform", "Platform")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Genre", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Genre", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.User", b =>
                {
                    b.HasOne("PracticeGamestore.DataAccess.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Country", b =>
                {
                    b.Navigation("Blacklists");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Game", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("GameGenres");

                    b.Navigation("GameOrders");

                    b.Navigation("GamePlatforms");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Genre", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("GameGenres");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Order", b =>
                {
                    b.Navigation("GameOrders");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Platform", b =>
                {
                    b.Navigation("GamePlatforms");
                });

            modelBuilder.Entity("PracticeGamestore.DataAccess.Entities.Publisher", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
