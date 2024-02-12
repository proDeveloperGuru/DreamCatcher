using DreamCatcher.Infrastructure.Database;

namespace DreamCatcher.Infrastructure.Repositories
{
    public interface IRepositoryBase
    {

    }

    public class RepositoryBase : IRepositoryBase
    {
        public DCContext DreamDiaryContext { 
            get
            {
                return _factory.Get();
            } 
        }

        private IDatabaseFactory _factory;
        public RepositoryBase(IDatabaseFactory factory) { 
            _factory = factory;
        }
    }
}
