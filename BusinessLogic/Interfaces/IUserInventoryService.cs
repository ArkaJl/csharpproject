using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IUserInventoryService
    {
        Task<List<UserInventory>> GetAll();
        Task<List<UserInventory>> GetByUserId(string userId);
        Task<UserInventory?> GetByUserAndItemId(string userId, string itemId);
        Task Create(UserInventory model);
        Task Update(UserInventory model);
        Task Delete(string userId, string itemId);
        Task EquipItem(string userId, string itemId);
        Task UnequipItem(string userId, string itemId);
    }
}