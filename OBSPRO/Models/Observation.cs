﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBSPRO.Models
{
    public class Observation
    {
      
        [Display(Name = "Location")]
        public string LC { get; set; }

   
        [Display(Name = "User Name")]
        public string user_name { get; set; }

        [Display(Name = "Form Instance Id")]
        public string form_inst_id { set; get; } 

        [Display(Name = "Observer Id")]
        public string observer_id { set; get; }

        [Display(Name = "Observed Id")]
        public string observed_id { set; get; }

        [Display(Name = "Status")]
        public string status { set; get; }

        [Display(Name = "First Name")]
        public string observer_first_name { set; get; }

        [Display(Name = "Last Name")]
        public string observed_last_name { set; get; }


        [Display(Name = "ADP ID")]
        public string observed_adp_id { set; get; }


        [Display(Name = "Form Title")]
        public string form_title { set; get; }

        [Display(Name = "Observation Start Time")]
        public string obs_start_time { set; get; }

    }
}