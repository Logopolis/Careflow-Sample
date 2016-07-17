using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RestApi.Models.Testing
{
    /// <summary>
    /// A version of DbSet which uses in-memory storage
    /// rather than a database
    /// </summary>
    public class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity>
        where TEntity : class
    {
        /* A more complete implementation of TestDbSet can be found at
         * https://msdn.microsoft.com/en-gb/data/dn314431.aspx,
         * which was used as a guide to create this more limited one.
         */

        private readonly ObservableCollection<TEntity> _data;
        private readonly IQueryable _query;

        public TestDbSet()
        {
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        public override TEntity Add(TEntity entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            entities.Select(e => Add(e));
            return entities;
        }

        public override TEntity Attach(TEntity entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override TEntity Remove(TEntity entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public override IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            entities.Select(e => Remove(e));
            return entities;
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public Type ElementType
        {
            get
            {
                return _query.ElementType;
            }
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        public Expression Expression
        {
            get
            {
                return _query.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _query.Provider;
            }
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}