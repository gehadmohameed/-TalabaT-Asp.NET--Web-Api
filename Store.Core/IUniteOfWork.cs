using Store.Core.Entites;
using Store.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core
{
    public interface IUniteOfWork :IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository <TEntity> () where TEntity : BaseEntity;


        Task<int> CompleteAsync();

    }
}
