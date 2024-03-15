using System.ComponentModel.DataAnnotations;

namespace RedisNet8Api.DTOs.Student
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Student id is required")]
        public required int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        public required string Name { get; set; }

        public string? Address { get; set; }
    }
}
