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
    
    public partial class OBS_API_JSON
    {
        public long obs_api_json_id { get; set; }
        public long obs_api_event_log_id { get; set; }
        public short obs_api_json_seq { get; set; }
        public string obs_api_json_rcvd_sent { get; set; }
        public string obs_api_json_text { get; set; }
    
        public virtual OBS_API_EVENT_LOG OBS_API_EVENT_LOG { get; set; }
    }
}