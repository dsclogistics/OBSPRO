//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OBSPRO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OBS_QUEST_ASSGND_MD
    {
        public long obs_qad_id { get; set; }
        public int obs_quest_md_id { get; set; }
        public int obs_question_id { get; set; }
        public System.DateTime obs_qad_eff_st_dt { get; set; }
        public System.DateTime obs_qad_eff_end_dt { get; set; }
    
        public virtual OBS_QUESTION_METADATA OBS_QUESTION_METADATA { get; set; }
        public virtual OBS_QUESTION OBS_QUESTION { get; set; }
    }
}
