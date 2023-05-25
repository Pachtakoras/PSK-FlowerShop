using Microsoft.AspNetCore.Mvc.Filters;

namespace FlowerShop.Logging
{
    public class LogMethod : IActionFilter
    {
        private readonly ILogger<LogMethod> _logger;
        public LogMethod(ILogger<LogMethod> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Completed: Page opened: {0}", context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Start: Opening page: {0}", context.ActionDescriptor.DisplayName);
        }
    }
}
