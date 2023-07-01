using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ModelsDb
{
    public class BankServiceContext : DbContext
    {
        public DbSet<Client> client { get; set; }
        public DbSet<Employee> employee { get; set; }
        public DbSet<Account> account { get; set; }
        public DbSet<Currency> currency { get; set; }
        public BankServiceContext()
        {

        }
        public BankServiceContext(DbContextOptions<BankServiceContext> options): base(options) 
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Port = 5432; Database = bankServiceMigr; Username = postgres; Password = 12345678");
        }
    }
}
