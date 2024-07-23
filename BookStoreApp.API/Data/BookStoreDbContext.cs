using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApplicationUser>
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC07BB19DAC6");

            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC0791068A9C");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA521AEB15").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Isbn)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Books_ToTable");
        });

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole 
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "8b80bb4a-bceb-4e48-b020-0b42da346212"
            },
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = "57f7dbef-2cf8-49c9-956a-3a9e16d6a0a5"
            }
        );

        var hasher = new PasswordHasher<ApplicationUser>();

        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "9f86d912-6254-44e6-aa64-d4da31c8a999",
                Email = "admin@test.com",
                NormalizedEmail = "ADMIN@TEST.COM",
                UserName = "admin@test.com",
                NormalizedUserName = "ADMIN@TEST.COM",
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = hasher.HashPassword(null, "Jabberwocky1!")
            },
            new ApplicationUser
            {
                Id = "0017d7fe-f844-47fa-96b1-f6f3f280db0f",
                Email = "user@test.com",
                NormalizedEmail = "USER@TEST.COM",
                UserName = "user@test.com",
                NormalizedUserName = "USER@TEST.COM",
                FirstName = "System",
                LastName = "User",
                PasswordHash = hasher.HashPassword(null, "Jabberwocky1!")
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "8b80bb4a-bceb-4e48-b020-0b42da346212",
                UserId = "0017d7fe-f844-47fa-96b1-f6f3f280db0f"
            },
            new IdentityUserRole<string>
            {
                RoleId = "57f7dbef-2cf8-49c9-956a-3a9e16d6a0a5",
                UserId = "9f86d912-6254-44e6-aa64-d4da31c8a999"
            }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
