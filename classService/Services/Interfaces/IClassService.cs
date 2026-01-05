using classService.DTOs;

namespace classService.Services.Interfaces
{
    public interface IClassService
    {
        List<ClassesResponse> GetAllClasses();
    }
}
