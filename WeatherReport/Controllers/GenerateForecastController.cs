using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherReport.Models;
using System.Collections;

namespace WeatherReport.Controllers
{
    public class GenerateForecastController : Controller
    {
        private WeatherReportsEntities db = new WeatherReportsEntities();
        //fields declared that will be used to generate a user specified weather forecast
        //----------------------------------
        static string [] cities;
        static string startdate;
        static string enddate;
        //----------------------------------

        //ActionResult for FavIndex where a user only see there favourite cities and then also can generate weather forecast
        //--------------------------------------------------------------------------
        public ActionResult FavIndex()
        {
            //Used if the view is opened and a user is not loged in
            if (Session["User"] == null)
            {
                Session["loginck"] = "failed";
                return RedirectToAction("Login", "LogInUsers");
            }
            else
            {
                //query to get a user favourites
                var tt = db.favourite_Table.ToList().Where(x => x.Username.Equals(Session["User"]));
                //puts all favourite in viewbag
                ViewBag.City = new SelectList(tt, "City", "City");
                return View();
            }
        }
        //--------------------------------------------------------------------------

        //ActionResult [HttpPost] to generate a user specified weather forecast for their favourites
        //--------------------------------------------------------------------------
        [HttpPost]
        public ActionResult FavIndex(string [] City, string start_Date, string end_Date)
        {
            //Used if the view is opened and a user is not loged in
            if (Session["User"] == null)
            {
                Session["loginck"] = "failed";
                return RedirectToAction("Login", "LogInUsers");
            }
            else
            {
                var dd = db.Forecast_Table.ToList().Where(x => x.Date >= Convert.ToDateTime(start_Date.ToString())
                && x.Date <= Convert.ToDateTime(end_Date.ToString()));
                if (dd.Count() >= 1)
                {
                    //assigning user specified input to asign it to global variables
                    cities = City;
                    startdate = start_Date;
                    enddate = end_Date;

                    //go to display view
                    return RedirectToAction("Display", "GenerateForecast");
                }
                else
                {
                    //if username is not in database
                    ViewBag.Error = "There is no records for the date/dates selected";
                    //query to get a user favourites
                    var ff = db.favourite_Table.ToList().Where(x => x.Username.Equals(Session["User"]));
                    //puts all favourite in viewbag
                    ViewBag.City = new SelectList(ff, "City", "City");
                }
                return View();
            }
        }
        //--------------------------------------------------------------------------

        //ActionResult for FavIndex where a user see all cities in the database
        //--------------------------------------------------------------------------
        public ActionResult IndexAll()
        {
            //Used if the view is opened and a user is not loged in
            if (Session["User"] == null)
            {
                Session["loginck"] = "failed";
                return RedirectToAction("Login", "LogInUsers");
            }
            else
            {
                //puts all Cities in viewbag
                ViewBag.City = new SelectList(db.City_Table, "City", "City");
                return View();
            }
        }
        //--------------------------------------------------------------------------

        //ActionResult [HttpPost] to generate a user specified weather forecast for all cities
        //--------------------------------------------------------------------------
        [HttpPost]
        public ActionResult IndexAll(string [] City, string start_Date, string end_Date)
        {
            //Used if the view is opened and a user is not loged in
            if (Session["User"] == null)
            {
                Session["loginck"] = "failed";
                return RedirectToAction("Login", "LogInUsers");
            }
            else
            {
                var dd = db.Forecast_Table.ToList().Where(x => x.Date >= Convert.ToDateTime(start_Date.ToString())
                && x.Date <= Convert.ToDateTime(end_Date.ToString()));
                if (dd.Count() >= 1)
                {
                    //assigning user specified input to asign it to global variables
                    cities = City;
                    startdate = start_Date;
                    enddate = end_Date;

                    //go to display view
                    return RedirectToAction("Display", "GenerateForecast");
                }
                else
                {
                    //if username is not in database
                    ViewBag.Error = "There is no records for the date/dates selected";
                    //puts all Cities in viewbag
                    ViewBag.City = new SelectList(db.City_Table, "City", "City");
                }
                return View();
            }
        }
        //--------------------------------------------------------------------------

        //Will go to the view to display all the specified results the user requested
        //--------------------------------------------------------------------------
        public ActionResult Display()
        {
            //Used if the view is opened and a user is not loged in
            if (Session["User"] == null)//so the user cant skip between tabs
            {
                Session["loginck"] = "failed";
                return RedirectToAction("Login", "LogInUsers");//redirect to a succes page
            }
            else
            {
                //query to get all the specified results the user requested
                return View(db.Forecast_Table.ToList().
                Where(x => x.Date >= Convert.ToDateTime(startdate.ToString()) 
                && x.Date <= Convert.ToDateTime(enddate.ToString()) 
                && cities.Contains(x.City)));
            }
        }
        //--------------------------------------------------------------------------
    }
}