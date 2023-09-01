using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        private readonly IConfiguration _configuration;

        public LogoutModel(ILogger<LogoutModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            SignOut("cookie", "FusionAuth");
            var host = _configuration["FusionAuth:Issuer"];
            var cookieName = _configuration["FusionAuth:CookieName"];

            var clientId = _configuration["FusionAuth:ClientId"];
            var url = host + "/oauth2/logout?client_id=" + clientId;
            Response.Cookies.Delete(cookieName);
            return Redirect(url);
        }
    }
}
