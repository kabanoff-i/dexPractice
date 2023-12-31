﻿using Microsoft.EntityFrameworkCore;

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
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public BankServiceContext(DbContextOptions<BankServiceContext> options): base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Port = 5432; Database = bankService; Username = postgres; Password = sewdaw");
        }
    }
}
