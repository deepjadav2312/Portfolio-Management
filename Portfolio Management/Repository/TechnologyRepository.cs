


using Microsoft.EntityFrameworkCore;
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository;
using PortfolioManagement_API.Repository.IRepostiory;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository
{
    public class TechnologyRepository : Repository<Technology>, ITechnologyRepository
    {
        private readonly ApplicationDbContext _db;
        public TechnologyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

  
        public async Task<Technology> UpdateAsync(Technology entity)
        {
         
            _db.Technologies.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
