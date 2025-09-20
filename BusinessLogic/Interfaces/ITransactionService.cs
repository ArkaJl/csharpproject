using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAll();
        Task<Transaction?> GetById(string id);
        Task<List<Transaction>> GetByUserId(string userId);
        Task<List<Transaction>> GetByItemId(string itemId);
        Task<List<Transaction>> GetByType(string type);
        Task<Transaction> Create(Transaction transaction);
        Task<Transaction> Update(Transaction transaction);
        Task Delete(string id);
        Task<List<Transaction>> GetByDateRange(DateTime startDate, DateTime endDate);
        Task<int> GetUserBalance(string userId);
    }
}