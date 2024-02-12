namespace DreamCatcher.Infrastructure.Database
{
    public class DatabaseFactory : IDatabaseFactory, IDisposable
    {
        private DCContext? _db;

        public DCContext Get()
        {
            if (_db == null)
                _db = new DCContext();
            return _db;
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
        }
    }
}
