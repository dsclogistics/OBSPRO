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
    
    public partial class OBS_API_RSRC_ACTION
    {
        public OBS_API_RSRC_ACTION()
        {
            this.OBS_API_EVENT_LOG = new HashSet<OBS_API_EVENT_LOG>();
        }
    
        public short obs_API_RA_ID { get; set; }
        public string obs_api_ra_rsrc_name { get; set; }
        public short obs_api_ra_ver { get; set; }
        public string obs_api_ra_action { get; set; }
        public string obs_api_ra_desc { get; set; }
    
        public virtual ICollection<OBS_API_EVENT_LOG> OBS_API_EVENT_LOG { get; set; }
    }
}
