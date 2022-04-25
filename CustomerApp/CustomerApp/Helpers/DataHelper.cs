using CustomerApp.Models;
using CustomerApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerApp.Helpers
{
    public class DataHelper
    {
        public static List<StatusCodeModel> GenderData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0","","#FFFFFF"),
                new StatusCodeModel("1",Language.gender_nam,"#ffc43d"),
                new StatusCodeModel("2",Language.gender_nu,"#F43927"),
                new StatusCodeModel("100000000",Language.gender_khac,"#8bce3d"),
            };
        }

        public static StatusCodeModel GetGenderById(string id)
        {
            return GenderData().SingleOrDefault(x => x.Id == id);
        }
    }
}
