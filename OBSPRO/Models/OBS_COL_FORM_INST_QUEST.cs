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
    
    public partial class OBS_COL_FORM_INST_QUEST
    {
        public OBS_COL_FORM_INST_QUEST()
        {
            this.OBS_COL_FORM_INST_ANS = new HashSet<OBS_COL_FORM_INST_ANS>();
        }
    
        public long obs_cfiq_id { get; set; }
        public long obs_col_form_quest_id { get; set; }
        public long obs_cfi_id { get; set; }
        public int obs_question_id { get; set; }
        public string obs_cfiq_quest_full_text { get; set; }
        public Nullable<short> obs_cfiq_quest_wgt { get; set; }
        public string obs_cfiq_comment { get; set; }
        public short obs_cfiq_quest_order { get; set; }
        public string obs_cfiq_form_sect_name { get; set; }
        public string obs_cfiq_form_sub_sect_name { get; set; }
        public string obs_cfiq_comment_mand_yn { get; set; }
        public string obs_cfiq_na_yn { get; set; }
        public string obs_cfiq_mult_ans_yn { get; set; }
        public Nullable<System.DateTime> obs_cfiq_init_ans_dtm { get; set; }
        public short obs_cfiq_version { get; set; }
        public Nullable<System.DateTime> obs_cfiq_upd_dtm { get; set; }
    
        public virtual ICollection<OBS_COL_FORM_INST_ANS> OBS_COL_FORM_INST_ANS { get; set; }
        public virtual OBS_COL_FORM_QUESTIONS OBS_COL_FORM_QUESTIONS { get; set; }
        public virtual OBS_COLLECT_FORM_INST OBS_COLLECT_FORM_INST { get; set; }
        public virtual OBS_QUESTION OBS_QUESTION { get; set; }
    }
}