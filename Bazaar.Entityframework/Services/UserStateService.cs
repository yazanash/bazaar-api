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
                entity.Step = BotStep.Start;
                entity.Email = null;
                entity.SelectedPackageId = null;
                entity.SelectedGatewayId = null;
                entity.TempReceiptFileId = null;
                _dbContext.Set<TelegramUserState>().Update(entity);
                await _dbContext.SaveChangesAsync();
            }
                
        }

        public async Task UpdateStateAsync(TelegramUserState state)
        {
            _dbContext.Set<TelegramUserState>().Update(state);
            await _dbContext.SaveChangesAsync();
        }
    }
}
