/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

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
        private UserLogic _userLogic;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public LoginController(UserLogic userLogic)
        {
            _userLogic = userLogic;
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
        public IActionResult Login(UserViewModel user)
        {
            // declare variables
            bool isValidated = false;
            UserViewModel? validatedUser;
            List<Claim> claims = new List<Claim>();
            ClaimsIdentity claimsIdentity;
            ClaimsPrincipal claimsPrincipal;

            // validate the user's credentials
            (isValidated, validatedUser) = _userLogic.ValidateUserCredentials(user.Username, user.PasswordHash);

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
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).Wait();

                // redirect to the sermons page after login
                return RedirectToAction("Index", "Sermons");
            }

            // add an error message and reload the login page
            ModelState.AddModelError("", "Your username or password was incorrect. Please try again.");
            return View(user);
        }

        /// <summary>
        /// post method to log the user out
        /// </summary>
        [HttpPost]
        public IActionResult Logout()
        {
            // sign the user out and clear the cookie
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            // redirect to the login page
            return RedirectToAction("Login");
        }
    }
}