
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
 
namespace TWP_API_Payroll.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseCoreAppCustomModelValidation(this IServiceCollection services)
        {
            _ = services.Configure((Action<ApiBehaviorOptions>)(apiBehaviorOptions =>
                    apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var message = GetModelStateErrorWithException(actionContext);
 
                        return new BadRequestObjectResult(new
                        {
                            statusCode = 400,
                            data = "Null",
                            message = "Bad Request",
                            error = message
                        });
                    }));
            return services;
        }
 
        private static ExpandoObject GetModelStateFriendlyError(ActionContext actionContext)
        {
            var modelType = actionContext.ActionDescriptor.Parameters.FirstOrDefault()?.ParameterType; //Get model type  
                //p => p.BindingInfo.BindingSource.Id.Equals("Body", StringComparison.InvariantCultureIgnoreCase)
 
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<string, object>>)expandoObj; //Cannot convert IEnumrable to ExpandoObject  
 
            var dictionary = actionContext.ModelState.ToDictionary(k => k.Key, v => v.Value)
                .Where(v => v.Value.ValidationState == ModelValidationState.Invalid)
                .ToDictionary(
                k =>
                {
                    if (modelType != null)
                    {
                        var property = modelType.GetProperties().FirstOrDefault(p => p.Name.Equals(k.Key, StringComparison.InvariantCultureIgnoreCase));
                        if (property != null)
                        {
                            //Try to get the attribute  
                            var displayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault()?.DisplayName;
                            return displayName ?? property.Name;
                        }
                    }
                    return k.Key; //Nothing found, return original validation key  
                },
                v => v.Value.Errors.Select(e => e.ErrorMessage).ToList() as object); //Box String collection
 
            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
 
            return expandoObj;
        }
 
        private static List<string> GetModelStateErrorWithException(ActionContext actionContext)
        {
            List<string> errorMessages = new List<string>();
            foreach (var error in actionContext.ModelState)
            {
                string message = error.Value.Errors != null && error.Value.Errors.Count() > 0 ?
                                    error.Value.Errors[0].Exception != null ?
                                            error.Key + " : " + error.Value.Errors[0].Exception.Message   //More weight-age is given to exception, so in case of one, use the exception message
                                    : error.Value.Errors[0].ErrorMessage != null ?
                                            error.Key + " : " + error.Value.Errors[0].ErrorMessage //Otherwise use error message
                                    : string.Empty
                            : string.Empty;
 
                if (!string.IsNullOrWhiteSpace(message))
                {
                    errorMessages.Add(message);
                }
            }
            return errorMessages;
        }
 
        private static List<string> GetModelStateError(ActionContext actionContext)
        {
            return actionContext.ModelState.Values.SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage).ToList();
        }
    }
}