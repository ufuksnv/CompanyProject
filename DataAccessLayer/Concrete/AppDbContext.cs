using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-LLU1JQO\\SQLEXPRESS;database=CompanyProjectDb;integrated security=true;");
        }

        
        public DbSet<Advice>? Advices { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<ToDo>? ToDos { get; set; }      
    }
}
