using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class StoreItemService : IStoreItemService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public StoreItemService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<StoreItem>> GetAll()
        {
            return await _repositoryWrapper.StoreItem.FindAll()
                .OrderBy(si => si.Name)
                .ToListAsync();
        }

        public async Task<StoreItem?> GetById(string id)
        {
            return await _repositoryWrapper.StoreItem
                .FindByCondition(si => si.Id.ToString() == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<StoreItem>> GetByType(string type)
        {
            return await _repositoryWrapper.StoreItem
                .FindByCondition(si => si.Type == type)
                .OrderBy(si => si.Name)
                .ToListAsync();
        }

        public async Task<List<StoreItem>> GetByCategory(string category)
        {
            return await _repositoryWrapper.StoreItem
                .FindByCondition(si => si.Category == category)
                .OrderBy(si => si.Name)
                .ToListAsync();
        }

        public async Task<StoreItem> Create(StoreItem storeItem)
        {
            _repositoryWrapper.StoreItem.Create(storeItem);
            await _repositoryWrapper.SaveAsync();
            return storeItem;
        }

        public async Task<StoreItem> Update(StoreItem storeItem)
        {
            _repositoryWrapper.StoreItem.Update(storeItem);
            await _repositoryWrapper.SaveAsync();
            return storeItem;
        }

        public async Task Delete(string id)
        {
            var storeItem = await _repositoryWrapper.StoreItem
                .FindByCondition(si => si.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (storeItem != null)
            {
                _repositoryWrapper.StoreItem.Delete(storeItem);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<List<StoreItem>> Search(string searchTerm)
        {
            return await _repositoryWrapper.StoreItem
                .FindByCondition(si => si.Name.Contains(searchTerm) ||
                                     (si.Description != null && si.Description.Contains(searchTerm)))
                .OrderBy(si => si.Name)
                .ToListAsync();
        }

        public async Task<List<StoreItem>> GetByPriceRange(int minPrice, int maxPrice)
        {
            return await _repositoryWrapper.StoreItem
                .FindByCondition(si => si.Price >= minPrice && si.Price <= maxPrice)
                .OrderBy(si => si.Price)
                .ToListAsync();
        }
    }
}