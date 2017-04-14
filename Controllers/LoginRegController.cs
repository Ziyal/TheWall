using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;
using Microsoft.AspNetCore.Http;

namespace TheWall.Controllers
{
    public class LoginRegController : Controller
    {
        private readonly DbConnector _dbConnector;
 
        public LoginRegController(DbConnector connect)    {
            _dbConnector = connect;
        }

        [HttpGet]
        [Route("")]
        public IActionResult LoginReg()
        {
            ViewBag.Errors = TempData["Errors"];
            return View("LoginReg");
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterUser model)  {
            List<string> allErrors = new List <string>();

            if(ModelState.IsValid) {

                // string UserQuery = ("SELECT * FROM users WHERE Email = '"+model.email+"'");
                // var CheckForUser = _dbConnector.Query(UserQuery);
                // if (CheckForUser != null) {
                //     allErrors.Add("Email already in use");
                //     TempData["Errors"] = allErrors;
                //     return RedirectToAction("LoginReg");
                // }
                // else {
                string query = $"INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES ('"+model.first_name+"', '"+model.last_name+"', '"+model.email+"', '"+model.password+"', NOW(), NOW())";
                _dbConnector.Execute(query);

                // Grab user id
                string IdQuery = ("SELECT id FROM users WHERE Email = '"+model.email+"'");
                var GrabId = _dbConnector.Query(IdQuery);
                HttpContext.Session.SetInt32("id", (int)GrabId[0]["id"]);
                return RedirectToAction("Success");
                // }
            }
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("LoginReg");

        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUser model)  {
            List<string> allErrors = new List <string>();

            if(ModelState.IsValid) {
                string UserQuery = ("SELECT * FROM users WHERE Email = '"+model.Email+"'");
                var CheckForUser = _dbConnector.Query(UserQuery);
                // Check if user exists
                if (CheckForUser != null) {

                    string PasswordQuery = ("SELECT Password FROM users WHERE Email = '"+model.Email+"'");
                    var CheckPassword = _dbConnector.Query(PasswordQuery);

                    // Check for correct password
                    if (model.Password == (string)CheckPassword[0]["Password"]) {
                        // Grab user id
                        string IdQuery = ("SELECT id FROM users WHERE Email = '"+model.Email+"'");
                        var GrabId = _dbConnector.Query(UserQuery);
                        HttpContext.Session.SetInt32("id", (int)GrabId[0]["id"]);
                        return RedirectToAction("Success");
                    }
                    else {
                        allErrors.Add("Incorrect password");
                        TempData["Errors"] = allErrors;
                    }
                }
            }
            // ViewBag.Errors = ModelState.Values;

            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("LoginReg");
        }

        [HttpGet]
        [Route("Success")]
        public IActionResult Success()
        {
            System.Console.WriteLine("SUCCESS");
            return RedirectToAction("Wall", "Wall");
        }
    }
}
