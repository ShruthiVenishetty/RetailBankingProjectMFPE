using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationRepository.Models
{
    public class LoginDBContext : DbContext
    {
        public LoginDBContext()
        {

        }
        public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"data source=(localdb)\MSSQLLocalDB; database=LoginDB; integrated security=true");
            }
        }

        public DbSet<Login> Logins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }




    }

}
