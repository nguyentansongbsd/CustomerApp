using CustomerApp.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class PhasesLanchModel
    {
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }
        public Guid bsd_phaseslaunchid { get; set; }
        public DateTime startdate_event { get; set; }
        public DateTime enddate_event { get; set; }
        public string statuscode_event { get; set; }

        public Guid discount_id { get; set; }
        public string discount_name { get; set; }
        public Guid internel_id { get; set; }
        public string internel_name { get; set; }
        public Guid paymentscheme_id { get; set; }
        public string paymentscheme_name { get; set; }
        public Guid promotion_id { get; set; }
        public string promotion_name { get; set; }
        public DateTime bsd_enddate { get; set; }
        public string statuscode { get; set; }
        public string statuscode_format { get => Data.GetPhasesLanchStatusCodeById(statuscode)?.Name; }
        public string statuscode_color { get => Data.GetPhasesLanchStatusCodeById(statuscode)?.Background; }
    }
}
