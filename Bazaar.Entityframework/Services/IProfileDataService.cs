using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public interface IProfileDataService
    {
        Task<Profile> CreateAsync(Profile entity);
        Task<Profile> UpateAsync(Profile entity);
        Task<Profile> GetUserProfileAsync(string userId);
        Task<Profile> GetProfileByIdAsync(int id);
    }
}
