using DataAccess.Interfaces;
using DataAccess.Models;


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
