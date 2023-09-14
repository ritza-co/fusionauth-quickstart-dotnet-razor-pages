using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages
{
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
            Nickels = Convert.ToInt32(Math.Floor(Amount / 0.05));
            Pennies = Convert.ToInt32(Math.Round((Amount - 0.05 * Nickels) / 0.01));

            ChangeBreakdown = String.Format("We can make change for ${0:#.00} with {1} nickels and {2} pennies!", Amount,  Nickels, Pennies);
        }
    }
}
