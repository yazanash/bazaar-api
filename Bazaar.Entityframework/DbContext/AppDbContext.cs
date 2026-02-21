using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.UserWallet;
using Bazaar.Entityframework.Models.Vehicles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Bazaar.Entityframework.DbContext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<VehicleAd> VehicleAds { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<VehicleModel> vehicleModels { get; set; }
        public DbSet<CarSpecs> CarSpecs { get; set; }
        public DbSet<MotorSpecs> MotorSpecs { get; set; }
        public DbSet<TruckSpecs> TruckSpecs { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<OTPModel> OTPModels { get; set; }
        public DbSet<AdBanners> AdBanners { get; set; }
        public DbSet<UserWallet> UserWallets{ get; set; }
        public DbSet<PackageBundle> PackageBundles { get; set; }
        public DbSet<CreditTransaction> CreditTransactions{ get; set; }
        public DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VehicleAd>()
                    .HasOne(v => v.CarSpecs)
                    .WithOne(s => s.VehicleAd)
                    .HasForeignKey<CarSpecs>(s => s.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<VehicleAd>()
                    .HasOne(v => v.MotorSpecs)
                    .WithOne(s => s.VehicleAd)
                    .HasForeignKey<MotorSpecs>(s => s.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<VehicleAd>()
                    .HasOne(v => v.TruckSpecs)
                    .WithOne(s => s.VehicleAd)
                    .HasForeignKey<TruckSpecs>(s => s.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VehicleModel>()
                    .HasOne(m => m.Manufacturer)
                    .WithMany()
                    .HasForeignKey(m => m.ManufacturerId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<VehicleAd>()
                   .HasOne(v => v.VehicleModel)
                   .WithMany()
                   .HasForeignKey(v => v.VehicleModelId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserFavorite>()
                   .HasKey(f => new { f.UserId, f.VehicleAdId });
            builder.Entity<VehicleImage>()
                   .HasOne<VehicleAd>()
                    .WithMany(v => v.VehicleImages)
                    .HasForeignKey(i => i.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<VehicleAd>()
                    .HasOne(v => v.User)
                    .WithMany()
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<AppUser>()
                    .HasOne(u => u.Profile)
                    .WithOne()
                    .HasForeignKey<Profile>(p => p.UserId);

            builder.Entity<Manufacturer>(entity =>
            {
                entity.Property(e => e.SupportedCategories)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v ?? new List<Category>(), (JsonSerializerOptions?)null),

                        v => JsonSerializer.Deserialize<List<Category>>(v, (JsonSerializerOptions?)null) ?? new List<Category>()
                    )
                    .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<Category>>(
                        (c1, c2) => c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()
                    ));
            });
        }
    }
}
