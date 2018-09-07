namespace Mizuho.London.FinanceLedgerPosting.Repository
{
    //public class GenericRepository<T> : IRepository<T>
    //    where T : class

    //{
    //    protected internal FinanceLedgerPostingDbContext Context { get; }
    //    public GenericRepository(IFinanceLedgerPostingDbContext context)
    //    {
    //        Context = context;
    //    }

    //    public T Create(T model)
    //    {
    //        var newModel = Context.Set<T>().Add(model);
    //        Context.SaveChanges();
    //        return newModel;
    //    }

    //    internal virtual async Task<T> CreateAsync(T model)
    //    {
    //        var newModel = Context.Set<T>().Add(model);
    //        await Context.SaveChangesAsync();
    //        return newModel;
    //    }

    //    public T Read(int id)
    //    {
    //      // return Context.Set<T>().First(m => m.Id == id);
    //      return Context.Set<T>().AsNoTracking().First(m => m.Id == id);

    //    }

    //    public T ReadAsNoTracking(int id)
    //    {
    //        //return Context.Set<T>().First(m => m.Id == id);
    //        return Context.Set<T>().AsNoTracking().First(m => m.Id == id);

    //    }

    //    public IEnumerable<T> Read()
    //    {
    //      // return Context.Set<T>().ToList().AsEnumerable();
    //      return Context.Set<T>().AsNoTracking().ToList().AsEnumerable();
    //    }

    //    public IEnumerable<T> ReadAsNoTracking()
    //    {
    //        // return Context.Set<T>().ToList().AsEnumerable();
    //        return Context.Set<T>().AsNoTracking().ToList().AsEnumerable();
    //    }


    //    public virtual T Find(Expression<Func<T, bool>> predicate)
    //    {
    //      //  return Context.Set<T>().SingleOrDefault(predicate);
    //       return Context.Set<T>().AsNoTracking().SingleOrDefault(predicate);
    //    }

    //    public virtual T FindAsNoTracking(Expression<Func<T, bool>> predicate)
    //    {
    //        //  return Context.Set<T>().SingleOrDefault(predicate);
    //        return Context.Set<T>().AsNoTracking().SingleOrDefault(predicate);
    //    }



    //    public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
    //    {
    //       //return await Context.Set<T>().SingleOrDefaultAsync(predicate);
    //      return await Context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
    //    }

    //    public virtual async Task<T> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate)
    //    {
    //        //return await Context.Set<T>().SingleOrDefaultAsync(predicate);
    //        return await Context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
    //    }


    //    public virtual IList<T> FindAll(Expression<Func<T, bool>> predicate)
    //    {
    //       // return Context.Set<T>().Where(predicate).ToList();
    //       return Context.Set<T>().Where(predicate).AsNoTracking().ToList();
    //    }

    //    public virtual IList<T> FindAllAsNoTracking(Expression<Func<T, bool>> predicate)
    //    {
    //        //return Context.Set<T>().Where(predicate).ToList();
    //        return Context.Set<T>().Where(predicate).AsNoTracking().ToList();
    //    }

    //    public virtual T Update(T model)
    //    {
    //        if (model == null)
    //            return null;

    //        var existing = Read(model.Id);
    //        if (existing == null)
    //            return null;

            
    //        Context.ModifyEntity(existing, model);
    //        Context.SaveChanges();
    //        return model;
    //    }

    //    public virtual void Delete(T model)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public T UpdateDb(T model)
    //    //public void SaveChanges()
    //    {
    //        Context.Entry(model).State = EntityState.Modified;
    //        Context.SaveChanges();
    //        return model;
    //    }

   // }
}
