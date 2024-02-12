using DreamCatcher.Core.Models;

namespace DreamCatcher.Core.Servicees.DreamServiss
{
    public interface IDreamServiss : IService
    {
        public List<Dream> GetDreams();
        public List<Dream> GetDreamsOfType(DreamType type);
        public List<Dream> GetDreamsByDate(DateTime date);
        public List<Dream> GetDreamsYearMonth(int year, int month);

        public void WriteDownDream(Dream dream);
        public void UpdateDream(Guid id, Dream newValues);

        public Dream? GetById(Guid id);
        public List<Dream> GetByIds(Guid[] ids);

        public void DeleteDream(Guid id);
        public void DeleteDream(Dream dream);
    }
}
