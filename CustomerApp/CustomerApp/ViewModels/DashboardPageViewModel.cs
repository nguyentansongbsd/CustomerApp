﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class DashboardPageViewModel : BaseViewModel
    {
        public ObservableCollection<ChartModel> DataMonthQueue { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthQuote { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthOptionEntry { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthUnit { get; set; } = new ObservableCollection<ChartModel>();

        private LoyaltyModel _loyalty;
        public LoyaltyModel Loyalty { get => _loyalty; set { _loyalty = value;OnPropertyChanged(nameof(Loyalty)); } }

        private DateTime _dateBefor;
        public DateTime dateBefor { get => _dateBefor; set { _dateBefor = value; OnPropertyChanged(nameof(dateBefor)); } }
        public DateTime dateAfter { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

        private int _numQueue;
        public int numQueue { get => _numQueue; set { _numQueue = value; OnPropertyChanged(nameof(numQueue)); } }
        private int _numQuote;
        public int numQuote { get => _numQuote; set { _numQuote = value; OnPropertyChanged(nameof(numQuote)); } }
        private int _numOptionEntry;
        public int numOptionEntry { get => _numOptionEntry; set { _numOptionEntry = value; OnPropertyChanged(nameof(numOptionEntry)); } }
        private int _numUnit;
        public int numUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(numUnit)); } }

        public DateTime first_Month { get; set; }
        public DateTime second_Month { get; set; }
        public DateTime third_Month { get; set; }
        public DateTime fourth_Month { get; set; }

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            //await RefreshDashboard();
            IsRefreshing = false;
        });

        public DashboardPageViewModel()
        {
            dateBefor = DateTime.Now;
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            dateAfter = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
            first_Month = dateAfter;
            second_Month = dateAfter.AddMonths(1);
            third_Month = second_Month.AddMonths(1);
            fourth_Month = dateBefor;
        }

        public async Task LoadContactLoyalty()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='contactid' />
                                    <attribute name='bsd_totalamountofownership3years' />
                                    <attribute name='bsd_totalamountofownership' />
                                    <attribute name='bsd_loyaltystatus' />
                                    <attribute name='bsd_loyaltydate' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='bsd_membershiptier' from='bsd_membershiptierid' to='bsd_membershiptier' link-type='inner' alias='ad' >
                                        <attribute name='bsd_name' alias='membershiptier_name'/>
                                        <attribute name='bsd_membershiptierid' alias='membershiptier_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LoyaltyModel>>("contacts", fetchXml);
            if (result == null && result.value == null) return;
            this.Loyalty = (result.value as List<LoyaltyModel>).SingleOrDefault();
        }

        public async Task LoadQueueFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='opportunity'>
                                        <attribute name='bsd_bookingtime' alias='Date'/>
                                        <attribute name='opportunityid' alias='Id' />
                                            <filter type='and'>
                                                <condition attribute='bsd_bookingtime' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                                <condition attribute='statuscode' operator='in'>
                                                    <value>100000002</value>
                                                    <value>100000000</value>
                                                </condition>
                                                <condition attribute='parentcontactid' operator='eq' value='{UserLogged.Id}' />
                                            </filter>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("opportunities", fetchXml);
            if (result == null) return;

            var countQueueFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countQueueFr };

            var countQueueSe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countQueueSe };

            var countQueueTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countQueueTh };

            var countQueueFo = this.numQueue = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countQueueFo };

            this.DataMonthQueue.Add(chartFirstMonth);
            this.DataMonthQueue.Add(chartSecondMonth);
            this.DataMonthQueue.Add(chartThirdMonth);
            this.DataMonthQueue.Add(chartFourthMonth);
        }
        public async Task LoadQuoteFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='bsd_deposittime' alias='Date' />
                                    <attribute name='quoteid' alias='Id' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='in'>
                                        <value>3</value>
                                        <value>861450000</value>
                                        <value>4</value>
                                      </condition>
                                      <condition attribute='bsd_deposittime' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                    </filter>
                                    <link-entity name='contactquotes' from='quoteid' to='quoteid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='aa'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("quotes", fetchXml);
            if (result == null) return;

            var countQuoteFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countQuoteFr };

            var countQuoteSe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countQuoteSe };

            var countQuoteTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countQuoteTh };

            var countQuoteFo = this.numQuote = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countQuoteFo };

            this.DataMonthQuote.Add(chartFirstMonth);
            this.DataMonthQuote.Add(chartSecondMonth);
            this.DataMonthQuote.Add(chartThirdMonth);
            this.DataMonthQuote.Add(chartFourthMonth);
        }
        public async Task LoadOptionEntryFourMonths()
        {
            // ngoại trừ các sts Terminated , 1st Installment, Option, Qualify, Signed D.A
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='quoteid' alias='Id'/>
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='not-in'>
                                        <value>100000006</value>
                                        <value>100000001</value>
                                        <value>100000000</value>
                                        <value>100000007</value>
                                        <value>100000008</value>
                                      </condition>
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='bsd_signedcontractdate' operator='null' />
                                    </filter>
                                    <link-entity name='contactorders' from='salesorderid' to='salesorderid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='ab'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("salesorders", fetchXml);
            if (result == null) return;

            var countOptionEntryFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countOptionEntryFr };

            var countOptionEntrySe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countOptionEntrySe };

            var countOptionEntryTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countOptionEntryTh };

            var countOptionEntryFo = this.numOptionEntry = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countOptionEntryFo };

            this.DataMonthOptionEntry.Add(chartFirstMonth);
            this.DataMonthOptionEntry.Add(chartSecondMonth);
            this.DataMonthOptionEntry.Add(chartThirdMonth);
            this.DataMonthOptionEntry.Add(chartFourthMonth);
        }
        public async Task LoadUnitFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='productid' alias='Id'/>
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000002' />
                                    </filter>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' link-type='inner'>
	                                    <attribute name='bsd_signedcontractdate' alias='Date'/>
                                          <filter type='and'>
                                            <condition attribute='bsd_signedcontractdate' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                          </filter>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_customer' link-type='inner' alias='ac'>
                                      <filter type='and'>
                                        <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("products", fetchXml);
            if (result == null) return;

            var countUnitFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countUnitFr };

            var countUnitSe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countUnitSe };

            var countUnitTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countUnitTh };

            var countUnitFo = this.numUnit = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countUnitFo };

            this.DataMonthUnit.Add(chartFirstMonth);
            this.DataMonthUnit.Add(chartSecondMonth);
            this.DataMonthUnit.Add(chartThirdMonth);
            this.DataMonthUnit.Add(chartFourthMonth);
        }
    }
}