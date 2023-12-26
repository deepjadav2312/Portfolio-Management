
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository;
using PortfolioManagement_API.Repository.IRepostiory;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository
{
    public class ProjectDetailsRepository : Repository<ProjectDetails>, IProjectDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ProjectDetails> UpdateAsync(ProjectDetails entity)
        {
            _db.ProjectDetailses.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
