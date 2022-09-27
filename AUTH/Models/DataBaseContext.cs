﻿using Microsoft.EntityFrameworkCore;


namespace AUTH_Service.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Employee> Employees { get; set; }

    }
}