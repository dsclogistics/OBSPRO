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
    
    public partial class OBS_QUESTION
    {
        public OBS_QUESTION()
        {
            this.OBS_COL_FORM_INST_MM_ATTACH = new HashSet<OBS_COL_FORM_INST_MM_ATTACH>();
            this.OBS_COL_FORM_INST_QUEST = new HashSet<OBS_COL_FORM_INST_QUEST>();
            this.OBS_QUEST_ANS_TYPES = new HashSet<OBS_QUEST_ANS_TYPES>();
            this.OBS_QUEST_ASSGND_MD = new HashSet<OBS_QUEST_ASSGND_MD>();
        }
    
        public int obs_question_id { get; set; }
        public short obs_question_ver { get; set; }
        public string obs_question_full_text { get; set; }
        public string obs_question_short_text { get; set; }
        public string obs_question_desc { get; set; }
        public string obs_question_mm_url { get; set; }
        public System.DateTime obs_question_eff_st_dt { get; set; }
        public System.DateTime obs_question_eff_end_dt { get; set; }
        public string obs_question_added_uid { get; set; }
        public System.DateTime obs_question_added_dtm { get; set; }
        public string obs_question_upd_uid { get; set; }
        public Nullable<System.DateTime> obs_question_upd_dtm { get; set; }
    
        public virtual ICollection<OBS_COL_FORM_INST_MM_ATTACH> OBS_COL_FORM_INST_MM_ATTACH { get; set; }
        public virtual ICollection<OBS_COL_FORM_INST_QUEST> OBS_COL_FORM_INST_QUEST { get; set; }
        public virtual ICollection<OBS_QUEST_ANS_TYPES> OBS_QUEST_ANS_TYPES { get; set; }
        public virtual ICollection<OBS_QUEST_ASSGND_MD> OBS_QUEST_ASSGND_MD { get; set; }
    }
}