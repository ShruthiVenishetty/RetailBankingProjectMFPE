using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServiceRepository.Models
{

    public partial class AccountDbContext : DbContext
    {
        public AccountDbContext()
        {

        }
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountType> AccountType { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; database=AccountDB; integrated security=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountType>().HasData(
                    new AccountType
                    {
                        AccountTypeCode = 200,
                        AccountTypeName = "Savings"
                    },
                    new AccountType
                    {
                        AccountTypeCode = 201,
                        AccountTypeName = "Current"
                    }
                );
        }

       
    }

}