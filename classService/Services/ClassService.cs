using classService.DTOs;
using classService.Models;
using classService.Repositories.Interfaces;
using classService.Services.Interfaces;
namespace classService.Services
{
    public class ClassServices : IClassService
    {
        private readonly IClassesRepository _repository;

        public ClassServices(IClassesRepository repository)
        {
            _repository = repository;
        }
        public List<ClassesResponse> GetAllClasses()
        {
            var classes = _repository.GetAllClasses();
            return classes.Select(c => new ClassesResponse
            {
                class_id = c.class_id,
                course_id = c.course_id,
                teacher_id = c.teacher_id,
                class_name = c.class_name,
                start_date = c.start_date,
                end_date = c.end_date,
                max_students = c.max_students,
                class_status = c.class_status
            }).ToList();
        }
    }
}
