using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class UserInfoPageViewModel : BaseViewModel
    {
        private ContactModel _contact;
        public ContactModel Contact { get => _contact; set { _contact = value; OnPropertyChanged(nameof(Contact)); } }
        public string UserName { get; set; }
        public ImageSource Avatar { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        private bool _isNullPassword;
        public bool isNullPassword { get => _isNullPassword; set { _isNullPassword = value; OnPropertyChanged(nameof(isNullPassword)); } }
        public UserInfoPageViewModel()
        {
            ConverterAvata();
            if (!string.IsNullOrWhiteSpace(UserLogged.Password))
                isNullPassword = true;
            else 
                isNullPassword = false;
        }
        //load thông tin contact
        public async Task LoadContact()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='bsd_fullname' />
                                    <attribute name='mobilephone' />
                                    <attribute name='bsd_identitycardnumber' />
                                    <attribute name='gendercode' />
                                    <attribute name='emailaddress1' />
                                    <attribute name='createdon' />
                                    <attribute name='birthdate' />
                                    <attribute name='contactid' />
                                    <attribute name='bsd_postalcode' />
                                    <attribute name='bsd_housenumberstreet' />
                                    <attribute name='bsd_contactaddress' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer' alias='a_8b5241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_countryid' alias='_bsd_country_value'/>
                                        <attribute name='bsd_countryname' alias='bsd_country_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_country_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer' alias='a_8fd440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_provinceid' alias='_bsd_province_value'/>
                                        <attribute name='new_name' alias='bsd_province_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_province_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer' alias='a_50d440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_districtid' alias='_bsd_district_value'/>
                                        <attribute name='new_name' alias='bsd_district_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_district_en'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactModel>>("contacts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            Contact = result.value.SingleOrDefault();
        }
        // thay doi mat khau
        public async Task<bool> ChangePassword()
        {
            string path = $"/bsd_employees({UserLogged.Id})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_password"] = this.ConfirmNewPassword;

            var content = data as object;
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // chuyển avata sang imagesource
        private void ConverterAvata()
        {
            if (!string.IsNullOrWhiteSpace(UserLogged.Avartar))
            {
                byte[] bytes = System.Convert.FromBase64String(UserLogged.Avartar);
                Avatar = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            else
            {
                string name = string.IsNullOrWhiteSpace(UserLogged.ContactName) ? UserLogged.User : UserLogged.ContactName;
                Avatar = $"https://ui-avatars.com/api/?background=2196F3&rounded=false&color=ffffff&size=150&length=2&name={name}";
            }    
        }
    }
}
