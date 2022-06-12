// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccessLayer.Generic;
using Domain.Models.General;
using Domain.POs;

namespace BusinessLogicLayer.Generic
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class, new()
    {
        protected readonly IUnitOfWork UnitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public GenericService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _genericRepository = UnitOfWork.Repository<TEntity>();
        }

        /// <summary>
        /// Using for get entity with primary key.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <returns>Service result with entity object.</returns>
        public ServiceResultExt<TEntity> Get(object id)
        {
            var res = _genericRepository.Get(id);
            return res.ToServiceResultExt();
        }

        /// <summary>
        /// Using for get entity with primary key with select query expression.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <param name="select">Select query expression.</param>
        /// <typeparam name="TOut">Mapped object type.</typeparam>
        /// <returns>Service result with referenced mapped object.</returns>
        public ServiceResultExt<TOut> Get<TOut>(object id, Expression<Func<TEntity, TOut>> @select) where TOut : class, new()
        {
            var res = _genericRepository.Get(id, @select);
            return res.ToServiceResultExt();
        }

        /// <summary>
        /// Using for get entity with where query expression.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <returns>Service result with entity object.</returns>
        public ServiceResultExt<TEntity> Get(Expression<Func<TEntity, bool>> @where)
        {
            var res = _genericRepository.Get(@where);
            return res.ToServiceResultExt();
        }

        /// <summary>
        /// Using for get entity with where and select query expressions.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <param name="select">Select query expression.</param>
        /// <typeparam name="TOut">Mapped object type.</typeparam>
        /// <returns>Service result with referenced mapped object.</returns>
        public ServiceResultExt<TOut> Get<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select) where TOut : class, new()
        {
            var res = _genericRepository.Get(@where, @select);
            return res.ToServiceResultExt();
        }

        /// <summary>
        /// Using for get all entities.
        /// </summary>
        /// <param name="config">Sql configuration model.</param>
        /// <returns>Service result with list of entities.</returns>
        public ServiceResultScalarExt<IEnumerable<TEntity>> GetAll(SqlConfigPO<TEntity> config = null)
        {
            var res = _genericRepository.GetAll(out var totalCount, config);
            return res.ToServiceResultExt(totalCount);
        }

        /// <summary>
        /// Using for get all entities by filtering where query expression.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <param name="config">Sql configuration model.</param>
        /// <returns>Service result with list of entities.</returns>
        public ServiceResultScalarExt<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> @where, SqlConfigPO<TEntity> config = null)
        {
            var res = _genericRepository.GetAll(@where, out var totalCount, config);
            return res.ToServiceResultExt(totalCount);
        }

        /// <summary>
        /// Using for get all entities by filtering where and modelling by select query expressions.
        /// </summary>
        /// <param name="where">Where query expressions.</param>
        /// <param name="select">Select query expressions.</param>
        /// <param name="config">Sql configuration model.</param>
        /// <typeparam name="TOut">Mapped object type.</typeparam>
        /// <returns>Service result with list of referenced mapped objects.</returns>
        public ServiceResultScalarExt<IEnumerable<TOut>> GetAll<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select, SqlConfigPO<TEntity> config = null) where TOut : class, new()
        {
            var res = _genericRepository.GetAll(@where, @select, out var totalCount, config);
            return res.ToServiceResultExt(totalCount);
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Service result.</returns>
        public ServiceResult Insert(TEntity entity)
        {
            _genericRepository.Insert(entity);
            return new ServiceResult();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Service result</returns>
        public ServiceResult Update(TEntity entity)
        {
            _genericRepository.Update(entity);
            return new ServiceResult();
        }

        /// <summary>
        /// Inserts entities.
        /// </summary>
        /// <param name="entities">Entity objects.</param>
        /// <returns>Service result.</returns>
        public ServiceResult InsertRange(List<TEntity> entities)
        {
            _genericRepository.InsertRange(entities);
            return new ServiceResult();
        }

        /// <summary>
        /// Updates entities.
        /// </summary>
        /// <param name="entities">Entity objects.</param>
        /// <returns>Service result.</returns>
        public ServiceResult UpdateRange(List<TEntity> entities)
        {
            _genericRepository.UpdateRange(entities);
            return new ServiceResult();
        }

        /// <summary>
        /// Removes entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Service result.</returns>
        public ServiceResult Remove(TEntity entity)
        {
            _genericRepository.Remove(entity);
            return new ServiceResult();
        }

        /// <summary>
        /// Removes entity.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <returns>Service result.</returns>
        public ServiceResult Remove(object id)
        {
            _genericRepository.Remove(id);
            return new ServiceResult();
        }

        /// <summary>
        /// Removes entities.
        /// </summary>
        /// <param name="entities">Entity objects.</param>
        /// <returns>Service result.</returns>
        public ServiceResult RemoveRange(List<TEntity> entities)
        {
            _genericRepository.RemoveRange(entities);
            return new ServiceResult();
        }
    }
}