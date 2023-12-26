


using PortfolioManagement_API.Models;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository.IRepostiory
{
    public interface IProjectDetailsRepository : IRepository<ProjectDetails>
    {
      
        Task<ProjectDetails> UpdateAsync(ProjectDetails entity);
  
    }
}
