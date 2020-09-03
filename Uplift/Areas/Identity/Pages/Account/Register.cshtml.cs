using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid) return Page();

            var user = new ApplicationUser
            {
                Email = Input.Email,
                UserName = Input.Email,
                Name = Input.Name,
                City = Input.City,
                StreetAddress = Input.StreetAddress,
                State = Input.State,
                PhoneNumber = Input.PhoneNumber,
                PostalCode = Input.PostalCode


            };
            var result = await _userManager.CreateAsync(user, Input.Password);


            if (result.Succeeded)
            {
                await CreateRoles();

                _logger.LogInformation("User created a new account with password.");

                var role = HttpContext.Request.Form["rdUserRole"].ToString();

                await AssignRoleToRegisteredUser(role, user);

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                }
                else
                {
                    return LocalRedirect(returnUrl);
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();

        }

        private async Task AssignRoleToRegisteredUser(string role, IdentityUser user)
        {
            if (role == AppConstants.Admin)
            {
                await _userManager.AddToRoleAsync(user, AppConstants.Admin);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, AppConstants.Manager);
            }
        }

        private async Task CreateRoles()
        {
            if (!await _roleManager.RoleExistsAsync(AppConstants.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppConstants.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(AppConstants.Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppConstants.Manager));
            }
        }
    }
}
