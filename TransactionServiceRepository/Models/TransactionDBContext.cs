using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    public class TransactionDBContext : DbContext
    {
        public TransactionDBContext()
        {

        }
        public TransactionDBContext(DbContextOptions<TransactionDBContext> options) : base(options)
        {

        }
        public virtual DbSet<FinancialTransaction> financialTransactions { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; database=TransactionDB; integrated security=true");
            }
        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentMethod>().HasData(
                    new PaymentMethod
                    {
                        PaymentMethodCode = 300,
                        PaymentMethodName = "Amex"
                    },
                    new PaymentMethod
                    {
                        PaymentMethodCode = 301,
                        PaymentMethodName = "Bank Transfer"
                    },
                    new PaymentMethod
                    {
                        PaymentMethodCode = 302,
                        PaymentMethodName = "Cash"
                    },
                    new PaymentMethod
                    {
                        PaymentMethodCode = 303,
                        PaymentMethodName = "Diners Club"
                    },
                    new PaymentMethod
                    {
                        PaymentMethodCode = 304,
                        PaymentMethodName = "Master Card"
                    },
                    new PaymentMethod
                    {
                        PaymentMethodCode = 305,
                        PaymentMethodName = "Visa"
                    }
                );
            modelBuilder.Entity<TransactionType>().HasData(
                    new TransactionType
                    {
                        TransactionTypeCode = 4000,
                        TransactionTypeDescription = "Adjustment"
                    },
                    new TransactionType
                    {
                        TransactionTypeCode = 4001,
                        TransactionTypeDescription = "Payment"
                    },
                    new TransactionType
                    {
                        TransactionTypeCode = 4002,
                        TransactionTypeDescription = "Refund"
                    },
                    new TransactionType
                    {
                        TransactionTypeCode = 4003,
                        TransactionTypeDescription = "Self"
                    }
                );
        }


       
    }

}
    

