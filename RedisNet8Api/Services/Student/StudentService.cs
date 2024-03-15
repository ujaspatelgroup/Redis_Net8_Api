using Dapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RedisNet8Api.Data;
using RedisNet8Api.DTOs.Shared;
using RedisNet8Api.DTOs.Student;
using RedisNet8Api.Services.Shared;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace RedisNet8Api.Services.Student
{
    public class StudentService : IStudentService
    {
        private readonly IApplicationContextDapper _context;
        private readonly ICacheService _cacheService;

        public StudentService(IApplicationContextDapper context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<ServiceResponse<List<GetStudentDto>>> GetAllStudentsAsync()
        {
            var serviceresponse = new ServiceResponse<List<GetStudentDto>>();
            var cacheData = await getCache();

            if (cacheData is not null)
            {
                var result = (IEnumerable<GetStudentDto>)cacheData;
                serviceresponse.data = result.ToList();
                return serviceresponse;
            }

            var query = "Select [Id], [Name], [Address] From Students";
            var students = await _context.GetDataAsync<GetStudentDto>(query);

            setCache(students);

            serviceresponse.data = students.ToList();
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetStudentDto>> GetStudentAsync(int id)
        {
            var serviceresponse = new ServiceResponse<GetStudentDto>();

            var findstudent = await findStudent(id);
            if (findstudent is not null)
            {
                serviceresponse.data = findstudent;
                return serviceresponse;
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Student not found";
                return serviceresponse;
            }
        }

        public async Task<ServiceResponse<GetStudentDto>> AddStudentAsync(AddStudentDto _student)
        {
            var serviceresponse = new ServiceResponse<GetStudentDto>();
            var query = "INSERT INTO Students ([Name], [Address]) OUTPUT inserted.Id VALUES (@Name, @Address)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", _student.Name, DbType.String);
            parameters.Add("Address", _student.Address, DbType.String);
            int Key = await _context.GetDataSingleAsync<int>(query, parameters);

            if (Key > 0)
            {
                GetStudentDto getStudentDto = new GetStudentDto { Name = _student.Name, Id = Key, Address = _student.Address };
                setCache(getStudentDto, Key);

                var students = await GetStudentAsync(Key);
                serviceresponse.data = students.data;
                return serviceresponse;
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Student not added";
                return serviceresponse;
            }
        }

        public async Task<ServiceResponse<GetStudentDto>> UpdateStudentAsync(UpdateStudentDto _student)
        {
            var serviceresponse = new ServiceResponse<GetStudentDto>();
            var findstudent = await findStudent(_student.Id);
            if (findstudent is null)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Student not found";
                return serviceresponse;
            }

            var query = "UPDATE Students SET [Name] = @Name, [Address] = @Address WHERE [Id] = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", _student.Id, DbType.Int64);
            parameters.Add("Name", _student.Name, DbType.String);
            parameters.Add("Address", _student.Address, DbType.String);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            if (result)
            {
                GetStudentDto getStudentDto = new GetStudentDto { Name = _student.Name, Id = _student.Id, Address = _student.Address };
                setCache(getStudentDto, _student.Id);
                var studentResult = await findStudent(_student.Id);
                serviceresponse.data = studentResult;
                return serviceresponse;
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Student not updated";
                return serviceresponse;
            }
        }

        public async Task<ServiceResponse<List<GetStudentDto>>> DeleteStudentAsync(int id)
        {
            var serviceresponse = new ServiceResponse<List<GetStudentDto>>();

            var findstudent = await findStudent(id);
            if (findstudent is null)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Student not found";
                return serviceresponse;
            }

            var query = "DELETE FROM Students WHERE [Id] = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int64);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            if (result)
            {
                await _cacheService.RemoveData($"student{id}");
                var students = await GetAllStudentsAsync();
                serviceresponse.data = students.data;
                return serviceresponse;
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Student not deleted";
                return serviceresponse;
            }
        }

        private async Task<GetStudentDto> findStudent(int id)
        {
            GetStudentDto? student = null;

            var cacheData = await getCache(id);
            if (cacheData is not null)
            {
                return (GetStudentDto)cacheData;
            }

            var query = "Select [Id], [Name], [Address] From Students Where [Id] = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int64);

            student = await _context.GetDataSingleAsync<GetStudentDto>(query, parameters);
            setCache(student, id);

            return student;
        }

        private async Task<object> getCache(int? key = null)
        {
            if (key is not null)
            {
                var cacheData = await _cacheService.GetData<GetStudentDto>($"student{key}");
                if (cacheData is not null)
                {
                    return cacheData;
                }
            }
            else
            {
                var cacheData = await _cacheService.GetData<IEnumerable<GetStudentDto>>($"students");
                if (cacheData is not null)
                {
                    return cacheData;
                }
            }
            return default;
        }

        private async void setCache(object data, int? key = null)
        {
            var expiryTime = DateTimeOffset.Now.AddSeconds(60);
            if (key is not null)
            {
                var student = (GetStudentDto)data;
                await _cacheService.SetData<GetStudentDto>($"student{student.Id}", student, expiryTime);
            }
            else
            {
                var student = (IEnumerable<GetStudentDto>)data;
                await _cacheService.SetData<IEnumerable<GetStudentDto>>($"students", student, expiryTime);
            }
        }
    }
}
