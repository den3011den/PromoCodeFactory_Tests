using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;

namespace PromoCodeFactory.DataAccess
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class DataContext
        : DbContext
    {
        /// <summary>
        /// Промокоды
        /// </summary>
        public DbSet<PromoCode> PromoCodes { get; set; }

        /// <summary>
        /// Клиенты
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Предпочтения
        /// </summary>
        public DbSet<Preference> Preferences { get; set; }

        /// <summary>
        /// Роли
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Сотрудники
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Партнёры
        /// </summary>
        public DbSet<Partner> Partners { get; set; }

        /// <summary>
        /// Лимиты промокодов
        /// </summary>
        public DbSet<PartnerPromoCodeLimit> PartnerPromoCodeLimit { get; set; }

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ограничения
            modelBuilder.Entity<CustomerPreference>()
                .HasKey(bc => new { bc.CustomerId, bc.PreferenceId });
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(bc => bc.Customer)
                .WithMany(b => b.Preferences)
                .HasForeignKey(bc => bc.CustomerId);
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(bc => bc.Preference)
                .WithMany()
                .HasForeignKey(bc => bc.PreferenceId);

            modelBuilder.Entity<PartnerPromoCodeLimit>()
                .HasOne(pr => pr.Partner);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}