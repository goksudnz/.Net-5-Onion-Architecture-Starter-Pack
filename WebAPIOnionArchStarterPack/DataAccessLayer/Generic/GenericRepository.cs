// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccessLayer.Events;
using DataAccessLayer.Helpers;
using Domain.Entities;
using Domain.Entities.LogEntities;
using Domain.Interfaces;
using Domain.POs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataAccessLayer.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        public event EventHandler<DataHistoryEventArgs> DataChanged;
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly string _userId;
        private readonly string _userName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="accessor">Http Context Accessor</param>
        public GenericRepository(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _context = context ?? throw new ArgumentNullException(DALConstants.DatabaseConstants.DbContextNullException);
            _userName = accessor.HttpContext?.User?.Identity?.Name;
            _userId = string.IsNullOrEmpty(_userName)
                ? string.Empty
                : context.Set<AppUser>().FirstOrDefault(x => x.UserName.ToLower() == _userName.ToLower())?.Id;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Gets entity by id.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <returns>Referenced entity object.</returns>
        public TEntity Get(object id) => _dbSet.Find(id);

        /// <summary>
        /// Gets entity by id and return it as mapped object.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <param name="select">Select query expression.</param>
        /// <typeparam name="TOut">Mapped object.</typeparam>
        /// <returns>Referenced mapped object.</returns>
        public TOut Get<TOut>(object id, Expression<Func<TEntity, TOut>> @select) => _dbSet.Select(@select)
            .FirstOrDefault(x => ((object)x.GetType().GetProperty("Id").GetValue(x)).ToString() == id.ToString());

        /// <summary>
        /// Gets entity by where query.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <returns>Referenced entity object.</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> @where) => _dbSet.FirstOrDefault(@where);

        /// <summary>
        /// Gets entity by where query and return it as mapped object.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <param name="select">Select query expression.</param>
        /// <typeparam name="TOut">Mapped object.</typeparam>
        /// <returns>Referenced mapped object.</returns>
        public TOut Get<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select) => _dbSet.Where(@where).Select(@select).FirstOrDefault();

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="totalCount">Total count of returned list.</param>
        /// <param name="config">Sql query configuration parameters.</param>
        /// <returns>List of referenced entities.</returns>
        public IEnumerable<TEntity> GetAll(out int totalCount, SqlConfigPO<TEntity> config = null)
        {
            config ??= new SqlConfigPO<TEntity>();
            if (config.IncludeSoftDelete || !typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                var result = _dbSet.AsQueryable();
                totalCount = result.Count();
                ExecuteSqlConfig(ref result, config);
                return result;
            }

            Expression<Func<TEntity, bool>> simple = x => true;
            Expression<Func<ISoftDelete, bool>> deletedExpression = x => x.IsDeleted == false;

            var res = _dbSet.Where(simple.And(deletedExpression));
            totalCount = res.Count();
            ExecuteSqlConfig(ref res, config);
            return res;
        }

        /// <summary>
        /// Gets filtered entities by using where query.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <param name="totalCount">Total count of returned list.</param>
        /// <param name="config">Sql query configuration parameters.</param>
        /// <returns>List of referenced entities.</returns>
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> @where, out int totalCount, SqlConfigPO<TEntity> config = null)
        {
            config ??= new SqlConfigPO<TEntity>();
            if (config.IncludeSoftDelete || !typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                var result = _dbSet.Where(@where);
                totalCount = result.Count();
                ExecuteSqlConfig(ref result, config);
                return result;
            }

            Expression<Func<ISoftDelete, bool>> deletedExpression = x => x.IsDeleted == false;

            var res = _dbSet.Where(@where.And(deletedExpression));
            totalCount = res.Count();
            ExecuteSqlConfig(ref res, config);
            return res;
        }

        /// <summary>
        /// Gets filtered entities and return them as mapped object by using where and select query.
        /// </summary>
        /// <param name="where">Where query expression.</param>
        /// <param name="select">Select query expression.</param>
        /// <param name="totalCount">Total count of returned list.</param>
        /// <param name="config">Sql query configuration parameters.</param>
        /// <typeparam name="TOut">Mapped object.</typeparam>
        /// <returns>List of referenced mapped objects.</returns>
        public IEnumerable<TOut> GetAll<TOut>(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TOut>> @select, out int totalCount, SqlConfigPO<TEntity> config = null)
        {
            config ??= new SqlConfigPO<TEntity>();
            if (config.IncludeSoftDelete || !typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                var result = _dbSet.Where(@where);
                totalCount = result.Count();
                ExecuteSqlConfig(ref result, config);
                return result.Select(@select);
            }

            Expression<Func<ISoftDelete, bool>> deletedExpression = x => x.IsDeleted == false;

            var res = _dbSet.Where(@where.And(deletedExpression));
            totalCount = res.Count();
            ExecuteSqlConfig(ref res, config);
            return res.Select(@select);
        }

        /// <summary>
        /// Inserts entity.
        /// </summary>
        /// <param name="entity">Entity which is going to insert.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Insert(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(string.Format(DALConstants.DatabaseConstants.EntityNullException, nameof(entity)));

            if (entity is IHistoryPersisted)
            {
                var createdBy = entity.GetType().GetProperty(nameof(IHistoryPersisted.CreatedBy));
                var createDate = entity.GetType().GetProperty(nameof(IHistoryPersisted.CreateDate));

                createdBy?.SetValue(entity, _userId);
                createDate?.SetValue(entity, DateTime.UtcNow);

                DataHistoryCreator(entity, nameof(Insert));
            }

            _dbSet.Add(entity);
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        /// <param name="entity">Entity which is going to update.</param>
        public void Update(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(string.Join(DALConstants.DatabaseConstants.EntityNullException, nameof(entity)));

            if (_context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            if (entity.GetType().GetProperty(DALConstants.DatabaseConstants.HistoryPersistedCreatedBy) != null)
                _context.Entry(entity).Property(DALConstants.DatabaseConstants.HistoryPersistedCreatedBy).IsModified = false;
            if (entity.GetType().GetProperty(DALConstants.DatabaseConstants.HistoryPersistedCreateDate) != null)
                _context.Entry(entity).Property(DALConstants.DatabaseConstants.HistoryPersistedCreateDate).IsModified = false;

            if (entity is IHistoryPersisted)
                DataHistoryCreator(entity, nameof(Update));
        }

        /// <summary>
        /// Inserts entities.
        /// </summary>
        /// <param name="entities">Entities which are going to insert.</param>
        public void InsertRange(List<TEntity> entities)
        {
            if (entities is null)
                throw new ArgumentNullException(string.Format(DALConstants.DatabaseConstants.EntityNullException, nameof(entities)));

            if (!entities.Any()) return;
            
            // if entity configure for save history, then entity is saving log db
            if (entities.First() is IHistoryPersisted)
            {
                foreach (var entity in entities)
                {
                    var createdBy = entity.GetType().GetProperty(nameof(IHistoryPersisted.CreatedBy));
                    var createDate = entity.GetType().GetProperty(nameof(IHistoryPersisted.CreateDate));
                    createdBy?.SetValue(entity, _userId);
                    createDate?.SetValue(entity, DateTime.UtcNow);

                    DataHistoryCreator(entity, nameof(InsertRange));
                }
            }

            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Updates entities.
        /// </summary>
        /// <param name="entities">Entities which are going to update.</param>
        public void UpdateRange(List<TEntity> entities)
        {
            if (entities is null)
                throw new ArgumentNullException(string.Format(DALConstants.DatabaseConstants.EntityNullException,
                    nameof(entities)));

            if (!entities.Any()) return;

            // if entity configure for save history, then entity is saving log db
            if (entities.First() is IHistoryPersisted)
                foreach (var entity in entities)
                    DataHistoryCreator(entity, nameof(UpdateRange));

            _dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Remove by id.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Remove(object id)
        {
            var entity = Get(id);
            if (entity is null) return;

            Remove(entity);
        }

        /// <summary>
        /// Remove by entity.
        /// </summary>
        /// <param name="entity">Entity which is going to remove.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Remove(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(string.Format(DALConstants.DatabaseConstants.EntityNullException, nameof(entity)));

            // if entity configure for soft delete, then entity is updated as deleted.
            if (entity is ISoftDelete)
            {
                entity.GetType().GetProperty(DALConstants.DatabaseConstants.IsSoftDelete)?.SetValue(entity, true);

                Update(entity);
                return;
            }

            // if entity configure for save history, then entity is saving log db
            if (entity is IHistoryPersisted)
                DataHistoryCreator(entity, nameof(Remove));

            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Removes more than one entities.
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveRange(List<TEntity> entities)
        {
            if (entities is null)
                throw new ArgumentNullException(string.Format(DALConstants.DatabaseConstants.EntityNullException, nameof(entities)));
            
            
            if (!entities.Any()) return;

            // if entity configure for soft delete, then entity is updated as deleted.
            if (entities.First() is ISoftDelete)
            {
                foreach (var entity in entities)
                {
                    var updatableEntity = entity.GetType().GetProperty(DALConstants.DatabaseConstants.IsSoftDelete);
                    updatableEntity?.SetValue(updatableEntity, true);
                }

                UpdateRange(entities);
                return;
            }

            // if entity configure for save history, then entity is saving log db
            if (entities.First() is IHistoryPersisted)
                foreach (var entity in entities)
                    DataHistoryCreator(entity, nameof(RemoveRange));

            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Get primary keys and values of entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        private Dictionary<string, object> GetPrimaryKeyColumns(TEntity entity)
        {
            var keys = _context.Model?.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties.Select(x => x.Name);
            if (keys is null) return new Dictionary<string, object>();

            var props = typeof(TEntity).GetProperties().Where(x => keys.Contains(x.Name));
            return props.ToDictionary(x => x.Name, x => typeof(TEntity).GetProperty(x.Name)?.GetValue(entity));
        }

        /// <summary>
        /// Executes sql configurations.
        /// </summary>
        /// <param name="list">List of referenced objects.</param>
        /// <param name="config">Sql configuration object.</param>
        /// <typeparam name="TOut">Referenced object.</typeparam>
        private void ExecuteSqlConfig<TOut>(ref IQueryable<TOut> list, SqlConfigPO<TOut> config) where TOut : class, new()
        {
            if (config.OrderBy is not null)
                list = config.IsDescending
                    ? list.AsEnumerable().OrderByDescending(config.OrderBy).AsQueryable()
                    : list.AsEnumerable().OrderBy(config.OrderBy).AsQueryable();

            if (config.Skip.HasValue) list = list.Skip(config.Skip.Value);
            if (config.Take.HasValue) list = list.Take(config.Take.Value);
        }

        /// <summary>
        /// Saving data history on background.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        private void DataHistoryCreator(TEntity entity, string action)
        {
            Task.Run(() =>
            {
                var primaryKeyValues = GetPrimaryKeyColumns(entity);

                var dataHistory = new DataHistory()
                {
                    Action = action,
                    EntityId = string.Join(", ", primaryKeyValues.Values),
                    EntityKeys = string.Join(", ", primaryKeyValues.Keys),
                    EntityName = typeof(TEntity).Name,
                    UserId = string.IsNullOrEmpty(_userId) ? _userName : _userId,
                    UserName = string.IsNullOrEmpty(_userName) ? string.Empty : _userName,
                    Data = JsonConvert.SerializeObject(entity)
                };

                OnDataChanged(new DataHistoryEventArgs(dataHistory));
            });
        }

        /// <summary>
        /// Triggers data history event.
        /// </summary>
        /// <param name="e"></param>
        private void OnDataChanged(DataHistoryEventArgs e) => DataChanged?.Invoke(this, e);
    }
}