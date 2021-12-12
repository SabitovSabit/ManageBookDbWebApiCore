using ManageBooksAPI.AuthenticateModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageBooksAPI.Models
{
    public class BookDbContext: IdentityDbContext<ApplicationUser>
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AuthorGenre> AuthorGenres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookGenre>().HasKey(bg => new { bg.BookId, bg.GenreId });

            modelBuilder.Entity<BookGenre>()
                .HasOne<Book>(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);


            modelBuilder.Entity<BookGenre>()
                .HasOne<Genre>(bg => bg.Genre)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            modelBuilder.Entity<AuthorGenre>().HasKey(ag => new { ag.AuthorId, ag.GenreId });

            modelBuilder.Entity<AuthorGenre>()
                .HasOne<Author>(ag => ag.Author)
                .WithMany(s => s.AuthorGenres)
                .HasForeignKey(ag => ag.AuthorId);


            modelBuilder.Entity<AuthorGenre>()
                .HasOne<Genre>(ag => ag.Genre)
                .WithMany(a => a.AuthorGenres)
                .HasForeignKey(ag => ag.GenreId);

            modelBuilder.Entity<AuthorBook>().HasKey(ab => new { ab.AuthorId, ab.BookId });

            modelBuilder.Entity<AuthorBook>()
                .HasOne<Author>(ab => ab.Author)
                .WithMany(a => a.AuthorBooks)
                .HasForeignKey(ab => ab.AuthorId);

            modelBuilder.Entity<AuthorBook>()
               .HasOne<Book>(ab => ab.Book)
               .WithMany(a => a.AuthorBooks)
               .HasForeignKey(ab => ab.BookId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
