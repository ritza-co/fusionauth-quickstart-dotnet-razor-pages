using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages
{
    [Authorize]
	public class MakeChangeModel : PageModel
    {
        public string ChangeBreakdown { get; set; } = "";

        public string Error { get; set; } = "";

        [BindProperty]
        public double Amount { get; set; } = 0.0d;


        public int Nickels { get; set; } = 0;
        public int Pennies { get; set; } = 0; 
        
        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (Amount <= 0)
            {
                Error = "Please enter an amount greater than $0";
            }
            else
            {
                Nickels = Convert.ToInt32(Math.Floor(Amount / 0.05));
                Pennies = Convert.ToInt32(Math.Round((Amount - 0.05 * Nickels) / 0.01));

                ChangeBreakdown = String.Format("We can make change for ${0:0.00} with {1} nickels and {2} pennies!", Amount, Nickels, Pennies);
            }
        }
    }
}
