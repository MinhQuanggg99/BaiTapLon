﻿using BaiTapLon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Data
{
    public class BTLContext : IdentityDbContext<AppUser>
    {
        public BTLContext(DbContextOptions<BTLContext> options)
            : base(options)
        {

        }
        public DbSet<Books> Books { get; set; }
        public DbSet<AppRole> AppRole { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BookType>()
                    .HasMany(e => e.Books)
                    .WithOne(e => e.BookType)
                    .HasForeignKey(e => e.BookTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<BookType> BookType { get; set; } = default!;
        public DbSet<BookAction> BookAction { get; set; } = default!;


    }
}
