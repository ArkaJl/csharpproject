
using Domain.Interfaces.Repositories;
using Domain.Models;


namespace DataAccess.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}
