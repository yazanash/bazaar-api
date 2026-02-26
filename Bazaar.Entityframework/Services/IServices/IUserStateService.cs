using Bazaar.Entityframework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services.IServices
{
    public interface IUserStateService
    {
        Task<TelegramUserState> GetOrCreateAsync(long chatId);
        Task UpdateStateAsync(TelegramUserState state);
        Task ResetStateAsync(long chatId);
    }
}
