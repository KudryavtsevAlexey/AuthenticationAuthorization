using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationAuthentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AuthorizationAuthentication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<ApplicationUser> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "UserName",
                    Password = "Password",
                    FirstName = "FirstName",
                    LastName = "LastName"
                });
        }

    }
}
