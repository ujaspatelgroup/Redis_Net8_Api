using Microsoft.AspNetCore.Mvc;
using RedisNet8Api.DTOs.Shared;
using RedisNet8Api.DTOs.Student;
using RedisNet8Api.Services.Student;

namespace RedisNet8Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ServiceResponse<List<GetStudentDto>>> Get()
        {
            return await _studentService.GetAllStudentsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse<GetStudentDto>> Get(int id)
        {
            return await _studentService.GetStudentAsync(id);
        }

        [HttpPost]
        public async Task<ServiceResponse<GetStudentDto>> AddStudent(AddStudentDto student)
        {
            return await _studentService.AddStudentAsync(student);
        }

        [HttpPut]
        public async Task<ServiceResponse<GetStudentDto>> UpdateStudent(UpdateStudentDto student)
        {
            return await _studentService.UpdateStudentAsync(student);
        }

        [HttpDelete]
        public async Task<ServiceResponse<List<GetStudentDto>>> DeleteStudent(int id)
        {
            return await _studentService.DeleteStudentAsync(id);
        }
    }
}
