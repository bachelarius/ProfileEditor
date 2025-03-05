using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProfileEditor.Validation
{
    public class AllowedContentTypesAttribute : ValidationAttribute
    {
        private readonly string[] _contentTypes;

        public AllowedContentTypesAttribute(string[] contentTypes)
        {
            _contentTypes = contentTypes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (!_contentTypes.Contains(file.ContentType.ToLower()))
                {
                    return new ValidationResult($"This file type is not allowed. Allowed types: {string.Join(", ", _contentTypes)}");
                }
            }

            return ValidationResult.Success;
        }
    }
} 