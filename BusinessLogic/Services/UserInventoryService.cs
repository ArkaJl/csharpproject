using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserInventoryService : IUserService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserInventoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<User>> GetAll()
        {
            return await _repositoryWrapper.User.FindAll().ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            return await _repositoryWrapper.User
                .FindByCondition(x => x.Id.ToString() == id)
                .FirstOrDefaultAsync();
        }


        public async Task Create(User model)
        {
            _repositoryWrapper.User.Create(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Update(User model)
        {
            _repositoryWrapper.User.Update(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Delete(string id) // Измените на string
        {
            var user = await _repositoryWrapper.User
                .FindByCondition(x => x.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                _repositoryWrapper.User.Delete(user);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}