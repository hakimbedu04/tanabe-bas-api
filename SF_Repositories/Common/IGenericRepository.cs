﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SF_Repositories.Common
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void DetachAll();
        int Save();
        void Save(string controller, string userid);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);

        void UpdateNew(TEntity entityToUpdate);
        void InsertOrUpdate(TEntity entity);
        bool Exists(TEntity entity);
        void DeleteAll();
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        TEntity GetByID(object id);

        TEntity GetByName(object name);
        TEntity GetByID(params object[] keyValues);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IEnumerable<TEntity> Get(int pageIndex, int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
            );

        void BeginTransaction(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted | System.Data.IsolationLevel.Snapshot);
        void EndTransaction(bool commit = true);

        /// <summary>
        /// Set to true to load related entities automatically.
        /// Set to false if you want faster performance. 
        /// Especially useful for data that might have circular reference.
        /// </summary>
        bool AllowLazyLoading { get; set; }
    }
}
