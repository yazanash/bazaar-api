using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IDataService<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
    }
}
