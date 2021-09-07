using System.Collections.Generic;
using FluentValidation.Results;

namespace Todo
{
    public static class ValidationResponse
    {
        public static Dictionary<string, string> CreateFieldsErrors(List<ValidationFailure> errors)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var error in errors)
            {
                var camelCasedPropertyName = char.ToLower(error.PropertyName[0]) + error.PropertyName.Substring(1);
                dictionary.Add(camelCasedPropertyName, error.ErrorMessage);
            }

            return dictionary;
        }

        public static object CreateGenericError(string error) => new { _error = error };
    }
}