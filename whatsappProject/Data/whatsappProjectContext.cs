using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;

namespace whatsappProject.Data
{
    public class whatsappProjectContext : DbContext
    {
        public whatsappProjectContext (DbContextOptions<whatsappProjectContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasKey(c => new { c.id, c.UserName });
        }

        public DbSet<whatsappProject.Models.Message>? Message { get; set; }
        public DbSet<whatsappProject.Models.User>? User { get; set; }
        public DbSet<whatsappProject.Models.Contact>? Contact { get; set; }
        public DbSet<whatsappProject.Models.transfer>? Transfer { get; set; }
        public DbSet<whatsappProject.Models.Invitation>? Invitation { get; set; }
    }
}
