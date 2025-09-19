using DataAccess.Models;


namespace BusinessLogic.Interfaces
{
    public interface IUserInventoryService
    {
        Task<List<UserInventory>> GetAll();
        Task<UserInventory?> GetById(string id);
        Task Create(UserInventory model);
        Task Update(UserInventory model);
        Task Delete(string id);
    }
}