using FluentValidation;
using TaskManagementApi.Features.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.CreateTaskDto.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.CreateTaskDto.Description)
            .MaximumLength(1000)
            .When(x => x.CreateTaskDto.Description is not null);
    }
}