using System;
using System.Data.Entity;

namespace RestApi.Models.Testing
{
    public class TestDbContext : DbContext
    {
        public override int SaveChanges()
        {
            return 0;
        }

        public override DbSet Set(Type entityType)
        {
            return Set<Type>();
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return new TestDbSet<TEntity>();
        }
    }
}