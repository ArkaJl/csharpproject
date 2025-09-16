using DataAccess.Models;


namespace BusinessLogic.Interfaces
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