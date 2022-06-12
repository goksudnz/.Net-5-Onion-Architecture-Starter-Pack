// Copyrights(c) Charqe.io. All rights reserved.

using System;

namespace DataAccessLayer.Generic
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, new();
        int SaveChanges();
    }
}