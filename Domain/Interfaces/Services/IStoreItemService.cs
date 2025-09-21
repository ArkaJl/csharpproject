using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IStoreItemService
    {
        Task<List<StoreItem>> GetAll();
        Task<StoreItem?> GetById(string id);
        Task<List<StoreItem>> GetByType(string type);
        Task<List<StoreItem>> GetByCategory(string category);
        Task<StoreItem> Create(StoreItem storeItem);
        Task<StoreItem> Update(StoreItem storeItem);
        Task Delete(string id);
        Task<List<StoreItem>> Search(string searchTerm);
        Task<List<StoreItem>> GetByPriceRange(int minPrice, int maxPrice);
    }
}