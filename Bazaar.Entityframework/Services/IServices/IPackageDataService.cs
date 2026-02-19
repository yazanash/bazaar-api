using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public  interface IPackageDataService
    {
        Task<Package> CreateAsync(Package entity);
        Task<Package> UpdateAsync(Package entity);
        Task<IEnumerable<Package>> GetAllAsync();
        Task<Package> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
    }
}
