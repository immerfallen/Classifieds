using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Classifieds.Web.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            //real database call to check if username and password are correct goes here!

            if(Input.Email == "admin@test.com" && Input.Password == "maro")
            {
                var claims = new List<Claim>
                {
                    //ver se a insercao das claims da api vai ser feita no nameidentifier
                    new Claim(ClaimTypes.NameIdentifier, Input.Email),
                    new Claim(ClaimTypes.Name, "USERNAME HERE"),
                    new Claim(ClaimTypes.Role, "ROLE"),
                    new Claim("RandomDataPoint", "RandomValue")
                };

                var identityUser = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identityUser);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = Input.RememberMe });

                return LocalRedirect(returnUrl);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
