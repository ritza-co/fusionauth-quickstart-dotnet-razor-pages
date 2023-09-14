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

        
        public void OnGet()
        {
        }

        public void OnPost()
        {
            var nickels = Math.Floor(Amount / 0.05);
            var pennies = Math.Round((Amount - 0.05 * nickels) / 0.01);

            ChangeBreakdown = String.Format("Your change is {0} nickels and {1} pennies", nickels, pennies);
        }
    }
}
