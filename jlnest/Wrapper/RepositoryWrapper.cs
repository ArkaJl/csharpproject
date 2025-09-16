using DataAccess.Interfaces;
using DataAccess.Repositories;
using DataAccess.Models;

namespace DataAccess.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly JlnestContext _repoContext;
        private IUserRepository? _user; // Добавьте nullable

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public RepositoryWrapper(JlnestContext repositoryContext)
        {
            _repoContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}