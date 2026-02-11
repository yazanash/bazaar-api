using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class ProfileDataService(AppDbContext appDbContext) : IProfileDataService
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<Profile> CreateAsync(Profile entity)
        {
            EntityEntry<Profile> CreatedResult = await _appDbContext.Set<Profile>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<Profile> GetProfileByIdAsync(int id)
        {
            Profile? profile = await _appDbContext.Set<Profile>().FindAsync(id);
            if (profile == null) throw new ResourceNotFoundException(id, $"profile with ID {id} was not found.");
            return profile;
        }
        public async Task<Profile> GetUserProfileAsync(string userId)
        {
            Profile? profile = await _appDbContext.Set<Profile>().AsNoTracking().FirstOrDefaultAsync(x=>x.UserId ==userId);
            if (profile == null) throw new UserAssetNotFoundException(userId, $"profile for user with ID {userId} was not found.");
            return profile;
        }

        public async Task<Profile> UpateAsync(Profile entity)
        {
            _appDbContext.Set<Profile>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
