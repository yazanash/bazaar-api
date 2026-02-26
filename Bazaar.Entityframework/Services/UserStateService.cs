using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Services
{
    public class UserStateService(AppDbContext context) : IUserStateService
    {
        private readonly AppDbContext _dbContext = context;
        public async Task<TelegramUserState> GetOrCreateAsync(long chatId)
        {
            TelegramUserState? entity = await _dbContext.Set<TelegramUserState>().AsNoTracking().FirstOrDefaultAsync((e) => e.ChatId == chatId);
            if (entity == null)
            {
                entity = new TelegramUserState { ChatId = chatId };
                await _dbContext.Set<TelegramUserState>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task ResetStateAsync(long chatId)
        {
            TelegramUserState? entity = await _dbContext.Set<TelegramUserState>().AsNoTracking().FirstOrDefaultAsync((e) => e.ChatId == chatId);
            if (entity == null)
            {
                entity = new TelegramUserState { ChatId = chatId };
                await _dbContext.Set<TelegramUserState>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                await _dbContext.Set<TelegramUserState>()
          .Where(e => e.ChatId == chatId)
          .ExecuteUpdateAsync(s => s
              .SetProperty(p => p.Step, BotStep.Start));
            }
                
        }

        public async Task UpdateStateAsync(TelegramUserState state)
        {
            await _dbContext.Set<TelegramUserState>()
         .Where(e => e.Id == state.Id)
         .ExecuteUpdateAsync(s => s
             .SetProperty(p => p.Step, state.Step)
             .SetProperty(p => p.Email, state.Email)
             .SetProperty(p => p.SelectedPackageId, state.SelectedPackageId)
             .SetProperty(p => p.SelectedGatewayId, state.SelectedGatewayId)
             .SetProperty(p => p.TempReceiptFileId, state.TempReceiptFileId)
             .SetProperty(p => p.LastInteraction, DateTime.UtcNow)
         );

        }
    }
}
