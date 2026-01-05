using classService.Models;
namespace classService.Repositories.Interfaces
{
    public interface IClassesRepository
    {
        public List<Classes> GetAllClasses();
    }
}
