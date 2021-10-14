using DisneyAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisneyAPI.Repositories
{
    public abstract class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        //By dependency injection
        protected readonly TContext _dbContext;

        private DbSet<TEntity> _dbSet;

        protected DbSet<TEntity> DbSet => _dbSet ??= _dbContext.Set<TEntity>(); // This is used for setting the type of entity in the context

        protected BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Get(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public List<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public void Add(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entity);
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Put(TEntity entity)
        {
            try
            {
                _dbContext.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            var entity = _dbContext.Find<TEntity>(id);

            try
            {
                _dbContext.Remove(entity);
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
