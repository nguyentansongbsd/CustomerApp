using System;
namespace CustomerApp.Models
{
    public class LoyaltyModel
    {
        public Guid contactid { get; set; }
        public decimal? bsd_totalamountofownership3years { get; set; }
        public decimal? bsd_totalamountofownership { get; set; }
        public string bsd_loyaltystatus { get; set; }
        public DateTime bsd_loyaltydate { get; set; }
        public Guid membershiptier_id { get; set; }
        public string membershiptier_name { get; set; }
    }
}
