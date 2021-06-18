using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using cis2055_NemesysProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<NemesysUser> _signInManager;
        private readonly UserManager<NemesysUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<NemesysUser> userManager,
            RoleManager<IdentityRole> roleManager,
        SignInManager<NemesysUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public IEnumerable<Role> listroles { get; set; }
        public string rolename { get; set; }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Nickname")]
            public string AuthorAlias { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

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
            public string RoleName { get; set; }


        }

        public class Role
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Roles = _roleManager.Roles.Where(r => r.Name != "Admin").ToList();
            
            List<Role> list = new List<Role>();
            foreach (var role in Roles)
            {
                list.Add(new Role() {Value = role.Name, Text = role.Name });
            }
            listroles = list;

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync( string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (Input.RoleName != "None")
            {
                if (ModelState.IsValid)
                {
                    var user = new NemesysUser
                    {
                        UserName = Input.Email,
                        AuthorAlias = Input.AuthorAlias,
                        PhoneNumber = Input.PhoneNumber,
                        Email = Input.Email
                    };



                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(user, Input.RoleName);
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            } else
            {
                ModelState.AddModelError(string.Empty, "Kindly Select a proper role");
            }
            // If we got this far, something failed, redisplay form
            Roles = _roleManager.Roles.Where(r => r.Name != "Admin").ToList();

            List<Role> list = new List<Role>();
            foreach (var role in Roles)
            {
                list.Add(new Role() { Value = role.Name, Text = role.Name });
            }
            listroles = list;
            return Page();
        }
    }
}
