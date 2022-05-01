using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Filters
{
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LogActionFilterAttribute> _logger;
        public LogActionFilterAttribute(ILogger<LogActionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var data = context.RouteData;
            var cont = data.Values["controller"];
            var action = data.Values["action"];
            _logger.LogInformation($"Executing {action} action of {cont} controller");
        }
    }
}
