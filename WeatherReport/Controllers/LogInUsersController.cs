using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherReport.Models;

namespace WeatherReport.Controllers
{
    public class LogInUsersController : Controller
    {
        private WeatherReportsEntities db = new WeatherReportsEntities();

        // GET: user
        //-------------------------------------------------------------------------
        public ActionResult Index()
        {
            return View();
        }
        //-------------------------------------------------------------------------

        //ActionResult that will be used for the log in of a user
        //-------------------------------------------------------------------------
        public ActionResult Login()
        {
            //Used if the view is opened and a user is not loged in
            if (Session["loginck"] as string == "failed")
            {
                ViewBag.Error = "Login First please";
            }

            return View();
        }
        //-------------------------------------------------------------------------

        //ActionResult (HttpPost) that will be used for the log in of a user to check if the user is in the database
        //-------------------------------------------------------------------------
        [HttpPost]
        public ActionResult login(string tbxUsername, string tbxPassword)
        {
            bool found = false;
            //process the login
            foreach (User_Table c in db.User_Table)//user is the table model  
            {
                //Gets username and password and assign it to variables
                string dd = tbxPassword.GetHashCode().ToString();
                string ss = dd;
                //see if the username is in the database
                if (c.Username.Equals(tbxUsername) && c.password.Equals(tbxPassword.GetHashCode().ToString()))
                {
                    //makes a seesion if the user is in the database
                    Session["User"] = c.Username;// create session varible 
                    found = true;
                    //go strait to the users favourite page so they can view their forecast
                    return RedirectToAction("FavIndex", "GenerateForecast");//redirect to a succes page
                }
            }
            //if username is not in database
            if (found == false)
            {
                //if username is Incorrect
                ViewBag.Error = "Username or Password is Incorrect";
            }
            return View();
        }
        //-------------------------------------------------------------------------

        //Sign out of the application
        //-------------------------------------------------------------------------
        public ActionResult SignOut()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------------------------
    }
}