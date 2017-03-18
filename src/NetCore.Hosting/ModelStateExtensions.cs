using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace NetCore.Contracts
{
    public static class ModelStateExtensions
    {
        public static List<ModelValidationResult> AsValidationResult(this ModelStateDictionary modelState)
        {
            List<ModelValidationResult> validations = new List<ModelValidationResult>();
            foreach (KeyValuePair<string, ModelStateEntry> item in modelState)
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    if (item.Value.Errors.Any(x => x.Exception != null))
                    {
                        var exceptionModelState = item.Value.Errors.FirstOrDefault(x => x.Exception != null);
                        validations.Add(new ModelValidationResult { Name = item.Key, Messages = new List<string> { exceptionModelState.Exception.Message } });
                        continue;
                    }
                    validations.Add(new ModelValidationResult { Name = item.Key, Messages = item.Value.Errors.Select(x => x.ErrorMessage).ToList() });
                }
            }

            return validations;
        }
    }
}
