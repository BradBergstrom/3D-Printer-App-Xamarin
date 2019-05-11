﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintQue.GUI.DetailPages;
using PrintQue.Helper;
using PrintQue.Models;
using PrintQue.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrintQue
{


[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserMainPage : ContentPage
	{


		public UserMainPage ()
		{
			InitializeComponent ();

        }

        private ObservableCollection<Printer> _printers;
        private bool isDataLoaded;
        public async void RefreshPrinterListView()
        {
            var pri = await Printer.GetAll();
            _printers = new ObservableCollection<Printer>(pri);
            PrinterListView.ItemsSource = _printers;
        }
        public async void SyncOfflineDatabase()
        {
            await AzureAppServiceHelper.SyncAsync();
        }
        protected override void OnAppearing()
        {
            if (isDataLoaded)
                return;
            isDataLoaded = true;
            RefreshPrinterListView();
            base.OnAppearing();

        }

        async private void PrinterListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var prichild = e.SelectedItem as Printer;
            string action = await DisplayActionSheet("What would you like to know about the Printer?"
                , "Projects Queued"
                , "Printer Color");
            switch(action)
            {
                case "Projects Queued":
                    await DisplayAlert("Projects Queued"
                        , "Here the number of projects queued for this printer today: " + prichild.Requests.Count, "OK");
                    break;
                case "Printer Color":
                    await DisplayAlert("Printer Color"
                        , "This printer only prints in " + prichild.PrintColor.Name, "OK");
                    break;
            }
            PrinterListView.SelectedItem = null;
        }

        private void  PrinterListView_Refreshing(object sender, EventArgs e)
        {
            RefreshPrinterListView();
            PrinterListView.IsRefreshing = false;
            PrinterListView.EndRefresh();
        }
        private void CreateRequestButton_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as Button;
            var selectedItem = menuItem.CommandParameter as Printer;
            var request = new Request()
            {
                Printer = selectedItem,
                PrinterID = selectedItem.ID,
                User = User.SearchByID(App.LoggedInUserID).Result,
                UserID = App.LoggedInUserID,
            };
            Navigation.PushAsync(new RequestDetailPage(request, 1));
        }
        
    }
}