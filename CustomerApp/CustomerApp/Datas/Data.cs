using CustomerApp.Models;
using CustomerApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerApp.Datas
{
    public class Data
    {
        #region Gender
        public static List<OptionSet> GenderData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.gender_nam),
                new OptionSet("2",Language.gender_nu),
                new OptionSet("100000000",Language.gender_khac),
            };
        }

        public static OptionSet GetGenderById(string id)
        {
            return GenderData().SingleOrDefault(x => x.Val == id);
        }
        #endregion
    }
}
