using System;
using System.Globalization;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public class IsRequiredField : ValidationRule
    {
        public int MinCount { get; set; } = 0;
        public string ErrorMessage { get; set; } = "Need fill";

        public IsRequiredField()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = "";

            try
            {
                content = (String)value;
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if (content == null || content.Trim().Length <= MinCount)
            {
                return new ValidationResult(false, ErrorMessage);
            }
            return ValidationResult.ValidResult;
        }
    }
}
