using System;
using FluentValidation.Validators;

namespace EventHandlers.Validator
{
    public static class EnumValidator
    {
        public static void Validate<T>(int? value, CustomContext context)
            where T : struct
        {
            if (value.HasValue)
            {
                Validate<T>(value.Value, context);
            }
        }
        public static void Validate<T>(int value, CustomContext context)
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                context.AddFailure($"{value} is not a valid value for {typeof(T).Name}");
            }
        }


    }
}
