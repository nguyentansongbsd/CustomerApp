﻿using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSale : ContentPage
    {
        public DirectSaleViewModel viewModel;
        public DirectSale()
        {
            LoadingHelper.Show();
            InitializeComponent();
            this.BindingContext = viewModel = new DirectSaleViewModel();
            Init();
            LoadingHelper.Hide();
        }
        public async void Init()
        {
            lookupNetArea.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.NetAreas = Data.NetAreaData();
                LoadingHelper.Hide();
            };
            lookupPrice.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.Prices = Data.PriceData();
                LoadingHelper.Hide();
            };
            lookupMultipleDirection.PreShow = async () => {
                LoadingHelper.Show();
                viewModel.DirectionOptions = Data.DirectionsData();
                LoadingHelper.Hide();
            };
            lookupMultipleUnitStatus.PreShow = async () => {
                LoadingHelper.Show();
                var unitStatus = Data.StatusCodesData();
                unitStatus.RemoveAt(0);
                viewModel.UnitStatusOptions = new List<OptionSetFilter>();
                foreach (var item in unitStatus)
                {
                    viewModel.UnitStatusOptions.Add(new OptionSetFilter { Val = item.Id, Label = item.Name });
                }
                LoadingHelper.Hide();
            };

        }

        private async void LoadProject_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Projects.Count == 0)
            {
                await viewModel.LoadProject();
                listviewProject.ItemsSource = viewModel.Projects;
            }

            await bottomModalProject.Show();
            LoadingHelper.Hide();
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            listviewProject.ItemsSource = viewModel.Projects.Where(x => x.bsd_name.ToLower().Contains(searchProject.Text.Trim().ToLower()) || x.bsd_projectcode.ToLower().Contains(searchProject.Text.Trim().ToLower()));
            LoadingHelper.Hide();
        }

        private async void SearchBar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchProject.Text))
            {
                LoadingHelper.Show();
                viewModel.Projects.Clear();
                await viewModel.LoadProject();
                listviewProject.ItemsSource = viewModel.Projects;
                LoadingHelper.Hide();
            }
        }

        private async void ProjectItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as ProjectList;
            viewModel.Project = item;
            await Task.WhenAll(
                viewModel.LoadPhasesLanch()
                //viewModel.LoadBlocks()
                );
            await bottomModalProject.Hide();
            LoadingHelper.Hide();
        }

        private async void PhaseLaunchItem_SelectedChange(object sender, EventArgs e)
        {
            //LoadingHelper.Show();
            //await viewModel.LoadBlocks();
            //LoadingHelper.Hide();
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Project == null)
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_chon_du_an);
                LoadingHelper.Hide();
            }
            else
            {
                string directions = (viewModel.SelectedDirections != null && viewModel.SelectedDirections.Count != 0) ? string.Join(",", viewModel.SelectedDirections) : null;
                string unitStatus = (viewModel.SelectedUnitStatus != null && viewModel.SelectedUnitStatus.Count != 0) ? string.Join(",", viewModel.SelectedUnitStatus) : null;

                DirectSaleSearchModel filter = new DirectSaleSearchModel(viewModel.Project.bsd_projectid, viewModel.PhasesLaunch?.Val, viewModel.IsEvent, viewModel.UnitCode, directions, unitStatus, viewModel.NetArea?.Id, viewModel.Price?.Id);

                DirectSaleDetail directSaleDetail = new DirectSaleDetail(filter);//,viewModel.Blocks
                directSaleDetail.OnCompleted = async (Success) =>
                {
                    if (Success == 0)
                    {
                        await Navigation.PushAsync(directSaleDetail);
                        LoadingHelper.Hide();
                    }
                    else if (Success == 1)
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.LongMessage(Language.khong_co_san_pham);
                    }
                    else if (Success == 2)
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.LongMessage(Language.khong_co_san_pham);
                    }
                };
            }
        }
        private void ShowInfo(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Project == null)
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_chon_du_an);
                LoadingHelper.Hide();
                return;
            }

            ProjectInfoPage projectInfo = new ProjectInfoPage(Guid.Parse(viewModel.Project.bsd_projectid), viewModel.Project.bsd_name);
            projectInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess == true)
                {
                    await Navigation.PushAsync(projectInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    await DisplayAlert("", Language.noti_khong_tim_thay_du_an, Language.btn_dong);
                    LoadingHelper.Hide();
                }
            };
        }
        private void Clear_Clicked(object sender, EventArgs e)
        {
            viewModel.Project = null;
            viewModel.PhasesLaunch = null;
            viewModel.IsEvent = false;
            viewModel.UnitCode = null;
            viewModel.SelectedDirections = null;
            viewModel.SelectedUnitStatus = null;
            viewModel.NetArea = null;
            viewModel.Price = null;
        }
    }
}