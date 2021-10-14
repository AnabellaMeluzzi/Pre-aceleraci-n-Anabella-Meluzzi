using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Interfaces
{
    interface IBaseRepository<TEntity>
        where TEntity : class

    {
        TEntity Get(int id);

        List<TEntity> GetAll();

        void Add(TEntity entity);

        void Put(TEntity entity);

        void Delete(int id);

        void Save();
    }
}
