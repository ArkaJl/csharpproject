
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper)
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
            await _repositoryWrapper.User.Create(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Update(User model)
        {
            await _repositoryWrapper.User.Update(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Delete(string id)
        {
            var user = await _repositoryWrapper.User
                .FindByCondition(x => x.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                await _repositoryWrapper.User.Delete(user);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}