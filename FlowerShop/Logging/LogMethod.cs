using FlowerShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FlowerShop.Logging
{
    public class LogMethod : IActionFilter, IExceptionFilter
    {
        private readonly ILogger<LogMethod> _logger;
        private readonly IServiceProvider _serviceProvider;

        public LogMethod(ILogger<LogMethod> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Completed: Action completed: {0}, User: {1}, Role: {2}",
                                   context.ActionDescriptor.DisplayName,
                                   context.HttpContext.User.Identity.Name,
                                   await GetUserRole(context.HttpContext.User));
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Start: Action started: {0}, User: {1}, Role: {2}",
                                   context.ActionDescriptor.DisplayName,
                                   context.HttpContext.User.Identity.Name,
                                   await GetUserRole(context.HttpContext.User));
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("An exception occurred: {0}", context.Exception.ToString());
        }

        private async Task<string> GetUserRole(ClaimsPrincipal userClaim)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var appUser = await userManager.GetUserAsync(userClaim);
                if (appUser != null)
                {
                    var userRoles = await userManager.GetRolesAsync(appUser);
                    return userRoles.FirstOrDefault() ?? "Unknown";
                }
                return "Not registered";
            }
        }
    }
}
