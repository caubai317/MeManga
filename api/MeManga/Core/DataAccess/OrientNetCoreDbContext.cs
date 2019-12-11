using Microsoft.EntityFrameworkCore;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.DataAccess
{
    public class MeMangaNetCoreDbContext : DbContext
    {
        public MeMangaNetCoreDbContext(DbContextOptions<MeMangaNetCoreDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<TypeBook> TypeBooks { get; set; }

        public DbSet<BookInType> BookInTypes { get; set; }

        public DbSet<BookInWriter> BookInWriters { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<FilePath> FilePaths { get; set; }

        public DbSet<UserCollectionBook> UserCollectionBooks { get; set; }

        public DbSet<Writer> Writers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Comment
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User).WithMany(u => u.Comments).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.Book).WithMany(u => u.Comments).IsRequired().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Comment>().HasKey(t => new { t.UserId, t.JobId });

            //modelBuilder.Entity<Comment>()
            //    .HasOne(pt => pt.User)
            //    .WithMany(p => p.Comments)
            //    .HasForeignKey(pt => pt.UserId);

            //modelBuilder.Entity<Comment>()
            //    .HasOne(pt => pt.Job)
            //    .WithMany(p => p.Comments)
            //    .HasForeignKey(pt => pt.JobId);

            // User In Role
            modelBuilder.Entity<BookInType>()
                .HasOne(u => u.Book).WithMany(u => u.BookInTypes).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookInType>().HasKey(t => new { t.BookId, t.TypeBookId });

            modelBuilder.Entity<BookInType>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.BookInTypes)
                .HasForeignKey(pt => pt.BookId);

            modelBuilder.Entity<BookInType>()
                .HasOne(pt => pt.TypeBook)
                .WithMany(p => p.BookInTypes)
                .HasForeignKey(pt => pt.TypeBookId);

            // User In Job
            modelBuilder.Entity<BookInWriter>()
                .HasOne(u => u.Book).WithMany(u => u.BookInWriters).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookInWriter>().HasKey(t => new { t.BookId, t.WriterId });

            modelBuilder.Entity<BookInWriter>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.BookInWriters)
                .HasForeignKey(pt => pt.BookId);

            modelBuilder.Entity<BookInWriter>()
                .HasOne(pt => pt.Writer)
                .WithMany(p => p.BookInWriters)
                .HasForeignKey(pt => pt.WriterId);

            // User In Job
            modelBuilder.Entity<UserCollectionBook>()
                .HasOne(u => u.User).WithMany(u => u.UserCollectionBooks).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserCollectionBook>().HasKey(t => new { t.UserId, t.BookId });

            modelBuilder.Entity<UserCollectionBook>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserCollectionBooks)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserCollectionBook>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.UserCollectionBooks)
                .HasForeignKey(pt => pt.BookId);
        }
    }
}
