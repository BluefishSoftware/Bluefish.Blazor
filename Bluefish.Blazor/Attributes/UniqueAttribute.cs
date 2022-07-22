namespace Bluefish.Blazor.Attributes;

public class UniqueAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (context.ObjectInstance is IUniqueConstraint uc)
        {
            if (uc.ExistingValues.Contains(uc.UniqueValue))
            {
                return new ValidationResult(ErrorMessage, new[] { context.MemberName });
            }
        }
        return ValidationResult.Success;
    }
}