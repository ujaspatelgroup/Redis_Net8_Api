using RedisNet8Api.DTOs.Shared;
using RedisNet8Api.DTOs.Student;

namespace RedisNet8Api.Services.Student
{
    public interface IStudentService
    {
        public Task<ServiceResponse<List<GetStudentDto>>> GetAllStudentsAsync();

        public Task<ServiceResponse<GetStudentDto>> GetStudentAsync(int id);

        public Task<ServiceResponse<GetStudentDto>> AddStudentAsync(AddStudentDto student);

        public Task<ServiceResponse<GetStudentDto>> UpdateStudentAsync(UpdateStudentDto student);

        public Task<ServiceResponse<List<GetStudentDto>>> DeleteStudentAsync(int id);
    }
}
