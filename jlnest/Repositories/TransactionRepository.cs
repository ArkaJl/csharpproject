using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}
