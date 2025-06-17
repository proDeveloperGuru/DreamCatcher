using DreamCatcher.Core.Models;
using DreamCatcher.Core.Repositories;
using DreamCatcher.Infrastructure.Database;

namespace DreamCatcher.Infrastructure.Repositories
{
    public class DreamRepository : RepositoryBase, IDreamRepository
    {
        public DreamRepository(IDatabaseFactory factory) : base(factory) { 
        
        }

        public void DeleteDream(Guid id)
        {
            var dream = DreamDiaryContext.Dreams.Where(x => x.Id == id).FirstOrDefault();
            if(dream != null)
            {
                DreamDiaryContext.Dreams.Remove(dream);
                DreamDiaryContext.SaveChanges();
            }
        }

        public Dream? GetById(Guid id)
        {
           return DreamDiaryContext.Dreams.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Dream> GetByIds(Guid[] ids)
        {
            return DreamDiaryContext.Dreams.Where(x => ids.Contains(x.Id)).ToList();
        }

        public List<Dream> GetDreams()
        {
            return DreamDiaryContext.Dreams.ToList();
        }

        public List<Dream> GetDreamsByDate(DateTime date)
        {
            return DreamDiaryContext.Dreams.Where(x => x.DateTime.Year == date.Year && x.DateTime.Month == date.Month && x.DateTime.Date == date.Date).ToList();
        }

        public List<Dream> GetDreamsOfType(DreamType type)
        {
            return DreamDiaryContext.Dreams.Where(x => x.Type == type).ToList();
        }

        public List<Dream> GetDreamsYearMonth(int year, int month)
        {
            return DreamDiaryContext.Dreams.Where(x => x.DateTime.Year == year && x.DateTime.Month == month).ToList();
        }

        public void UpdateDream(Guid id, Dream newValues)
        {
            var dream = GetById(id);
            if (dream != null)
            {
                dream.Description = newValues.Description;
                dream.Title = newValues.Title;
                dream.DateTime = newValues.DateTime;
                dream.Intensity = newValues.Intensity;
                dream.Type = newValues.Type;
                dream.Picture = newValues.Picture;
            }

            DreamDiaryContext.SaveChanges();
        }

        public void WriteDownDream(Dream dream)
        {
            DreamDiaryContext.Dreams.Add(dream);
            DreamDiaryContext.SaveChanges();
        }
    }
}
