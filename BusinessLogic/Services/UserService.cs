
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repositoryWrapper;

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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(model.Username))
                throw new InvalidOperationException(nameof(model.Username));

            if (string.IsNullOrEmpty(model.Email))
                throw new InvalidOperationException(nameof(model.Email));

            if (string.IsNullOrEmpty(model.PasswordHash))
                throw new InvalidOperationException(nameof(model.PasswordHash));

            await _repositoryWrapper.User.Create(model);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task Update(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(model.Username))
                throw new InvalidOperationException(nameof(model.Username));

            if (string.IsNullOrEmpty(model.Email))
                throw new InvalidOperationException(nameof(model.Email));

            if (string.IsNullOrEmpty(model.PasswordHash))
                throw new InvalidOperationException(nameof(model.PasswordHash));

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