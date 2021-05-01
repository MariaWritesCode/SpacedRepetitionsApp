using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepApp.Infrastructure
{
    public class SpacedRepAppDbContext : IdentityDbContext<IdentityUser>
    {
        public SpacedRepAppDbContext(DbContextOptions<SpacedRepAppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasOne(category => category.User);
            modelBuilder.Entity<Note>().HasOne(note => note.User);
            modelBuilder.Entity<Tag>().HasOne(tag => tag.User);

            base.OnModelCreating(modelBuilder);
        }
    }
}
