using CustomerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class ContactModel
    {
        // id
        public Guid contactid { get; set; }
        // họ tên
        public string bsd_fullname { get; set; }
        // id giới tính
        public string gendercode { get; set; }
        // giới tính format
        public string gender_format { get { return !string.IsNullOrWhiteSpace(gendercode) ? DataHelper.GetGenderById(gendercode)?.Name : null; } }
        //email
        public string emailaddress1 { get; set; }
        // sdt
        public string mobilephone { get; set; }
        // ngày sinh
        public DateTime birthdate { get; set; }
    }
}
