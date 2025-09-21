
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserInventoryService : IUserInventoryService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserInventoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<UserInventory>> GetAll()
        {
            return await _repositoryWrapper.UserInventory.FindAll()
                .Include(ui => ui.Item)
                .Include(ui => ui.User)
                .ToListAsync();
        }

        public async Task<List<UserInventory>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.UserInventory
                .FindByCondition(x => x.UserId.ToString() == userId)
                .Include(ui => ui.Item)
                .ToListAsync();
        }

        public async Task<UserInventory?> GetByUserAndItemId(string userId, string itemId)
        {
            return await _repositoryWrapper.UserInventory
                .FindByCondition(x => x.UserId.ToString() == userId && x.ItemId.ToString() == itemId)
                .Include(ui => ui.Item)
                .FirstOrDefaultAsync();
        }

        public async Task Create(UserInventory model)
        {
            await _repositoryWrapper.UserInventory.Create(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Update(UserInventory model)
        {
            await _repositoryWrapper.UserInventory.Update(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Delete(string userId, string itemId)
        {
            var userInventory = await _repositoryWrapper.UserInventory
                .FindByCondition(x => x.UserId.ToString() == userId && x.ItemId.ToString() == itemId)
                .FirstOrDefaultAsync();

            if (userInventory != null)
            {
                await _repositoryWrapper.UserInventory.Delete(userInventory);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task EquipItem(string userId, string itemId)
        {
            var userInventory = await _repositoryWrapper.UserInventory
                .FindByCondition(x => x.UserId.ToString() == userId && x.ItemId.ToString() == itemId)
                .FirstOrDefaultAsync();

            if (userInventory != null)
            {
                userInventory.IsEquipped = true;
                await _repositoryWrapper.UserInventory.Update(userInventory);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task UnequipItem(string userId, string itemId)
        {
            var userInventory = await _repositoryWrapper.UserInventory
                .FindByCondition(x => x.UserId.ToString() == userId && x.ItemId.ToString() == itemId)
                .FirstOrDefaultAsync();

            if (userInventory != null)
            {
                userInventory.IsEquipped = false;
                await _repositoryWrapper.UserInventory.Update(userInventory);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}