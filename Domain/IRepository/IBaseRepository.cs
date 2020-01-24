using System;
using System.Collections.Generic;

namespace Domain.IRepository
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        void Incluid(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        IEnumerable<TEntity> GetAll();

        TEntity GetbyId(long id);

        void SaveChanges();
    }
}
