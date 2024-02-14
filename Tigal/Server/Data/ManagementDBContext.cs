using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tigal.Shared.Models;
using Tigal.Shared.Models.Houses;

namespace Tigal.Server.Data
{
    public class ManagementDBContext : DbContext
    {
        public ManagementDBContext(DbContextOptions<ManagementDBContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; } = default!;
        public DbSet<Houses> Houses { get; set; } = default!;
        public DbSet<PropertyImages> PropertyImages { get; set; } = default!;
        public DbSet<PropertyComments> PropertyComments { get; set; } = default!;
    }
}
