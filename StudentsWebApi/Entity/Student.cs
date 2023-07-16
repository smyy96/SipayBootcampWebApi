
using System.ComponentModel.DataAnnotations;

namespace StudentsWebApi.Entity
{
    public class Student
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Teacher { get; set; }

        [Required(ErrorMessage = "Classroom is required.")]
        public int Classroom { get; set; }
        public DateTime Birthdate { get; set; }
    }
}