// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Models.General;
using Domain.POs;

namespace BusinessLogicLayer.Generic
{
    public interface IGenericService<TEntity> where TEntity : class, new()
    {
        ServiceResultExt<TEntity> Get(object id);
        ServiceResultExt<TOut> Get<TOut>(object id, Expression<Func<TEntity, TOut>> @select) where TOut : class , new();
        ServiceResultExt<TEntity> Get(Expression<Func<TEntity, bool>> @where);
        ServiceResultExt<TOut> Get<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select) where TOut : class, new();

        ServiceResultScalarExt<IEnumerable<TEntity>> GetAll(SqlConfigPO<TEntity> config = null);
        ServiceResultScalarExt<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> @where, SqlConfigPO<TEntity> config = null);
        ServiceResultScalarExt<IEnumerable<TOut>> GetAll<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select, SqlConfigPO<TEntity> config = null) where TOut : class, new();

        ServiceResult Insert(TEntity entity);
        ServiceResult Update(TEntity entity);

        ServiceResult InsertRange(List<TEntity> entities);
        ServiceResult UpdateRange(List<TEntity> entities);

        ServiceResult Remove(TEntity entity);
        ServiceResult Remove(object id);
        ServiceResult RemoveRange(List<TEntity> entities);
    }
}