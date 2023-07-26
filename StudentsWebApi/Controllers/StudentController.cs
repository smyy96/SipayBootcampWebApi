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
            new Student {Id=1, Name = "Sümeyye Coskun", Classroom=10, Birthdate=new DateTime(1996, 05, 08), Email="smyy.cskn@hotmail.com", Phone="991521231214", Teacher="Fatma" },
            new Student {Id=2, Name = "Busra Isýk", Classroom=8, Birthdate=new DateTime(2000, 09, 08), Email="busraa@hotmail.com", Phone="78954252", Teacher="Afra" },
            new Student {Id=3, Name = "Kader Yýlmaz", Classroom=5, Birthdate = new DateTime(1999, 5, 15), Email="kaderrr@hotmail.com", Phone="99152147558", Teacher="Tuðba" }
        };

        

        [HttpGet] //Öðrencileri listeleme
        public IActionResult Get()
        {
            return Ok(students);
        }


        [HttpGet("{id}")] //Id deðeri ile öðrenci listeleme
        public IActionResult GetById(string id)
        {
            var student = students.Where(x => x.Id == int.Parse(id)).FirstOrDefault();
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpGet("orderbyvalue")] //Seçtiðimiz propertiye göre sýralama iþlemi yapýyor
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


        [HttpPost] // Ögrenci ekleme islemi yapýyor. Burada model binding iþlemi olarak frombody kullandým.
        public IActionResult AddStudent([FromBody] Student student)
        {
            if (student == null) return BadRequest();

            var validationResult = _validator.Validate(student);  //girilen propertyleri kontrol ediyoruz.

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            // Öðrenci modeli geçerliyse, ekleme iþlemini yapýn
            students.Add(student);

            return Ok("Student added.");
        }


        [HttpDelete] // Öðrenci silme iþlemi yapýyor. Burada model binding iþlemi olarak fromquery kullandým.
        public IActionResult DeleteStudent([FromQuery] int id)
        {
            var deletestudent = students.Find(x => x.Id == id);

            if (deletestudent == null)
                return NotFound();

            students.Remove(deletestudent);
            return Ok();
        }


        [HttpPut("{id}")] // Öðrenci güncelleme iþlemi yapýyor. Burada model binding iþlemi olarak frombody ve fromroute kullandým.
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



        [HttpPatch("students/{id}")] // Patch ile id deðerini girdiðimiz kullanýcýnýn sadece deðiþtirmek istediðimiz deðerini(name, phone, email...) deðiþtiriyoruz.
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
                "path": "/Phone", //deðiþtirmek istediðimiz properti
                "value": "123456789" // ve deðiþtirmek istediðimiz deðeri bu þekilde güncelleyebiliyoruz, patch' execute ettikten sonra
              }
            ]       */


    }
}