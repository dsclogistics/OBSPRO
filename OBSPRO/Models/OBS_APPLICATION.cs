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
    
    public partial class OBS_APPLICATION
    {
        public OBS_APPLICATION()
        {
            this.OBS_API_EVENT_LOG = new HashSet<OBS_API_EVENT_LOG>();
        }
    
        public short obs_app_id { get; set; }
        public string obs_app_name { get; set; }
        public string obs_app_name_token { get; set; }
        public string obs_app_type { get; set; }
        public System.DateTime obs_app_in_svc_dt { get; set; }
    
        public virtual ICollection<OBS_API_EVENT_LOG> OBS_API_EVENT_LOG { get; set; }
    }
}
