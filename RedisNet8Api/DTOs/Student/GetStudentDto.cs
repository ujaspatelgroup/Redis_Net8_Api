using System.ComponentModel.DataAnnotations;

namespace RedisNet8Api.DTOs.Student
{
    public class GetStudentDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }
    }
}