using System.ComponentModel.DataAnnotations;

namespace RedisNet8Api.DTOs.Student
{
    public class AddStudentDto
    {
        [Required(ErrorMessage = "Student name is required")]
        public required string Name { get; set; }

        public string? Address { get; set; }
    }
}
