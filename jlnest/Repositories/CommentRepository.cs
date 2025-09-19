using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;


namespace DataAccess.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}
