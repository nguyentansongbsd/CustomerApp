using CustomerApp.Datas;
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
        public byte[] AvatarArr { get; set; }

        private string _avatar;
        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(nameof(Avatar)); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }

        private string _oldPassword;
        public string OldPassword { get => _oldPassword; set { _oldPassword = value; OnPropertyChanged(nameof(OldPassword)); } }

        private string _newPassword;
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); } }

        private string _confirmNewPassword;
        public string ConfirmNewPassword { get => _confirmNewPassword; set { _confirmNewPassword = value; OnPropertyChanged(nameof(ConfirmNewPassword)); } }

        private OptionSet _gender;
        public OptionSet Gender { get => _gender; set { _gender = value; OnPropertyChanged(nameof(Gender)); } }

        private List<OptionSet> _genders;
        public List<OptionSet> Genders { get => _genders; set { _genders = value; OnPropertyChanged(nameof(Genders)); } }

        private AddressModel _addressContact;
        public AddressModel AddressContact { get => _addressContact; set { _addressContact = value; OnPropertyChanged(nameof(AddressContact)); } }

        public UserInfoPageViewModel()
        {
            Password = UserLogged.Password;
            Genders = Data.GenderData();
            Avatar = UserLogged.Avartar;
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
                                        <attribute name='bsd_countryid' alias='country_id'/>
                                        <attribute name='bsd_countryname' alias='country_name'/>
                                        <attribute name='bsd_nameen'  alias='country_name_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer' alias='a_8fd440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_provinceid' alias='province_id'/>
                                        <attribute name='new_name' alias='province_name'/>
                                        <attribute name='bsd_nameen'  alias='province_name_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer' alias='a_50d440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_districtid' alias='district_id'/>
                                        <attribute name='new_name' alias='district_name'/>
                                        <attribute name='bsd_nameen'  alias='district_name_en'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactModel>>("contacts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            Contact = result.value.SingleOrDefault();
            Gender = Data.GetGenderById(Contact.gendercode);
            AddressContact = new AddressModel
            {
                country_id = Contact.country_id,
                country_name = !string.IsNullOrWhiteSpace(Contact.country_name_en) && UserLogged.Language == "en" ? Contact.country_name_en : Contact.country_name,
                country_name_en = Contact.country_name_en,
                province_id = Contact.province_id,
                province_name = !string.IsNullOrWhiteSpace(Contact.province_name_en) && UserLogged.Language == "en" ? Contact.province_name_en : Contact.province_name,
                province_name_en = Contact.province_name_en,
                district_id = Contact.district_id,
                district_name = !string.IsNullOrWhiteSpace(Contact.district_name_en) && UserLogged.Language == "en" ? Contact.district_name_en : Contact.district_name,
                district_name_en = Contact.district_name_en,
                address = Contact.bsd_contactaddress,
                lineaddress = Contact.bsd_housenumberstreet
            };
        }
        // thay doi mat khau
        public async Task<bool> ChangePassword()
        {
            string path = $"/contacts({UserLogged.Id})";
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
        // update avata
        public async Task<bool> ChangeAvatar()
        {
            string path = $"/contacts({UserLogged.Id})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["entityimage"] = this.AvatarArr;

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

        public async Task<bool> UpdateUserInfor()
        {
            string path = $"/contacts({UserLogged.ContactId})";
            var content = await GetContent();
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

        public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        {
            var result = await CrmHelper.SetNullLookupField("contacts", contactId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> GetContent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["emailaddress1"] = Contact.emailaddress1;
            data["mobilephone"] = Contact.mobilephone;
            data["birthdate"] = (DateTime.Parse(Contact.birthdate.ToString()).ToLocalTime()).ToString("yyyy-MM-dd");
            data["gendercode"] = this.Gender?.Val;
           // data["bsd_postalcode"] = Contact.bsd_postalcode;

            if (AddressContact != null && !string.IsNullOrWhiteSpace(AddressContact.lineaddress))
            {
                if (!string.IsNullOrWhiteSpace(AddressContact.address))
                    data["bsd_contactaddress"] = AddressContact.address;
                if (!string.IsNullOrWhiteSpace(AddressContact.lineaddress))
                    data["bsd_housenumberstreet"] = AddressContact.lineaddress;

                if (AddressContact.country_id != Guid.Empty)
                    data["bsd_country@odata.bind"] = "/bsd_countries(" + AddressContact.country_id + ")";
                else
                    await DeletLookup("bsd_country", Contact.contactid);

                if (AddressContact.province_id != Guid.Empty)
                    data["bsd_province@odata.bind"] = "/new_provinces(" + AddressContact.province_id + ")";
                else
                    await DeletLookup("bsd_province", Contact.contactid);

                if (AddressContact.district_id != Guid.Empty)
                    data["bsd_district@odata.bind"] = "/new_districts(" + AddressContact.district_id + ")";
                else
                    await DeletLookup("bsd_district", Contact.contactid);
            }

            return data;
        }
    }
}
