// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.POs;

namespace DataAccessLayer.Generic
{
    public interface IGenericRepository<TEntity> where TEntity : class, new()
    {
        TEntity Get(object id);
        TOut Get<TOut>(object id, Expression<Func<TEntity, TOut>> @select);
        TEntity Get(Expression<Func<TEntity, bool>> @where);
        TOut Get<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select);

        IEnumerable<TEntity> GetAll(out int totalCount, SqlConfigPO<TEntity> config = null);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> @where, out int totalCount, SqlConfigPO<TEntity> config = null);
        IEnumerable<TOut> GetAll<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select, out int totalCount, SqlConfigPO<TEntity> config = null);

        void Insert(TEntity entity);
        void Update(TEntity entity);

        void InsertRange(List<TEntity> entities);
        void UpdateRange(List<TEntity> entities);

        void Remove(object id);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entities);

    }
}