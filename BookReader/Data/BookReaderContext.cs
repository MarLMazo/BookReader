using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BookReader.Models;

namespace BookReader.Data
{
    public class BookReaderContext : DbContext
    {
        public BookReaderContext() : base("name=BookReaderContext")
        {

        }

        public DbSet<Reader> Readers { get; set; }
        public DbSet<Book> Books { get; set; }
 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(p => p.Readers)
                .WithMany(b => b.Books)
                .Map(cs =>
                {
                    cs.MapLeftKey("BookId");
                    cs.MapRightKey("ReaderId");
                    cs.ToTable("BookReaders");
                });
        }
    }
}