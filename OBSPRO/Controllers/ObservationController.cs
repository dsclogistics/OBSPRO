﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OBSPRO.Models;
using OBSPRO.App_Code;
using Newtonsoft.Json.Linq;
using PagedList;
using PagedList.Mvc;

namespace OBSPRO.Controllers

{
    public class ObservationController : Controller
    {
        APIDataParcer apiParcer = new APIDataParcer();
        User usr = new User();

        [HttpGet]
        public ActionResult Index( string searchString, string sortBy, FormCollection form_data, int? page, int? PageSize)
        {            
            usr.setUser();
            ViewBag.CurrentItemsPerPage = PageSize ?? 15;
            string frmStatus = null;
            try
            {
                frmStatus = Request.QueryString["frmStatus"];
            }
            catch { }
            bool searchAll = (searchString == null && sortBy == null && frmStatus ==null) ? true : false;
            frmStatus = searchAll ? "STARTED,READY FOR REVIEW,COMPLETED" : frmStatus;
            frmStatus = frmStatus == null ? "" : frmStatus;
            ViewBag.searchText = searchString;
            ViewBag.sortStartDateParameter = String.IsNullOrEmpty(sortBy) ? "StartDate" : "";
            ViewBag.sortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.sortEmpObservedParameter = sortBy == "Observed Emplpoyee" ? "Observed Emplpoyee desc" : "Observed Emplpoyee";
            ViewBag.sortObserverParameter = sortBy == "Observer" ? "Observer desc" : "Observer";
            ViewBag.sortADPParameter = sortBy == "ADP ID" ? "ADP ID desc" : "ADP ID";
            ViewBag.sortStatusParameter = sortBy == "Status" ? "Status desc" : "Status";
            ViewBag.sortComplDateParameter = sortBy == "Complete Date" ? "Complete Date desc" : "Complete Date";
            ViewBag.Open = (searchAll ? "STARTED,READY FOR REVIEW,COMPLETED" : frmStatus).Contains("STARTED")?"checked":"";
            ViewBag.Ready = (searchAll ? "STARTED,READY FOR REVIEW,COMPLETED" : frmStatus).Contains("READY FOR REVIEW") ? "checked" : "";
            ViewBag.Completed = (searchAll ? "STARTED,READY FOR REVIEW,COMPLETED" : frmStatus).Contains("COMPLETED") ? "checked" : "";
            ViewBag.FullfrmStatus = frmStatus;
            if (usr.role == "Not Authorized" || usr.role == "")
            {

                return View(apiParcer.getAllObservations(usr.emp_id, frmStatus, searchString, sortBy).ToPagedList(page ?? 1, PageSize ?? 15));
            }
            else
            {          
                return View(apiParcer.getAllObservations(frmStatus, searchString, sortBy).ToPagedList(page ?? 1, PageSize ?? 15));
            }
                
        }

        [HttpGet]
        public ActionResult viewForm(int id)
        {
            return View(apiParcer.getFormInstance(id));
        }

        // GET: Observation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Observation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Observation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Observation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Observation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Observation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
