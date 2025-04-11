using FinalProject360.Repositorio.Contexto;

public abstract class BaseRepositorio
{
    protected readonly BarbeariaDbContext _dbContext;

    protected BaseRepositorio(BarbeariaDbContext contexto)
    {
        _dbContext = contexto;
    }
}