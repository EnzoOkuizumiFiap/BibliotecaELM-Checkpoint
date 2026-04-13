using System.ComponentModel.DataAnnotations;

namespace BibliotecaELM.Application.Validation;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class DateOnlyRangeAttribute : ValidationAttribute
{
    private readonly DateOnly _min;
    private readonly DateOnly _max;

    public DateOnlyRangeAttribute(string min, string max)
    {
        _min = DateOnly.Parse(min);
        _max = DateOnly.Parse(max);
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        if (value is not DateOnly date)
            return false;

        return date >= _min && date <= _max;
    }
}
