


using PortfolioManagement_API.Models;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository.IRepostiory
{
    public interface ITechnologyRepository : IRepository<Technology>
    {
      
        Task<Technology> UpdateAsync(Technology entity);
  
    }
}
