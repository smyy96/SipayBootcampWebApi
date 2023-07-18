using FluentValidation;
using StudentsWebApi.Entity;

namespace StudentsWebApi.FluentValidation
{
    public class StudentValidator : AbstractValidator<Student>  
    {
        public StudentValidator()  // propertylerin kontrollerini yapıyoruz. Kurallarını belirliyoruz.
        {
            RuleFor(student => student.Name)
            .NotEmpty().WithMessage("Student name cannot be empty.")
            .Length(3, 50).WithMessage("Student name must be between 3 and 50 characters.");

            RuleFor(student => student.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(student => student.Phone)
                .NotEmpty().WithMessage("Phone number cannot be empty.");

            RuleFor(student => student.Teacher)
                .NotEmpty().WithMessage("Teacher name cannot be empty.")
                .Length(2, 50).WithMessage("Teacher name must be between 3 and 50 characters.");

            RuleFor(student => student.Classroom)
                .NotEmpty().WithMessage("Classroom cannot be empty.");

            RuleFor(student => student.Birthdate)
                .NotEmpty().WithMessage("Birthdate cannot be empty.")
                .Must(BeAValidDate).WithMessage("Please enter a valid birthdate.");
        }

        private bool BeAValidDate(DateTime date)
        {
            DateTime minDate = new DateTime(1990, 1, 1); // Geçerli tarihin belirlediğim tarihten sonra olması gerektiğini belirtiyoruz.
            return date >= minDate;
        }
    }
}
