using IDP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Infra.Data
{
    public class ShopQueryDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ShopQueryDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("QueryDbConnection"));
        }

        public DbSet<User> Tbl_Users { get; set; }
    }
}
