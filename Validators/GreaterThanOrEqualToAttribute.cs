using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IEIPaperSearch.Pages
{
    internal class GreaterThanOrEqualToAttribute : ValidationAttribute
    {
        readonly string testedPropertyName;

        public GreaterThanOrEqualToAttribute(string testedPropertyName)
        {
            this.testedPropertyName = testedPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (testedPropertyName is null)
            {
                return new ValidationResult(string.Format("Unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo!.GetValue(validationContext.ObjectInstance, null);
            if (value is null || propertyTestedValue is null)
            {
                return ValidationResult.Success;
            }

            if (!IsComparableWithProperty(propertyTestedInfo, value.GetType()))
            {
                return new ValidationResult(string.Format("Property {0} is not comparable with this property.", this.testedPropertyName));
            }

            if ((dynamic)value >= (dynamic)propertyTestedValue)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        bool IsComparableWithProperty(PropertyInfo? propertyInfo, params Type[] typeArguments)
        {
            var targetType = propertyInfo?.PropertyType;
            if (targetType is null)
            {
                return false;
            }

            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                targetType = targetType.GenericTypeArguments[0];
            }
            return typeof(IComparable<>).MakeGenericType(typeArguments).IsAssignableFrom(targetType);
        }
    }
}