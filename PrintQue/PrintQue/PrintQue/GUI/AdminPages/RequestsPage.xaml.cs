﻿
using PrintQue.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using PrintQue.GUI.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace PrintQue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage : ContentPage
    {
        private ObservableCollection<Request> _requests;
        private bool isDataLoaded;
        public RequestsPage ()
		{
			InitializeComponent ();
            
        }

        protected override void OnAppearing()
        {

            if (isDataLoaded)
                return;
            isDataLoaded = true;
            RefreshRequestsView();

            base.OnAppearing();
        }

        public async void RefreshRequestsView()
        {
            var req = await Request.GetAll();


            _requests = new ObservableCollection<Request>(req);
            RequestListView.ItemsSource = _requests;
        }

        public void Clicked_Approve(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var request = menuItem.CommandParameter as Request;
            DisplayAlert("Approved", request.ProjectName, "OK");
        }

        public void Clicked_Deny(object sender, EventArgs e)
        {
            var request = (sender as MenuItem).CommandParameter as Request;
            DisplayAlert("Denied", request.ProjectName, "OK");
        }

        private void RequestListView_Refreshing(object sender, EventArgs e)
        {
            RefreshRequestsView();
            RequestListView.ItemsSource = _requests;
            RequestListView.IsRefreshing = false;
            RequestListView.EndRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshRequestsView();
            
            RequestListView.ItemsSource = _requests.Where(r => r.ProjectName.Contains(e.NewTextValue) 
                || r.user.Name.Contains(e.NewTextValue));

        }

        async private void RequestListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var request = e.SelectedItem as Request;
            request = await Request.SearchByName(request.ProjectName);
            await Navigation.PushAsync(new RequestDetailPage(request));
            RequestListView.SelectedItem = null;
        }
    }
}