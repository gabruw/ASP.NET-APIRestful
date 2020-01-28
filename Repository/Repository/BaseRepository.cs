using System.Linq;
using Domain.IRepository;
using Repository.Context;
using System.Collections.Generic;

namespace Repository.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly APITestContext APITestContext;

        public BaseRepository(APITestContext apiTestContext)
        {
            APITestContext = apiTestContext;
        }

        public void Incluid(TEntity entity)
        {
            APITestContext.Set<TEntity>().Add(entity);
            APITestContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            APITestContext.Set<TEntity>().Update(entity);
            APITestContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            APITestContext.Remove(entity);
            APITestContext.SaveChanges();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return APITestContext.Set<TEntity>().ToList();
        }

        public TEntity GetbyId(long Id)
        {
            return APITestContext.Set<TEntity>().Find(Id);
        }

        public void SaveChanges()
        {
            APITestContext.SaveChanges();
        }

        public void Dispose()
        {
            APITestContext.Dispose();
        }
    }
}
