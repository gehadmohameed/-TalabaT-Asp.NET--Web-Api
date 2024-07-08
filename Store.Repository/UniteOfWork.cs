using Store.Core;
using Store.Core.Entites;
using Store.Core.Repositories;
using Store.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class UniteOfWork : IUniteOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;

        public UniteOfWork(StoreContext dbContext)
        {
              _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        =>   await _dbContext.SaveChangesAsync();



        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(Type))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(Type, Repository);

            }
            return _repositories[Type] as IGenericRepository<TEntity>;


        }
    }
}
