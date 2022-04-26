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
        #region NetAreaDirectSale
        public static List<NetAreaDirectSaleModel> NetAreaData()
        {
            return new List<NetAreaDirectSaleModel>()
            {
                new NetAreaDirectSaleModel("1",Language.string_under + " 60 "+Language.sqm,"60"),
                new NetAreaDirectSaleModel("2","60 "+Language.sqm + " -> 80 "+Language.sqm,"60","80"),
                new NetAreaDirectSaleModel("3","81 "+Language.sqm + " -> 100 "+Language.sqm,"81","100"),
                new NetAreaDirectSaleModel("4","101 "+Language.sqm + " -> 120 "+Language.sqm,"101","120"),
                new NetAreaDirectSaleModel("5","121 "+Language.sqm + " -> 150 "+Language.sqm,"121","150"),
                new NetAreaDirectSaleModel("6","151 "+Language.sqm + " -> 180 "+Language.sqm,"151","180"),
                new NetAreaDirectSaleModel("7","211 "+Language.sqm + " -> 240 "+Language.sqm,"211","240"),
                new NetAreaDirectSaleModel("8","241 "+Language.sqm + " -> 270 "+Language.sqm,"241","270"),
                new NetAreaDirectSaleModel("9","271 "+Language.sqm + " -> 300 "+Language.sqm,"271","300"),
                new NetAreaDirectSaleModel("10","301 "+Language.sqm + " -> 350 "+Language.sqm,"301","350"),
                new NetAreaDirectSaleModel("11",Language.string_more_than + " 350 "+Language.sqm,null,"350"),
            };
        }
        public static NetAreaDirectSaleModel GetNetAreaById(string Id)
        {
            return NetAreaData().SingleOrDefault(x => x.Id == Id);
        }
        #endregion
        #region Price
        public static List<PriceDirectSaleModel> PriceData()
        {
            return new List<PriceDirectSaleModel>()
            {
                new PriceDirectSaleModel("1",Language.string_under + " 1 " + Language.currency_billion,"1000000000"), //currency_billion //duoi
                new PriceDirectSaleModel("2","1 " + Language.currency_billion + " -> 2 " + Language.currency_billion,"1000000000","2000000000"),
                new PriceDirectSaleModel("3","2 " + Language.currency_billion + " -> 5 " + Language.currency_billion,"2000000000","5000000000"),
                new PriceDirectSaleModel("4","5 " + Language.currency_billion + " -> 10 " + Language.currency_billion,"5000000000","10000000000"),
                new PriceDirectSaleModel("5","10 " + Language.currency_billion + " -> 20 " + Language.currency_billion,"10000000000","20000000000"),
                new PriceDirectSaleModel("6","20 " + Language.currency_billion + " -> 50 " + Language.currency_billion,"20000000000","50000000000"),
                new PriceDirectSaleModel("7",Language.string_more_than + " 50 "+ Language.currency_billion,null,"50000000000") //hơn
                //string_under
                // string_more_than
            };
        }
        public static PriceDirectSaleModel GetPriceById(string Id)
        {
            return PriceData().SingleOrDefault(x => x.Id == Id);
        }
        #endregion
        #region Directions
        public static List<OptionSetFilter> DirectionsData()
        {
            var directions = new List<OptionSetFilter>();
            directions.Add(new OptionSetFilter { Val = "100000000", Label = Language.direction_east });//East
            directions.Add(new OptionSetFilter { Val = "100000001", Label = Language.direction_west });//West
            directions.Add(new OptionSetFilter { Val = "100000002", Label = Language.direction_south });//South
            directions.Add(new OptionSetFilter { Val = "100000003", Label = Language.direction_north });//North
            directions.Add(new OptionSetFilter { Val = "100000004", Label = Language.direction_north_west });//North West
            directions.Add(new OptionSetFilter { Val = "100000005", Label = Language.direction_north_east });//North East
            directions.Add(new OptionSetFilter { Val = "100000006", Label = Language.direction_south_west });//South West
            directions.Add(new OptionSetFilter { Val = "100000007", Label = Language.direction_south_east });//South East
            return directions;
        }
        public static OptionSet GetDiretionById(string diretionId)
        {
            var diretion = DirectionsData().Single(x => x.Val == diretionId);
            return diretion;
        }
        #endregion
        #region StatusCodeUnit
        public static StatusCodeModel GetStatusCodeById(string statusCodeId)
        {
            return StatusCodesData().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> StatusCodesData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0",Language.unit_draft_sts,"#333333"), //Draft unit_draft_sts
                new StatusCodeModel("1",Language.unit_preparing_sts,"#FDC206"),//Preparing unit_preparing_sts
                new StatusCodeModel("100000000",Language.unit_available_sts,"#06CF79"),//Available unit_available_sts
                new StatusCodeModel("100000004",Language.unit_queuing_sts,"#03ACF5"),//Queuing unit_queuing_sts
                new StatusCodeModel("100000006",Language.unit_reserve_sts,"#04A388"),//Reserve unit_reserve_sts
                new StatusCodeModel("100000005",Language.unit_collected_sts,"#9A40AB"),//Collected unit_collected_sts
                new StatusCodeModel("100000003",Language.unit_deposited_sts,"#FA7901"),//Deposited unit_deposited_sts
                new StatusCodeModel("100000001",Language.unit_1st_installment_sts,"#808080"),//1st Installment unit_1st_installment_sts
                new StatusCodeModel("100000002",Language.unit_sold_sts,"#D42A16"),//Sold unit_sold_sts
            };
        }
        #endregion
        #region
        #endregion
    }
}
