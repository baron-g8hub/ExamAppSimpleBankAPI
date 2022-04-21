using DataAccessLayer.Models;
using SimpleBankWebAPI.Models;

namespace SimpleBankWebAPI.Contracts
{
    public interface IPostedTransactionsRepository : IRepositoryBase<PostedTransaction>
    {
        IQueryable<PostedTransaction> SelectAll();
        IEnumerable<PostedTransaction> GetAll();
        Task<ICollection<PostedTransaction>> GetAllAsync();
        PostedTransaction GetById(int Id);
        Task<PostedTransaction> GetByIdAsync(int? Id);
        Task AddAsync(PostedTransaction entity);
        void Save();
        Task<int> SaveAsync(CancellationToken cancellationtoken);
        Task<int> PostTransactionAsync(CancellationToken ct, PostingTransactionWrapper wrapper);
    }
}