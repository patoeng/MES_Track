
using System.Data.Entity;
using System.Linq;


namespace MES.Data
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        private readonly IDbSet<T> _set;

        public EfRepository(DbContext context)
        {
            this._context = context;
            this._set = context.Set<T>();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }

        public IQueryable<T> All()
        {
            return this._set;
        }

        public T GetById(object id)
        {
            return this._set.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void Delete(object id)
        {
            this.Delete(this.GetById(id));
        }

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this._context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this._set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
