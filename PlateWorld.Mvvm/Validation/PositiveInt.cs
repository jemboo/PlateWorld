using System;
using System.Globalization;
using System.Windows.Controls;

namespace PlateWorld.Mvvm.Validation
{
    public class PositiveInt : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var price = Convert.ToDouble(value);

                if (price < 0)
                {
                    return new ValidationResult(false, "Must be a positive integer.");
                }
                return ValidationResult.ValidResult;
            }
            catch (Exception)
            {
                // Exception thrown by Conversion - value is not a number.
                return new ValidationResult(false, "Must be an integer.");
            }
        }
    }
}