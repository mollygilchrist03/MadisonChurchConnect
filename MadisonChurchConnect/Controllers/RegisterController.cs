/*
 * Molly Gilchrist
 * 3/17/2026
 * STG-456
 * Capstone Project
 */

using Microsoft.AspNetCore.Mvc;
using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.BusinessLogic;

namespace MadisonChurchConnect.Controllers
{
    public class RegisterController : Controller
    {
        // class level variables
        private UserLogic _userLogic;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public RegisterController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        /// <summary>
        /// get method to load the registration page
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserViewModel());
        }

        /// <summary>
        /// post method to process the registration form
        /// </summary>
        [HttpPost]
        public IActionResult Register(UserViewModel user)
        {
            // return the form if the model state is invalid
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // attempt to add the user
            if (_userLogic.AddUser(user) != -1)
            {
                // redirect to login on success so they can sign in right away
                return RedirectToAction("Login", "Login");
            }

            // add an error message if registration failed and reload the form
            ModelState.AddModelError("", "Registration failed. Your username or email may already be in use.");
            return View(user);
        }
    }
}