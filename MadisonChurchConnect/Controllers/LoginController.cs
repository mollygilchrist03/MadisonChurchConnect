/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.BusinessLogic;
using System.Security.Claims;

namespace MadisonChurchConnect.Controllers
{
    public class LoginController : Controller
    {
        // class level variables
        private readonly UserLogic _userLogic;
        private readonly ILogger<LoginController> _logger;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public LoginController(UserLogic userLogic, ILogger<LoginController> logger)
        {
            _userLogic = userLogic;
            _logger = logger;
        }

        /// <summary>
        /// get method to load the login page
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserViewModel());
        }

        /// <summary>
        /// post method to process the login form
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserViewModel user)
        {
            // declare variables
            bool isValidated = false;
            UserViewModel? validatedUser;
            List<Claim> claims = new List<Claim>();
            ClaimsIdentity claimsIdentity;
            ClaimsPrincipal claimsPrincipal;
            Stopwatch totalStopwatch = Stopwatch.StartNew();
            Stopwatch credentialValidationStopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Login attempt started for username '{Username}'.", user.Username);

            // validate the user's credentials
            (isValidated, validatedUser) = _userLogic.ValidateUserCredentials(user.Username, user.PasswordHash);
            credentialValidationStopwatch.Stop();

            _logger.LogInformation(
                "Login credential validation finished for username '{Username}' in {ElapsedMilliseconds} ms. Success: {IsValidated}",
                user.Username,
                credentialValidationStopwatch.ElapsedMilliseconds,
                isValidated);

            // if credentials are valid, sign the user in
            if (isValidated && validatedUser != null)
            {
                // add claims for the user's id and username
                claims.Add(new Claim(ClaimTypes.NameIdentifier, validatedUser.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, validatedUser.Username));

                // create a claims identity and principal
                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // sign the user in and store the claims principal in a cookie
                Stopwatch signInStopwatch = Stopwatch.StartNew();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                signInStopwatch.Stop();
                totalStopwatch.Stop();

                _logger.LogInformation(
                    "Login cookie sign-in finished for username '{Username}' in {SignInElapsedMilliseconds} ms. Total login time: {TotalElapsedMilliseconds} ms.",
                    validatedUser.Username,
                    signInStopwatch.ElapsedMilliseconds,
                    totalStopwatch.ElapsedMilliseconds);

                // redirect to the sermons page after login
                return RedirectToAction("Index", "Sermons");
            }

            totalStopwatch.Stop();
            _logger.LogWarning(
                "Login attempt failed for username '{Username}' after {TotalElapsedMilliseconds} ms.",
                user.Username,
                totalStopwatch.ElapsedMilliseconds);

            // add an error message and reload the login page
            ModelState.AddModelError("", "Your username or password was incorrect. Please try again.");
            return View(user);
        }

        /// <summary>
        /// get method to log the user out when the logout route is requested directly
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return await LogoutCurrentUserAsync();
        }

        /// <summary>
        /// post method to log the user out
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            return await LogoutCurrentUserAsync();
        }

        /// <summary>
        /// signs the current user out and redirects to the menu page
        /// </summary>
        private async Task<IActionResult> LogoutCurrentUserAsync()
        {
            // sign the user out and clear the cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // redirect to the menu page
            return RedirectToAction("Index", "Menu");
        }
    }
}
