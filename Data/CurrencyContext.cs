using Common.Enum;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CurrencyContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions{ get; set; }
        public DbSet<Convertions> Convertions { get; set; }

        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Currencies
            // Seed data for Currency
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Code = "USD", Legend = "UnitedStatesDollar", Symbol = "$", ConvertionIndex = 1},
                new Currency { Code = "EUR", Legend = "Euro", Symbol = "€", ConvertionIndex = 1.09f },
                new Currency { Code = "CLP", Legend = "ChileanPeso", Symbol = "$", ConvertionIndex = 0.001f }
            );

            // Seed data for Subscriptions
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription { Id = 1, SubscriptionType = SubscriptionType.Free, MaxConversions = 10 },
                new Subscription { Id = 2, SubscriptionType = SubscriptionType.Trial, MaxConversions = 100 },
                new Subscription { Id = 3, SubscriptionType = SubscriptionType.Pro, MaxConversions = int.MaxValue }
            );

            // Seed data for Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "user1", Password = "password1", confirmPassword = "password1", Email = "user@gmail.com", SubscriptionId = 1, conversions = 0, Role = UserRole.User },
                new User { Id = 2, Username = "user2", Password = "password2", confirmPassword = "password2", Email = "user2@example.com", SubscriptionId = 2, conversions = 0, Role = UserRole.User },
                new User { Id = 3, Username = "user3", Password = "password3", confirmPassword = "password3", Email = "user3@example.com", SubscriptionId = 3, conversions = 0, Role = UserRole.Admin }
            );
        }
    }
}

