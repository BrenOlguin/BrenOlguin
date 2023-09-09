using EjemploApi6.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EjemploApi6.DataAccess.EntityFramework
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IGenericRepository where TEntity : class
    {
        private readonly bool _shareContext;

        public GenericRepository()
        {
        }

        public GenericRepository(DbContext context)
        {
            Context = context;
            _shareContext = true;
        }

        public DbContext Context { get; }

        private DbSet<TEntity>? _dbSet;

        protected DbSet<TEntity?>? DbSet
        {
            get { return _dbSet ??= Context.Set<TEntity>(); }
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass 
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Should free managed/unmanaged resources here
            if (disposing && !_shareContext && (Context != null))
            {
                //Context.Dispose();
            }
        }

        public virtual IQueryable<TEntity?> All()
        {
            return All(false);
        }

        public virtual IQueryable<TEntity?> All(bool noTracking)
        {
            var result = DbSet.AsQueryable();
            return noTracking ? result.AsNoTracking() : result;
        }

        public virtual IQueryable<TEntity?> Filter(Expression<Func<TEntity?, bool>> predicate)
        {
            return Filter(predicate, false);
        }

        public virtual IQueryable<TEntity?> Filter(Expression<Func<TEntity?, bool>> predicate, bool noTracking)
        {
            var result = DbSet.Where(predicate).AsQueryable();

            return noTracking ? result.AsNoTracking() : result;
        }

        public virtual IQueryable<TEntity?> Filter(Expression<Func<TEntity?, bool>> filter, out int total, int index = 0,
            int size = 50)
        {
            return Filter(filter, out total, false, index, size);
        }

        public virtual IQueryable<TEntity?> Filter(Expression<Func<TEntity?, bool>> filter, out int total,
            bool noTracking, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();

            return noTracking ? resetSet.AsQueryable().AsNoTracking() : resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TEntity?, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public virtual TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TEntity Find(Expression<Func<TEntity?, bool>> predicate)
        {
            return DbSet.Where(predicate).FirstOrDefault();
        }

        public virtual TEntity Find(Expression<Func<TEntity?, bool>> predicate, bool noTracking)
        {

            return noTracking
                ? DbSet.Where(predicate).AsNoTracking().FirstOrDefault()
                : DbSet.Where(predicate).FirstOrDefault();
        }

        public virtual async Task<TEntity?> LastAsync(string includeProperties = "")
        {

            IQueryable<TEntity?> query = Includes(includeProperties);

            return await query.LastAsync();
        }

        public virtual TEntity? Add(TEntity? entity)
        {
            var newEntry = DbSet.Add(entity);
            //TODO Context.SaveChanges();
            return newEntry.Entity;
        }

        public virtual int Count => DbSet.Count();

        public virtual int Delete(TEntity? entity)
        {
            DbSet.Remove(entity);
            return Context.SaveChanges();
        }

        public virtual int Delete(Expression<Func<TEntity?, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            return Context.SaveChanges();
        }

        public virtual int Update(TEntity? entity)
        {
            var entry = Context.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
            return Context.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity? entity)
        {
            var entry = Context.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds an non-typed object to the repository. Checks the type before adding to see if it is comaptible 
        /// with the repository
        /// </summary>
        /// <param name="entity"></param>
        public void AddObject(object entity)
        {
            if (entity.GetType() == typeof(TEntity))
            {
                Add((TEntity)entity);
            }
        }

        /// <summary>
        /// Updates an non-typed object to the repository. Checks the type before updating to see if it is comaptible 
        /// with the repository
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateObject(object entity)
        {
            if (entity.GetType() == typeof(TEntity))
            {
                Update((TEntity)entity);
            }
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }


        public IQueryable<TEntity?> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity?>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")

        {
            IQueryable<TEntity?> query = Includes(includeProperties);

            if (filter != null)
                query = query.Where(filter);

            return orderBy?.Invoke(query).AsQueryable() ?? query;
        }

        public async Task<TEntity?> GetOrCreateAsync(Expression<Func<TEntity, bool>>? filter, TEntity? entity)
        {
            IQueryable<TEntity?> query = Get(filter);

            if (query.Any())
            {
                return await query.FirstOrDefaultAsync();
            }

            return await AddAsync(entity);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IQueryable<TEntity?> Includes(string includeProperties)
        {
            var query1 = DbSet.AsQueryable();
            foreach (var includeProperty in includeProperties.Split(
                new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query1 = query1.Include(includeProperty);

            return query1;

        }

        public virtual async Task<TEntity?> AddAsync(TEntity? entity)
        {
            var newEntry = await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return newEntry.Entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(TEntity? entity)
        {
            DbSet.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity?, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            return await Context.SaveChangesAsync();
        }

        public async Task ReLoadAsync(TEntity entity)
        {
            await Context.Entry(entity).ReloadAsync();
        }
    }
}
