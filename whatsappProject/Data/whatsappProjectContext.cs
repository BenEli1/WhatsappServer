#nullable disable
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

        public DbSet<whatsappProject.Models.FeedBack> Grading { get; set; }

        public DbSet<whatsappProject.Models.User>? User { get; set; }

        public DbSet<whatsappProject.Models.ChatList>? ChatList { get; set; }
    }
}
