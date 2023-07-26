using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using StudentsWebApi.Entity;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.VisualStudio.Services.Common;
using FluentValidation;
using Microsoft.VisualStudio.Services.Users;

namespace StudentsWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [FakeAuthorization]
    public class StudentController : ControllerBase
    {

        private readonly IValidator<Student> _validator;

        public StudentController(IValidator<Student> validator)
        {
            _validator = validator;
        }

        private static List<Student> students = new List<Student>()
        {
            new Student {Id=1, Name = "S�meyye Coskun", Classroom=10, Birthdate=new DateTime(1996, 05, 08), Email="smyy.cskn@hotmail.com", Phone="991521231214", Teacher="Fatma" },
            new Student {Id=2, Name = "Busra Is�k", Classroom=8, Birthdate=new DateTime(2000, 09, 08), Email="busraa@hotmail.com", Phone="78954252", Teacher="Afra" },
            new Student {Id=3, Name = "Kader Y�lmaz", Classroom=5, Birthdate = new DateTime(1999, 5, 15), Email="kaderrr@hotmail.com", Phone="99152147558", Teacher="Tu�ba" }
        };

        

        [HttpGet] //��rencileri listeleme
        public IActionResult Get()
        {
            return Ok(students);
        }


        [HttpGet("{id}")] //Id de�eri ile ��renci listeleme
        public IActionResult GetById(string id)
        {
            var student = students.Where(x => x.Id == int.Parse(id)).FirstOrDefault();
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpGet("orderbyvalue")] //Se�ti�imiz propertiye g�re s�ralama i�lemi yap�yor
        public ActionResult<List<Student>> GetStudents([FromQuery] string orderByProperty)
        {
            var filteredStudents = students.AsQueryable();

            switch (orderByProperty.ToLower())
            {
                case "name":
                    filteredStudents = filteredStudents.OrderBy(s => s.Name);
                    break;
                case "classroom":
                    filteredStudents = filteredStudents.OrderBy(s => s.Classroom);
                    break;
                case "email":
                    filteredStudents = filteredStudents.OrderBy(s => s.Email);
                    break;
                case "phone":
                    filteredStudents = filteredStudents.OrderBy(s => s.Phone);
                    break;
                case "teacher":
                    filteredStudents = filteredStudents.OrderBy(s => s.Phone);
                    break;
                default:
                    return BadRequest("Sorting is done only on \"name, phone, teacher, email and classroom\" values.");
            }

            return Ok(filteredStudents.ToList());
        }


        [HttpPost] // �grenci ekleme islemi yap�yor. Burada model binding i�lemi olarak frombody kulland�m.
        public IActionResult AddStudent([FromBody] Student student)
        {
            if (student == null) return BadRequest();

            var validationResult = _validator.Validate(student);  //girilen propertyleri kontrol ediyoruz.

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            // ��renci modeli ge�erliyse, ekleme i�lemini yap�n
            students.Add(student);

            return Ok("Student added.");
        }


        [HttpDelete] // ��renci silme i�lemi yap�yor. Burada model binding i�lemi olarak fromquery kulland�m.
        public IActionResult DeleteStudent([FromQuery] int id)
        {
            var deletestudent = students.Find(x => x.Id == id);

            if (deletestudent == null)
                return NotFound();

            students.Remove(deletestudent);
            return Ok();
        }


        [HttpPut("{id}")] // ��renci g�ncelleme i�lemi yap�yor. Burada model binding i�lemi olarak frombody ve fromroute kulland�m.
        public IActionResult UpdateStudent(int id, [FromBody] Student updatestudent)
        {
            var student = students.Find(x => x.Id == id);
            if (student == null) return NotFound();

            var validationResult = _validator.Validate(updatestudent);   //girilen propertyleri kontrol ediyoruz.

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }


            students.Remove(student);
            students.Add(updatestudent);
            return Ok();
        }



        [HttpPatch("students/{id}")] // Patch ile id de�erini girdi�imiz kullan�c�n�n sadece de�i�tirmek istedi�imiz de�erini(name, phone, email...) de�i�tiriyoruz.
        public ActionResult<Student> UpdateStudent(int id, [FromBody] object patchData)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            var patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<Student>>(patchData.ToString());
            patchDocument.ApplyTo(student);

            return student;
        }


        /*  [
              {
                "op": "replace",
                "path": "/Phone", //de�i�tirmek istedi�imiz properti
                "value": "123456789" // ve de�i�tirmek istedi�imiz de�eri bu �ekilde g�ncelleyebiliyoruz, patch' execute ettikten sonra
              }
            ]       */


    }
}