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
        Task AddTransactionAsync(PostedTransaction entity);
    
     
      
    }
}