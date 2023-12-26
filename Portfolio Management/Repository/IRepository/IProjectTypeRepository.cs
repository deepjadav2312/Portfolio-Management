
using PortfolioManagement_API.Models;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository.IRepostiory
{
    public interface IProjectTypeRepository : IRepository<ProjectType>
    {
      
        Task<ProjectType> UpdateAsync(ProjectType entity);
  
    }
}
