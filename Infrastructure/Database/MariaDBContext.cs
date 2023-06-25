using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class MariaDBContext : DbContext
    {
        public MariaDBContext(DbContextOptions<MariaDBContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccessPointModel> AccessPoints { get; set; }
        public DbSet<AccessEventModel> AccessEvents { get; set; }
    }
}
