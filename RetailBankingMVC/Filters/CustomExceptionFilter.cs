using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RetailBankingMVC.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<LogActionFilterAttribute> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        public CustomExceptionFilter(ILogger<LogActionFilterAttribute> logger, IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
        }
        public void OnException(ExceptionContext context)
        {
            var result = new ViewResult { ViewName = "Error" };
            result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
            result.ViewData.Add("ErrorMessage", context.Exception.Message);
            _logger.LogError("Error: " + context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = result;
        }
    }
}
