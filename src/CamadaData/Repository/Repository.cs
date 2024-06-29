using CamadaData.Context;

namespace CamadaData.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly MeuDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(MeuDbContext db)
    {
        Db = db;
        DbSet = Db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity> ObterPorId(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<TEntity>> ObterTodos()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task Adicionar(TEntity entity)
    {
        DbSet.Add(entity);
        await SavaChanges();
    }

    public virtual async Task Atualizar(TEntity entity)
    {
        DbSet.Update(entity);
        await SavaChanges();
    }

    public  virtual async Task Remover(Guid id)
    {
        //var entity = await DbSet.FindAsync(id); //O remover não precisa do objeto todo, apenas da instancia do TEntity com o Id.
        var entity = new TEntity { Id = id };
        DbSet.Remove(entity);
        await SavaChanges();
    }

    public async Task<int> SavaChanges()
    {
        return await Db.SaveChangesAsync();
    }
    public void Dispose()
    {
        Db?.Dispose();
    }
}
