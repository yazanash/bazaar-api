using Azure;
using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Filters;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.Vehicles;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class AdDataService(AppDbContext appDbContext) : IAdDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<bool> ChangeAddStatus(int vehicleAdId, PubStatus pubStatus, string? reason)
        {
            var rowsAffected = await _appDbContext.VehicleAds
          .Where(x => x.Id == vehicleAdId)
          .ExecuteUpdateAsync(setters => setters
              .SetProperty(v => v.PublishStatus, pubStatus)
              .SetProperty(v => v.RejectionReason, reason)
              .SetProperty(v => v.PublishedAt, pubStatus == PubStatus.Accepted ? DateTime.UtcNow : (DateTime?)null));

            if (rowsAffected == 0) throw new ResourceNotFoundException($"Ad with ID {vehicleAdId} was not found.");
            return true;
        }

        public async Task<PagedList<VehicleAd>> GetAllStarredAsync(int page, int size, string? currentUserId)
        {
            var query = _appDbContext.Set<VehicleAd>()
                            .Where(x=>x.Special)
                        .OrderByDescending(f => f.PublishedAt)
                        .Include(ad => ad.VehicleModel).ThenInclude(x=>x!.Manufacturer)
                        .Include(ad => ad.City)
                        .Include(ad => ad.UserFavorites.Where(f => f.UserId == currentUserId))
                        .Include(ad=>ad.VehicleImages.FirstOrDefault())
                        .AsNoTracking();
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            return new PagedList<VehicleAd>(items, count, page, size);
        }

        public async Task<PagedList<VehicleAd>> GetByPostStatusAsync(int page, int size, string? currentUserId, PubStatus pubStatus)
        {
            var query = _appDbContext.Set<VehicleAd>()
                            .Where(x => x.PublishStatus == pubStatus)
                        .OrderBy(f => f.PublishedAt)
                        .Include(ad => ad.VehicleModel).ThenInclude(x => x!.Manufacturer)
                        .Include(ad => ad.City)
                        .Include(ad => ad.UserFavorites.Where(f => f.UserId == currentUserId))
                        .Include(ad => ad.VehicleImages.FirstOrDefault())
                        .AsNoTracking();
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            return new PagedList<VehicleAd>(items, count, page, size);
        }

        public async Task<VehicleAd> GetBySlugAsync(string slug, string? currentUserId)
        {
            VehicleAd? ad = await _appDbContext.Set<VehicleAd>()
                .Include(ad => ad.VehicleModel).ThenInclude(x => x!.Manufacturer)
                .Include(x => x.City)
                .Include(x => x.TruckSpecs)
                .Include(x => x.CarSpecs)
                .Include(x => x.MotorSpecs)
                .Include(x => x.User).ThenInclude(x => x!.Profile)
                .Include(x => x.VehicleImages)
                .Include(x => x.UserFavorites).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == slug);
            if (ad == null) throw new ResourceNotFoundException("ad was not found.");
            return ad;
        }

        public async Task<PagedList<VehicleAd>> GetUserFavoritesAsync(int page, int size, string currentUserId)
        {
            var query = _appDbContext.Set<VehicleAd>()
            .AsNoTracking()
            .Where(ad => ad.UserFavorites.Any(f => f.UserId == currentUserId))

            .Include(ad => ad.VehicleModel)
                .ThenInclude(m => m!.Manufacturer)
            .Include(ad => ad.City)
            .Include(ad => ad.User)
                .ThenInclude(u => u!.Profile)
            .Include(ad => ad.UserFavorites.Where(f => f.UserId == currentUserId))
            .Include(ad => ad.VehicleImages.FirstOrDefault())
            .OrderByDescending(ad => ad.PublishedAt);

            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return new PagedList<VehicleAd>(items, count, page, size);

        }

        public async Task<PagedList<VehicleAd>> SearchAsync(GeneralFilter generalFilter, ISpecsFilter? specsFilter, int page, int size, string? currentUserId)
        {
            var adPredicate = PredicateBuilder.New<VehicleAd>(true);
            adPredicate = adPredicate.And(v => v.PublishStatus == PubStatus.Accepted);

            if (!string.IsNullOrEmpty( generalFilter.Keyword)) adPredicate = adPredicate.And(v => v.Description.Contains(generalFilter.Keyword));
            if (generalFilter.CityId.HasValue) adPredicate = adPredicate.And(v => v.CityId == generalFilter.CityId);
            if (generalFilter.VehicleModelId.HasValue) adPredicate = adPredicate.And(v => v.VehicleModelId == generalFilter.VehicleModelId);
            if (generalFilter.IsUsed.HasValue) adPredicate = adPredicate.And(v => v.IsUsed == generalFilter.IsUsed);
            if (generalFilter.FuelType.HasValue) adPredicate = adPredicate.And(v => v.FuelType == generalFilter.FuelType);
            if (generalFilter.Installment.HasValue) adPredicate = adPredicate.And(v => v.Installment == generalFilter.Installment);
            if (generalFilter.PriceFrom.HasValue) adPredicate = adPredicate.And(v => v.Price >= generalFilter.PriceFrom);
            if (generalFilter.PriceTo.HasValue) adPredicate = adPredicate.And(v => v.Price <= generalFilter.PriceTo);
            if (generalFilter.Category.HasValue) adPredicate = adPredicate.And(v => v.Category == generalFilter.Category);
            if (generalFilter.PostDate == PostDateFilter.Past24) adPredicate = adPredicate.And(v => v.PublishedAt >= DateTime.UtcNow.AddHours(-24));
            if (generalFilter.PostDate == PostDateFilter.PastWeek) adPredicate = adPredicate.And(v => v.PublishedAt >= DateTime.UtcNow.AddDays(-7));
            if (generalFilter.PostDate == PostDateFilter.PastMonth) adPredicate = adPredicate.And(v => v.PublishedAt >= DateTime.UtcNow.AddMonths(-1));

            if (specsFilter != null)
            {
                if (specsFilter is CarSpecsFilter carF)
                {
                    adPredicate = adPredicate.And(v => v.CarSpecs != null);
                    if (carF.UsageType.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.UsageType == carF.UsageType);
                    if (carF.Transmission.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.Transmission == carF.Transmission);
                    if (carF.CarBodyType.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.CarBodyType == carF.CarBodyType);
                    if (carF.DriveSystem.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.DriveSystem == carF.DriveSystem);
                    if (carF.IsModified.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.IsModified == carF.IsModified);
                    if (carF.DoorsCount.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.DoorsCount == carF.DoorsCount);
                    if (carF.SeatsCount.HasValue) adPredicate = adPredicate.And(v => v.CarSpecs!.SeatsCount == carF.SeatsCount);
                }
                else if (specsFilter is TruckSpecsFilter truckF)
                {
                    adPredicate = adPredicate.And(v => v.TruckSpecs != null);
                    if (truckF.TruckBodyType.HasValue) adPredicate = adPredicate.And(v => v.TruckSpecs!.TruckBodyType == truckF.TruckBodyType);
                    if (truckF.PayloadFrom.HasValue) adPredicate = adPredicate.And(v => v.TruckSpecs!.Payload >= truckF.PayloadFrom);
                    if (truckF.PayloadTo.HasValue) adPredicate = adPredicate.And(v => v.TruckSpecs!.Payload <= truckF.PayloadTo);
                    if (truckF.TrucksUsageType.HasValue) adPredicate = adPredicate.And(v => v.TruckSpecs!.TrucksUsageType == truckF.TrucksUsageType);
                }
                else if (specsFilter is MotorSpecsFilter motorF)
                {
                    adPredicate = adPredicate.And(v => v.MotorSpecs != null);
                    if (motorF.MotorTransmission.HasValue) adPredicate = adPredicate.And(v => v.MotorSpecs!.MotorTransmission == motorF.MotorTransmission);
                    if (motorF.MotorBodyType.HasValue) adPredicate = adPredicate.And(v => v.MotorSpecs!.MotorBodyType == motorF.MotorBodyType);
                    if (motorF.IsModified.HasValue) adPredicate = adPredicate.And(v => v.MotorSpecs!.IsModified == motorF.IsModified);
                }
            }

            var query = _appDbContext.VehicleAds
                .AsExpandable()
                .Where(adPredicate)
                .OrderByDescending(v => v.PublishedAt)
                .Include(ad => ad.VehicleModel).ThenInclude(x => x!.Manufacturer)
                .Include(v => v.City)
                .Include(v => v.UserFavorites.Where(f => f.UserId == currentUserId))
                .Include(ad => ad.VehicleImages.FirstOrDefault())
                .AsNoTracking();

            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return new PagedList<VehicleAd>(items, count, page, size);
        }

        public async Task<bool> ToggleVehicleFavoriteAsync(int id, string userId)
        {
            var favorite = await _appDbContext.UserFavorites
                           .FirstOrDefaultAsync(f => f.UserId == userId && f.VehicleAdId == id);

            if (favorite != null)
            {
                _appDbContext.UserFavorites.Remove(favorite);
                await _appDbContext.VehicleAds
                    .Where(v => v.Id == id)
                    .ExecuteUpdateAsync(v => v.SetProperty(p => p.FavoritesCount, p => p.FavoritesCount - 1));

                await _appDbContext.SaveChangesAsync();
                return false;
            }
            else
            {
                await _appDbContext.UserFavorites.AddAsync(new UserFavorite
                {
                    UserId = userId,
                    VehicleAdId = id,
                    CreatedAt = DateTime.UtcNow
                });

                await _appDbContext.VehicleAds
                    .Where(v => v.Id == id)
                    .ExecuteUpdateAsync(v => v.SetProperty(p => p.FavoritesCount, p => p.FavoritesCount + 1));

                await _appDbContext.SaveChangesAsync();
                return true; 
            }
        }
    }
}
