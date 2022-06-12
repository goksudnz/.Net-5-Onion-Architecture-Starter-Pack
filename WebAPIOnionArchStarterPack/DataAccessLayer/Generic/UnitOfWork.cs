// Copyrights(c) Charqe.io. All rights reserved.

using System;
using DataAccessLayer.Events;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private bool _disposed = false;
        private bool _disposing = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application database context.</param>
        /// <param name="accessor">Http context accessor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _context = context ?? throw new ArgumentNullException(DalConstants.DatabaseConstants.DbContextNullException);
            _accessor = accessor;
        }
        
        /// <summary>
        /// Gets generic repository of entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>Repository of entity.</returns>
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            var repository = new GenericRepository<TEntity>(_context, _accessor);
            
            // ensure not added previously.
            repository.DataChanged -= OnDataHistoryCreated;

            // adding event listener to repository.
            repository.DataChanged += OnDataHistoryCreated;

            return repository;
        }

        /// <summary>
        /// Using for saving modified changes.
        /// </summary>
        /// <returns>Effected rows count.</returns>
        public int SaveChanges() => _context.SaveChanges();
        
        /// <summary>
        /// Dispose UnitOfWork object and finalize with garbage collector.
        /// </summary>
        public void Dispose()
        {
            Dispose(_disposing);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing context.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
                if (!disposing)
                {
                    _disposing = true;
                    _context.Dispose();
                    _disposing = false;
                }

            _disposed = true;
        }

        /// <summary>
        /// Logging data changes to logger db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDataHistoryCreated(object sender, DataHistoryEventArgs e)
        {
            // TODO: We are going to log entity changes to logger db.
        }
    }
}