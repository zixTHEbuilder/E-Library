using E_Library.Dtos;
using FluentValidation;

namespace E_Library.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can't be empty")
                .Length(3, 25).WithMessage("Username must be between 3 to 25 characters")
                .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Username can only contain letters and numbers");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .Length(6, 40).WithMessage("Password must be between 6 to 40 characters")
                .Matches(@"[0-9]+").WithMessage("Password must contain atleast 1 number");
        }
    }
}
