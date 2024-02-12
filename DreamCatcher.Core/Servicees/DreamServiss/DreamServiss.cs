using DreamCatcher.Core.Models;
using DreamCatcher.Core.Repositories;

namespace DreamCatcher.Core.Servicees.DreamServiss
{
    //private readonly IUnitOfWork _unitOfWork;

    public class DreamServiss : ServissBase, IDreamServiss
    {
        private IDreamRepository _dreamRepository;

        public DreamServiss(IDreamRepository dreamRepository)
        {
            _dreamRepository = dreamRepository;
        }

        public void DeleteDream(Guid id) => _dreamRepository.DeleteDream(id);

        public void DeleteDream(Dream dream) => _dreamRepository.DeleteDream(dream.Id);

        public Dream? GetById(Guid id) => _dreamRepository.GetById(id);

        public List<Dream> GetByIds(Guid[] ids) => _dreamRepository.GetByIds(ids);

        public List<Dream> GetDreams() => _dreamRepository.GetDreams();

        public List<Dream> GetDreamsByDate(DateTime date) => _dreamRepository.GetDreamsByDate(date);

        public List<Dream> GetDreamsOfType(DreamType type) => _dreamRepository.GetDreamsOfType(type);

        public List<Dream> GetDreamsYearMonth(int year, int month) => _dreamRepository.GetDreamsYearMonth(year, month);

        public void UpdateDream(Guid id, Dream newValues) => _dreamRepository.UpdateDream(id, newValues);

        public void WriteDownDream(Dream dream) => _dreamRepository.WriteDownDream(dream);
    }
}
