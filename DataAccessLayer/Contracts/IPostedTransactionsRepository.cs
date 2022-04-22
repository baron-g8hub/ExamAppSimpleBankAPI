using DataAccessLayer.Models;


namespace DataAccessLayer.Contracts
{
    public interface IPostedTransactionsRepository : IRepositoryBase<PostedTransaction>
    {
        IQueryable<PostedTransaction> SelectAll();
        IEnumerable<PostedTransaction> GetAll();
        Task<ICollection<PostedTransaction>> GetAllAsync();
        PostedTransaction GetById(int Id);
        Task<PostedTransaction> GetByIdAsync(int? Id);
        void AddTransaction(PostedTransaction entity);
        Task AddTransactionAsync(PostedTransaction entity);
        void Save();
        Task<int> SaveAsync(CancellationToken cancellationtoken);
      // Task<int> PostTransactionAsync(CancellationToken ct, PostingTransactionWrapper wrapper);
    }
}