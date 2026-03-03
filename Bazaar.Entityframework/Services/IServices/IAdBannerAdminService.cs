using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IAdBannerAdminService
    {
        Task<IEnumerable<AdBanners>> GetAllForAdminAsync();
    }
}
