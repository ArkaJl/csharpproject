
using Domain.Models;


namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User?> GetById(string id);
        Task Create(User model);
        Task Update(User model);
        Task Delete(string id); 
    }
}