using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TransactionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Transaction>> GetAll()
        {
            return await _repositoryWrapper.Transaction.FindAll()
                .Include(t => t.User)
                .Include(t => t.Item)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Transaction?> GetById(string id)
        {
            return await _repositoryWrapper.Transaction
                .FindByCondition(t => t.Id.ToString() == id)
                .Include(t => t.User)
                .Include(t => t.Item)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Transaction>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Transaction
                .FindByCondition(t => t.UserId.ToString() == userId)
                .Include(t => t.User)
                .Include(t => t.Item)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByItemId(string itemId)
        {
            return await _repositoryWrapper.Transaction
                .FindByCondition(t => t.ItemId.ToString() == itemId)
                .Include(t => t.User)
                .Include(t => t.Item)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByType(string type)
        {
            return await _repositoryWrapper.Transaction
                .FindByCondition(t => t.Type == type)
                .Include(t => t.User)
                .Include(t => t.Item)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            _repositoryWrapper.Transaction.Create(transaction);

            // Обновляем баланс пользователя
            if (transaction.UserId != null)
            {
                var user = await _repositoryWrapper.User
                    .FindByCondition(u => u.Id == transaction.UserId)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    if (transaction.Type == "purchase")
                    {
                        user.Coins -= transaction.Amount;
                    }
                    else if (transaction.Type == "reward" || transaction.Type == "transfer")
                    {
                        user.Coins += transaction.Amount;
                    }

                    _repositoryWrapper.User.Update(user);
                }
            }

            await _repositoryWrapper.SaveAsync();
            return transaction;
        }

        public async Task<Transaction> Update(Transaction transaction)
        {
            _repositoryWrapper.Transaction.Update(transaction);
            await _repositoryWrapper.SaveAsync();
            return transaction;
        }

        public async Task Delete(string id)
        {
            var transaction = await _repositoryWrapper.Transaction
                .FindByCondition(t => t.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (transaction != null)
            {
                // Восстанавливаем баланс пользователя при удалении транзакции
                if (transaction.UserId != null)
                {
                    var user = await _repositoryWrapper.User
                        .FindByCondition(u => u.Id == transaction.UserId)
                        .FirstOrDefaultAsync();

                    if (user != null)
                    {
                        if (transaction.Type == "purchase")
                        {
                            user.Coins += transaction.Amount;
                        }
                        else if (transaction.Type == "reward" || transaction.Type == "transfer")
                        {
                            user.Coins -= transaction.Amount;
                        }

                        _repositoryWrapper.User.Update(user);
                    }
                }

                _repositoryWrapper.Transaction.Delete(transaction);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<List<Transaction>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _repositoryWrapper.Transaction
                .FindByCondition(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .Include(t => t.User)
                .Include(t => t.Item)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUserBalance(string userId)
        {
            var transactions = await _repositoryWrapper.Transaction
                .FindByCondition(t => t.UserId.ToString() == userId)
                .ToListAsync();

            int balance = 0;
            foreach (var transaction in transactions)
            {
                if (transaction.Type == "purchase")
                {
                    balance -= transaction.Amount;
                }
                else if (transaction.Type == "reward" || transaction.Type == "transfer")
                {
                    balance += transaction.Amount;
                }
            }

            return balance;
        }
    }
}