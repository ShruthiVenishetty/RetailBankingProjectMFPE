using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerServiceRepository.Models
{
    public class CustomerDBContext : DbContext
    {
        public CustomerDBContext()
        {

        }
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"data source=(localdb)\MSSQLLocalDB; database=CustomerDB; integrated security=true");
            }
        }

        public DbSet<Customer> Customers { get; set; }
      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PAN_Number)
                .IsUnique();
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }

}
