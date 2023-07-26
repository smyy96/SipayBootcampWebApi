using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsWebApi.Entity
{
    public class Student
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Teacher { get; set; }
        public int Classroom { get; set; }
        public DateTime Birthdate { get; set; }
    }
}