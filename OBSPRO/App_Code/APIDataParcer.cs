﻿using Newtonsoft.Json.Linq;
using OBSPRO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace OBSPRO.App_Code
{   
    public class APIDataParcer
    {
        DataRetrieval api = new DataRetrieval();
        User usr = new User();

        //This method returns dashboard data
        public Dashboard getDashboard(string emp_id)
        {
            Dashboard dashboard = new Dashboard();
            if (String.IsNullOrEmpty(emp_id))
            {
                dashboard.isSuperUser = true;
            }
            else
            {
                dashboard.isSuperUser = false;
            }
            string raw_data = api.getOpenReadybyUser(emp_id,null);
            JObject parsed_result = JObject.Parse(raw_data);
            
            foreach (var res in parsed_result["resource"])
            {
                Observation obs = new Observation();
                obs.form_inst_id = (string)res["ObsColFormInstID"];
                obs.observed_id = (int)res["dsc_observed_emp_id"];
                obs.observer_id = (int)res["dsc_observer_emp_id"];
                obs.status = (string)res["obs_inst_status"] == "OPEN" ? "Started" : (string)res["obs_inst_status"] == "READY TO VERIFY" ? "Ready for Review" : (string)res["obs_inst_status"];
                obs.observed_first_name = (string)res["Observedfirst_name"];
                obs.observed_last_name = (string)res["Observedlast_name"];
                obs.observer_first_name = (string)res["Observerfirst_name"];
                obs.observer_last_name = (string)res["Observerlast_name"];
                obs.observed_adp_id = (string)res["ObservedADPID"];
                obs.form_title = (string)res["ColFormTitle"];
                obs.location = (string)res["lcname"];
                obs.obs_start_time = Convert.ToDateTime((string)res["ColFormStartDateTime"]);
                try
                {
                    obs.obs_compl_time = Convert.ToDateTime((string)res["completeddate"]);
                }
                catch
                {
                }
                switch (obs.status)
                {
                    case "Started":
                        dashboard.user_open_obs.Add(obs);
                        break;
                    case "Ready for Review":
                        dashboard.user_ready_obs.Add(obs);
                        break;
                    case "COMPLETE":
                        dashboard.user_complete_obs.Add(obs);
                        break;
                }
            }
            dashboard.user_open_obs = dashboard.user_open_obs.OrderByDescending(x => x.obs_start_time).ToList();
            dashboard.user_ready_obs = dashboard.user_ready_obs.OrderByDescending(x => x.obs_compl_time).ToList();

            return dashboard;
        }
        //This method returns dashboard data for super user 
        //public Dashboard getDashboard()
        //{
        //    Dashboard dashboard = new Dashboard();
        //    dashboard.isSuperUser = true;
        //    var all_obs_ins = (from j in db.OBS_COLLECT_FORM_INST
        //                       join g in db.OBS_COLLECT_FORM_TMPLT on j.obs_cft_id equals g.obs_cft_id
        //                       join k in db.OBS_INST on j.obs_inst_id equals k.obs_inst_id
        //                       join m in db.DSC_EMPLOYEE on k.dsc_observed_emp_id equals m.dsc_emp_id
        //                       join n in db.DSC_EMPLOYEE on j.dsc_observer_emp_id equals n.dsc_emp_id
        //                       where k.obs_inst_del_yn =="N"
        //                       orderby j.obs_cfi_start_dt descending
        //                       select new
        //                       {
        //                           observed_id = k.dsc_observed_emp_id,
        //                           observed_first_name = m.dsc_emp_first_name,
        //                           observed_last_name = m.dsc_emp_last_name,
        //                           observed_adp_id = m.dsc_emp_adp_id,
        //                           observer_id = j.dsc_observer_emp_id,
        //                           observer_first_name = n.dsc_emp_first_name,
        //                           observer_last_name = n.dsc_emp_last_name,
        //                           lc_id = k.dsc_lc_id,
        //                           customer_id = k.dsc_cust_id,
        //                           cft_id = j.obs_cft_id,
        //                           form_title = g.obs_cft_title,
        //                           inst_id = j.obs_inst_id,
        //                           cfi_id = j.obs_cfi_id,
        //                           start_date = j.obs_cfi_start_dt,
        //                           compl_date = j.obs_cfi_comp_date,
        //                           status = k.obs_inst_status=="COLLECTING"?"STARTED": "READY FOR REVIEW"
        //                       }).ToList();
        //    foreach(var inst in all_obs_ins)
        //    {
        //        Observation obs = new Observation();
        //        obs.form_inst_id = inst.cfi_id.ToString();
        //        obs.form_title = inst.form_title;
        //        obs.observer_id = inst.observer_id;
        //        obs.observed_adp_id = inst.observed_adp_id;
        //        obs.observer_first_name = inst.observer_first_name;
        //        obs.observer_last_name = inst.observer_last_name;
        //        obs.observed_id = (int)inst.observed_id;
        //        obs.observed_first_name = inst.observed_first_name;
        //        obs.observed_last_name = inst.observed_last_name;
        //        obs.obs_start_time = inst.start_date;
        //        obs.obs_compl_time = Convert.ToDateTime(inst.compl_date).ToString("MMM dd, yyyy hh:mm tt"); 
        //        obs.status = inst.status;
        //        switch (obs.status)
        //        {
        //            case "STARTED":
        //                dashboard.user_open_obs.Add(obs);
        //                break;
        //            case "READY FOR REVIEW":
        //                dashboard.user_ready_obs.Add(obs);
        //                break;
        //            case "COMPLETE":
        //                dashboard.user_complete_obs.Add(obs);
        //                break;
        //        }
        //    }            
        //    return dashboard;
        //}

        //This method is to parse get Collection form API. it returns the data for saved or submitted collection form with all the questions and answers
        public OBSCollectionForm getFormInstance(int? formId)
        {
            int intFormId = formId ?? 0;
            OBSCollectionForm obsColForm = new OBSCollectionForm();
            JObject parsed_result;
            bool APIcall_Succeeded = false;
            string APIcall_Message = "";

            try {
                parsed_result = JObject.Parse(api.getObsCollForm(intFormId));
                //Verify that the API call succeeded
                APIcall_Succeeded = ((string)parsed_result["result"]).Equals("SUCCESS") ? true : false;
                APIcall_Message = (string)parsed_result["message"];
                // Throw exception if the API call returns an error
                if (!APIcall_Succeeded) { throw new Exception(APIcall_Message); }  
            }
            catch (Exception ex) {
                // Throw exception if the API call fails
                APIcall_Succeeded = false;
                APIcall_Message = ex.Message;
                throw new Exception(APIcall_Message);
            }
            // We reach this section only if the API call is successfull and it returns a "SUCCESS" response.
            Section current_section = new Section();
            DateTime tempDateTime;
            current_section.sectionName = String.Empty;
            obsColForm.observedEmployeeId = (int)parsed_result["observationsColFormData"]["ObservedEmployeeId"];
            obsColForm.observerEmployeeId = (int)parsed_result["observationsColFormData"]["ObserverEmployeeID"];
            obsColForm.observerEmployeeFullName = (string)parsed_result["observationsColFormData"]["ObserverFullName"];
            obsColForm.observedEmployeeFullName = (string)parsed_result["observationsColFormData"]["ObservedFullName"];
            obsColForm.observedADPID = (string)parsed_result["observationsColFormData"]["ObservedADPID"];
            if (DateTime.TryParse((string)parsed_result["observationsColFormData"]["hiredDate"], out tempDateTime)) {
                obsColForm.hiredDate = tempDateTime; 
            }
            obsColForm.lc_id = (string)parsed_result["observationsColFormData"]["ObservedLocation"];
            obsColForm.customer_id = (string)parsed_result["observationsColFormData"]["customer"];
            obsColForm.obsColFormId = (int)parsed_result["observationsColFormData"]["OBSColFormID"];
            obsColForm.colFormTitle = (string)parsed_result["observationsColFormData"]["ColFormTitle"];
            obsColForm.colFormSubtitle = (string)parsed_result["observationsColFormData"]["ColFormSubTitle"];
            obsColForm.colFormVersion = (string)parsed_result["observationsColFormData"]["ColFormVersion"];
            obsColForm.obsInstId = (string)parsed_result["observationsColFormData"]["OBSInstID"];
            obsColForm.obsColFormInstId = (long)parsed_result["observationsColFormData"]["OBSColFormInstID"];
            obsColForm.colFormStartDateTime = Convert.ToDateTime((string)parsed_result["observationsColFormData"]["ColFormStartDateTime"]);
            obsColForm.strColFormStartDateTime = obsColForm.colFormStartDateTime.ToString("MMM dd, yyyy hh:mm tt");
            if (DateTime.TryParse((string)parsed_result["observationsColFormData"]["ObservationCompDate"], out tempDateTime)) {
                obsColForm.colFormSubmittedDateTime = tempDateTime;
                obsColForm.strColFormSubmittedDateTime = obsColForm.colFormSubmittedDateTime.ToString("MMM dd, yyyy hh:mm tt");
            }
            else {
                obsColForm.strColFormSubmittedDateTime = "";
            }           
            obsColForm.dBColFormStatus = (string)parsed_result["observationsColFormData"]["DBColFormStatus"];
            obsColForm.colFormStatus = (string)parsed_result["observationsColFormData"]["ColFormStatus"]=="Ready"?"Ready for Review": (string)parsed_result["observationsColFormData"]["ColFormStatus"]=="Open"?"Started": (string)parsed_result["observationsColFormData"]["ColFormStatus"];          
            obsColForm.colFormPrintPath = Common.ReadSetting("ServerType");
            JArray questions = (JArray)parsed_result["observationsColFormData"]["questions"];
            foreach (var quest in questions)
            {
                var answersFound = 0;
                if (current_section.sectionName != (string)quest["SectionName"])
                {
                    Section newSection = new Section();
                    newSection.sectionName = (string)quest["SectionName"];
                    if (!String.IsNullOrEmpty(current_section.sectionName))
                    {
                        obsColForm.sections.Add(current_section);
                    }
                    current_section = newSection;
                }
                Question formQuestion = new Question();
                formQuestion.uniqueQuestionId = (int)quest["UniqueQuestionId"];
                formQuestion.questionId = (int)quest["QuestionId"];
                formQuestion.questionOrder = (int)quest["QuestionOrder"];
                formQuestion.questionText = (string)quest["QuestionText"];
                formQuestion.answerType = (string)quest["AnswerType"];
                formQuestion.showsNA = (string)quest["showsNA"];
                formQuestion.canAddComment = (string)quest["canAddComment"];
                formQuestion.mustAddComment = (string)quest["MustAddComment"];
                formQuestion.answerChanged = (string)quest["answerChanged"];
                formQuestion.obscolformquestwgt = (int)quest["obscolformquestwgt"];
                formQuestion.comments = (string)quest["comments"];
                JArray answers = (JArray)quest["answers"];
                if (answers.HasValues)
                {
                    foreach (var ans in answers)
                    {
                        Answer answer = new Answer();
                        answer.answerId = ans["answerId"] == null ? 0 : (int)ans["answerId"];
                        answer.cfiqid = ans["CFIQID"] == null ? 0 : (int)ans["CFIQID"];
                        answer.obscolanswgt = (int)ans["obscolanswgt"];
                        answer.answerOrder = (int)ans["answerOrder"];
                        answer.answerText = (string)ans["answerText"];
                        formQuestion.answers.Add(answer);
                    }
                }
                JArray answer_values = (JArray)quest["answervalues"];
                if (answer_values != null && answer_values.HasValues)
                {
                    foreach (var ans_val in answer_values)
                    {
                        Answer answer_value = new Answer();
                        answer_value.answerId = 0;
                        answer_value.cfiqid = 0;
                        answer_value.obscolanswgt = (int)ans_val["obscolanswgt"];
                        answer_value.answerOrder = (int)ans_val["answerOrder"];
                        answer_value.answerText = (string)ans_val["answerText"];
                        formQuestion.naSelected = answer_value.answerText.Equals("n/a") ? true : false;
                        if (!answer_value.answerText.Equals("n/a")) { answersFound++; }
                        if (!formQuestion.naSelected)
                        {
                            var selected_answer = formQuestion.answers.FirstOrDefault(x => x.answerText == answer_value.answerText);
                            if (selected_answer != null)
                            {
                                selected_answer.isSelected = true;
                            }
                        }
                        formQuestion.answerValues.Add(answer_value);

                    }
                    formQuestion.responseClass = "answered";
                }
                else { 
                    formQuestion.responseClass = "unanswered";
                }
                if (formQuestion.answerType.Equals("FREE_TXT") && !(String.IsNullOrEmpty(formQuestion.comments)))
                {
                    formQuestion.responseClass = "answered";
                }
                
                current_section.questions.Add(formQuestion);
            }
            obsColForm.sections.Add(current_section);
            return obsColForm;
        }

        //This method returns all observations for Find Observation Menu
        public List<Observation> getAllObservations(string emp_id, string frmStatus, string searchString, string sortBy)
        {
            List<Observation> all_obs = new List<Observation>();
            //usr.setUser();
            JObject parsed_result = JObject.Parse(api.getOpenReadybyUser(emp_id, searchString));
            foreach (var res in parsed_result["resource"])
            {
                Observation obs = new Observation();
                obs.form_inst_id = (string)res["ObsColFormInstID"];
                obs.observed_id = (int)res["dsc_observed_emp_id"];
                obs.observer_id = (int)res["dsc_observer_emp_id"];
                obs.status = (string)res["obs_inst_status"]=="OPEN"?"Started": (string)res["obs_inst_status"]== "READY TO VERIFY"?"Ready for Review": (string)res["obs_inst_status"];
                obs.observed_first_name = (string)res["Observedfirst_name"];
                obs.observed_last_name = (string)res["Observedlast_name"];
                obs.observer_first_name = (string)res["Observerfirst_name"];
                obs.observer_last_name = (string)res["Observerlast_name"];
                obs.observed_adp_id = (string)res["ObservedADPID"];
                obs.form_title = (string)res["ColFormTitle"];
                obs.location = (string)res["lcname"];
                obs.obs_start_time = Convert.ToDateTime((string)res["ColFormStartDateTime"]);
                try
                {
                    obs.obs_compl_time = Convert.ToDateTime((string)res["completeddate"]);                   
                }
                catch
                {
                }
               
                if (frmStatus.Contains(obs.status))
                {
                    all_obs.Add(obs);
                }
            }

            switch (sortBy)
            {
                case "StartDate":
                    all_obs = all_obs.OrderBy(x => x.obs_start_time).ToList();
                    break;
                case "Title desc":
                    all_obs = all_obs.OrderByDescending(x => x.form_title).ToList();
                    break;
                case "Title":
                    all_obs = all_obs.OrderBy(x => x.form_title).ToList();
                    break;
                case "Observed Emplpoyee desc":
                    all_obs = all_obs.OrderByDescending(x => x.observed_last_name).ToList();
                    break;
                case "Observed Emplpoyee":
                    all_obs = all_obs.OrderBy(x => x.observed_last_name).ToList();
                    break;
                case "Observer desc":
                    all_obs = all_obs.OrderByDescending(x => x.observer_last_name).ToList();
                    break;
                case "Observer":
                    all_obs = all_obs.OrderBy(x => x.observer_last_name).ToList();
                    break;
                case "ADP ID desc":
                    all_obs = all_obs.OrderByDescending(x => int.Parse(x.observed_adp_id)).ToList();
                    break;
                case "ADP ID":
                    all_obs = all_obs.OrderBy(x => int.Parse(x.observed_adp_id)).ToList();
                    break;
                case "Status desc":
                    all_obs = all_obs.OrderByDescending(x => x.status).ToList();
                    break;
                case "Location desc":
                    all_obs = all_obs.OrderByDescending(x => x.location).ToList();
                    break;
                case "Location":
                    all_obs = all_obs.OrderBy(x => x.location).ToList();
                    break;
                case "Status":
                    all_obs = all_obs.OrderBy(x => x.status).ToList();
                    break;
                case "Complete Date desc":
                    all_obs = all_obs.OrderByDescending(x => x.obs_compl_time).ToList();
                    break;
                case "Complete Date":
                    all_obs = all_obs.OrderBy(x => x.obs_compl_time).ToList();
                    break;
                default: all_obs = all_obs.OrderByDescending(x => x.obs_start_time).ToList();
                    break;
            }
            return all_obs;
        }

        //This method returns all observations for super user  
        //public List<Observation> getAllObservations(string frmStatus, string searchString, string sortBy)
        //{
        //    List<Observation> all_obs = new List<Observation>();
        //    usr.setUser();
        //    JObject parsed_result = JObject.Parse(api.getOpenReadybyUser(null, searchString));
        //    foreach (var res in parsed_result["resource"])
        //    {
        //        Observation obs = new Observation();
        //        obs.form_inst_id = (string)res["ObsColFormInstID"];
        //        obs.observed_id = (int)res["dsc_observed_emp_id"];
        //        obs.observer_id = (int)res["dsc_observer_emp_id"];
        //        obs.status = (string)res["obs_inst_status"] == "OPEN" ? "STARTED" : (string)res["obs_inst_status"] == "READY TO VERIFY" ? "READY FOR REVIEW" : (string)res["obs_inst_status"];
        //        obs.observed_first_name = (string)res["Observedfirst_name"];
        //        obs.observed_last_name = (string)res["Observedlast_name"];
        //        obs.observer_first_name = (string)res["Observerfirst_name"];
        //        obs.observer_last_name = (string)res["Observerlast_name"];
        //        obs.observed_adp_id = (string)res["ObservedADPID"];
        //        obs.form_title = (string)res["ColFormTitle"];
        //        obs.location = (string)res["lcname"];
        //        obs.obs_start_time = Convert.ToDateTime((string)res["ColFormStartDateTime"]);
        //        try
        //        {
        //            obs.obs_compl_time = Convert.ToDateTime((string)res["completeddate"]);
        //        }
        //        catch
        //        {
        //        }

        //        if (frmStatus.Contains(obs.status))
        //        {
        //            all_obs.Add(obs);
        //        }
        //    }
        //    switch (sortBy)
        //    {
        //        case "StartDate":
        //            all_obs = all_obs.OrderBy(x => x.obs_start_time).ToList();
        //            break;
        //        case "Title desc":
        //            all_obs = all_obs.OrderByDescending(x => x.form_title).ToList();
        //            break;
        //        case "Title":
        //            all_obs = all_obs.OrderBy(x => x.form_title).ToList();
        //            break;
        //        case "Observed Emplpoyee desc":
        //            all_obs = all_obs.OrderByDescending(x => x.observed_last_name).ToList();
        //            break;
        //        case "Observed Emplpoyee":
        //            all_obs = all_obs.OrderBy(x => x.observed_last_name).ToList();
        //            break;
        //        case "Observer desc":
        //            all_obs = all_obs.OrderByDescending(x => x.observer_last_name).ToList();
        //            break;
        //        case "Observer":
        //            all_obs = all_obs.OrderBy(x => x.observer_last_name).ToList();
        //            break;
        //        case "ADP ID desc":
        //            all_obs = all_obs.OrderByDescending(x => int.Parse(x.observed_adp_id)).ToList();
        //            break;
        //        case "ADP ID":
        //            all_obs = all_obs.OrderBy(x => int.Parse(x.observed_adp_id)).ToList();
        //            break;
        //        case "Status desc":
        //            all_obs = all_obs.OrderByDescending(x => x.status).ToList();
        //            break;
        //        case "Status":
        //            all_obs = all_obs.OrderBy(x => x.status).ToList();
        //            break;
        //        case "Location desc":
        //            all_obs = all_obs.OrderByDescending(x => x.location).ToList();
        //            break;
        //        case "Location":
        //            all_obs = all_obs.OrderBy(x => x.location).ToList();
        //            break;
        //        case "Complete Date desc":
        //            all_obs = all_obs.OrderByDescending(x => x.obs_compl_time).ToList();
        //            break;
        //        case "Complete Date":
        //            all_obs = all_obs.OrderBy(x => x.obs_compl_time).ToList();
        //            break;
        //    }
        //    return all_obs;
        //}

        //This method returns all observations for super user  
        //public List<Observation> getAllObservations(string frmStatus, string searchString, string sortBy)
        //{
        //    List<Observation> all_obs = new List<Observation>();
        //    var all_obs_ins = (from j in db.OBS_COLLECT_FORM_INST
        //                       join g in db.OBS_COLLECT_FORM_TMPLT on j.obs_cft_id equals g.obs_cft_id
        //                       join k in db.OBS_INST on j.obs_inst_id equals k.obs_inst_id
        //                       join m in db.DSC_EMPLOYEE on k.dsc_observed_emp_id equals m.dsc_emp_id
        //                       join n in db.DSC_EMPLOYEE on j.dsc_observer_emp_id equals n.dsc_emp_id
        //                       where k.obs_inst_del_yn == "N"
        //                       orderby j.obs_cfi_start_dt descending
        //                       select new
        //                       {
        //                           observed_id = k.dsc_observed_emp_id,
        //                           observed_first_name = m.dsc_emp_first_name,
        //                           observed_last_name = m.dsc_emp_last_name,
        //                           observed_adp_id = m.dsc_emp_adp_id,
        //                           observer_id = j.dsc_observer_emp_id,
        //                           observer_first_name = n.dsc_emp_first_name,
        //                           observer_last_name = n.dsc_emp_last_name,
        //                           lc_id = k.dsc_lc_id,
        //                           customer_id = k.dsc_cust_id,
        //                           cft_id = j.obs_cft_id,
        //                           form_title = g.obs_cft_title,
        //                           inst_id = j.obs_inst_id,
        //                           cfi_id = j.obs_cfi_id,
        //                           start_date = j.obs_cfi_start_dt,
        //                           compl_date = j.obs_cfi_comp_date,
        //                           status = k.obs_inst_status == "COLLECTING" ? "STARTED" : "READY FOR REVIEW"
        //                       }).ToList();
        //    foreach (var inst in all_obs_ins)
        //    {
        //        Observation obs = new Observation();
        //        obs.form_inst_id = inst.cfi_id.ToString();
        //        obs.form_title = inst.form_title;
        //        obs.observer_id = inst.observer_id;
        //        obs.observed_adp_id = inst.observed_adp_id;
        //        obs.observer_first_name = inst.observer_first_name;
        //        obs.observer_last_name = inst.observer_last_name;
        //        obs.observed_id = (int)inst.observed_id;
        //        obs.observed_first_name = inst.observed_first_name;
        //        obs.observed_last_name = inst.observed_last_name;
        //        obs.obs_start_time = inst.start_date;
        //        obs.obs_compl_time = Convert.ToDateTime(inst.compl_date).ToString("MMM dd, yyyy hh:mm tt");
        //        obs.status = inst.status;
        //        if (frmStatus.Contains(obs.status))
        //        {
        //            if (String.IsNullOrEmpty(searchString))//no search parameter passed
        //            {
        //                all_obs.Add(obs);
        //            }
        //            else
        //            {
        //                string search_in = obs.form_title + obs.observed_first_name + obs.observed_last_name + obs.observer_first_name + obs.observer_last_name;
        //                if (Common.matchesSearchCriteria(searchString, search_in, "All"))
        //                {
        //                    all_obs.Add(obs);
        //                }
        //            }

        //        }                
        //    }
        //    switch (sortBy)
        //    {
        //        case "StartDate":
        //            all_obs = all_obs.OrderBy(x => x.obs_start_time).ToList();
        //            break;
        //        case "Title desc":
        //            all_obs = all_obs.OrderByDescending(x => x.form_title).ToList();
        //            break;
        //        case "Title":
        //            all_obs = all_obs.OrderBy(x => x.form_title).ToList();
        //            break;
        //        case "Observed Emplpoyee desc":
        //            all_obs = all_obs.OrderByDescending(x =>x.observed_last_name).ToList();
        //            break;
        //        case "Observed Emplpoyee":
        //            all_obs = all_obs.OrderBy(x => x.observed_last_name).ToList();
        //            break;
        //        case "Observer desc":
        //            all_obs = all_obs.OrderByDescending(x => x.observer_last_name).ToList();
        //            break;
        //        case "Observer":
        //            all_obs = all_obs.OrderBy(x => x.observer_last_name).ToList();
        //            break;
        //        case "ADP ID desc":
        //            all_obs = all_obs.OrderByDescending(x => int.Parse(x.observed_adp_id)).ToList();
        //            break;
        //        case "ADP ID":
        //            all_obs = all_obs.OrderBy(x => int.Parse(x.observed_adp_id)).ToList();
        //            break;
        //        case "Status desc":
        //            all_obs = all_obs.OrderByDescending(x => x.status).ToList();
        //            break;
        //        case "Status":
        //            all_obs = all_obs.OrderBy(x => x.status).ToList();
        //            break;
        //        case "Complete Date desc":
        //            all_obs = all_obs.OrderByDescending(x => x.obs_compl_time).ToList();
        //            break;
        //        case "Complete Date":
        //            all_obs = all_obs.OrderBy(x => x.obs_compl_time).ToList();
        //            break;
        //    }
        //    return all_obs;
        //}

        public string getUserRole(string userName)
        {
                      
            JObject parsed_result;
            try
            {
                parsed_result = JObject.Parse(api.getObserverRole(userName));
                JArray roles = (JArray)parsed_result["resource"];
                foreach (var res in roles)
                {
                    if ((string)res["obs_role_active_yn"] == "Y")
                    {
                        return (string)res["obs_role_name"];
                    }
                    else
                    {

                        return "Not Authorized";
                    }

                }
                return "Not Authorized";
            }
            catch
            {              
                return "Not Authorized";
            }
            
        }
    }
}