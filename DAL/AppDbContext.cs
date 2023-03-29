using LibraryAdminPanel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.DAL
{
     public class AppDbContext : IdentityDbContext<AppUser>

    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }
        public DbSet<Employee> Employers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<HistoryEmployee> HistoryEmployees { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expenditure> Expenditures { get; set; }
        public DbSet<Kassa> Kassas { get; set; }
        public DbSet<PaidedSalary> PaidedSalaries { get; set; }
        public DbSet<HasAdmins> HasAdmins { get; set; }
        public DbSet<HasAdmins> HasAdmin { get; set; }
        public DbSet<Authors> Author { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<BookAuthors> BookAuthors { get; set; }
          
        
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Kassa>().HasData(
                new Kassa { Id = 1, Balance = 0, LastModifiedTime = DateTime.UtcNow.AddHours(4), LastModified = "Hal hazırda yoxdur", LastModifiedMoney = 0, LastModifiedBy = "Hal hazırda yoxdur" }
                );
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HasAdmins>().HasData(
               new HasAdmins { Id = 1, HasAdmin = false }
               );
        }
     

    }
}
