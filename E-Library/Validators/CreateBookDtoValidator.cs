using FluentValidation;
using E_Library.Dtos;

namespace E_Library.Validators
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author name can't be empty")
                .Length(3, 25).WithMessage("Author name must be between 3 to 25 characters");

            RuleFor(x => x.BookName)
                .NotEmpty().WithMessage("Book name can't be empty")
                .Length(3, 30).WithMessage("Bookname must be between 3 to 30 characters");

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Price of the book must be set")
                .LessThanOrEqualTo(10000).WithMessage("Price of the book can't be greater than 10,000");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Book can't be empty, please enter the contents of the book");
        }
    }
}
