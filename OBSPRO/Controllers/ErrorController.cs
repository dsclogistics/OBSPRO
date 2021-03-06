﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBSPRO.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            Exception ex = new HttpUnhandledException();
            return View("Error", ex);
        }
        
        // GET: Error
        public ActionResult Index(string ErrorMsg)
        {
            Exception ex = new Exception(ErrorMsg);
            return View("Error", ex);
        }

        // GET: Error
        public ActionResult Message(string ErrorMsg)
        {
            ////Check first if the user is logged in       //Not needed-Layout Page is already checking this
            //try
            //{
            //    string sessionActive = Session["emp_id"].ToString();
            //}
            //catch
            //{
            //  return RedirectToAction("OBSLogout", "Login");
            //}

            Exception ex = new Exception(ErrorMsg);
            return View("Error", ex);
        }

        // GET: Error
        public ActionResult Index(Exception catchedException)
        {
            return View("Error", catchedException);
        }

        // GET: Error
        public ActionResult NotFound()
        {
            Exception ex = new Exception("Page not found or invalid URL Entry point used");
            return View("Error", ex);
        }

        // GET: Error
        public ActionResult BadRequest()
        {
            Exception ex = new Exception("Bad Request. Please verify your submission and try again.");
            return View("Error", ex);
        }

        // GET: Error
        public ActionResult NotAuthorized()
        {
            Exception ex = new UnauthorizedAccessException();
            return View("Error", ex);
        }

    }
}