using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages
{
    [Authorize]
    public class AccountModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}