using Microsoft.EntityFrameworkCore;
using Store.Core.Entites;
using Store.Core.Repositories;
using Store.Core.Specifications;
using Store.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
           _dbContext  = dbContext;
        }
        #region WithOut Spec
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();

            }
            else
            {
                return await _dbContext.Set<T>().ToListAsync();

            }
        }

       public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        #endregion


        #region with spec

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifiacation(Spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifiacation(Spec).FirstOrDefaultAsync();
            


        }
        private IQueryable<T> ApplySpecifiacation(ISpecifications<T> Spec)
        {
            return  SpecifiacationEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifiacation(Spec).CountAsync();
        }

        public async Task Add(T item)
        
        =>    await _dbContext.Set<T>().AddAsync(item);

        public void Delete(T item)
        => _dbContext.Set<T>().Remove(item);

        public void Update(T item)
        => _dbContext.Set<T>().Update(item);

        #endregion
    }
}
