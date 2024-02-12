namespace DreamCatcher.Infrastructure.Database
{
    public interface IDatabaseFactory
    {
        DCContext Get();
    }
}
