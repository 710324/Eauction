using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace EAuction.Processor.Helpers
{
    public static class ValidatorExtension
    {
        public static IEnumerable<string> ValidationErrorExtract(ValidationResult validationResult)
        {
            var result = validationResult.Errors
                .Select(x => string.Format($"{x.PropertyName} : { x.ErrorMessage}"))
                .ToList();
            return result;
        }
    }
}