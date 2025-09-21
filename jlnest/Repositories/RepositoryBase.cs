
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected JlnestContext RepositoryContext { get; set; }

        public RepositoryBase(JlnestContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            RepositoryContext.Set<T>().Where(expression).AsNoTracking();

        public async Task Create(T entity) => await RepositoryContext.Set<T>().AddAsync(entity);

        public async Task Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
            await Task.CompletedTask;
        }

        public async Task Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public async Task SaveAsync() => await RepositoryContext.SaveChangesAsync();
    }
}
