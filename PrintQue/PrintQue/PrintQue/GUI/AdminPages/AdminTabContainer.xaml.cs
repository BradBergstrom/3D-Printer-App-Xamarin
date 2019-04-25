﻿
using PrintQue.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PrintQue.GUI.DetailPages;

namespace PrintQue
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminTabContainer : TabbedPage
	{



        private List<PrintColor> printColors = new List<PrintColor>()
        {
            new PrintColor()
            {
                Name = "Red",
                HexValue = "#ff0000",
            },
            new PrintColor()
            {
                Name = "Green",
                HexValue = "#008000",
            },
            new PrintColor()
            {
                Name = "Orange",
                HexValue = "#FFA500",
            },
        };

        public List<Status> statuses = new List<Status>()
        {
            new Status()
            {
                Name = "Approved",
            },
            new Status()
            {
                Name = "Denied",
            },
            new Status()
            {
                Name = "nostatus",
            },
            new Status()
            {
                Name = "Busy",
            },
            new Status()
            {
                Name = "Open",
            },
            new Status()
            {
                Name = "Closed",
            },
        };
        private List<Printer> printers = new List<Printer>()
        {
            new Printer() {
                Name = "Demilovato",
                
            },
            new Printer() {
                Name = "Prince",

            },
            new Printer() {
                Name = "Corpus",

            },
        };
        private List<User> users = new List<User>()
        {
            new User()
            {
                Email = "drew.doser@gmail.com",
                Password = "1234",
                Name = "Andrew",
                Admin = 0,
            },
            new User()
            {
                Email = "Brad.Bergstrom@gmail.com",
                Password = "1234",
                Name = "Brad",
                Admin = 0,
            },

        };

		public AdminTabContainer ()
		{
			InitializeComponent ();

        }

        private void ToolbarItem_Plus_Activated(object sender, EventArgs e)
        {
            var request = new Request();
            request = null;
            Navigation.PushAsync(new RequestDetailPage(request));
        }

        private void ToolbarItem_Add_Printer_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrinterDetailPage());
        }

        private void ToolbarItem_Add_Request_Activated(object sender, EventArgs e)
        {
            var request = new Request();
            request = null;
            Navigation.PushAsync(new RequestDetailPage(request));
        }

        async private void ToolbarItem_Run_Activated(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "You are about to logout. Are you sure?", "Yes", "No");
            if(response)
                await Navigation.PopAsync();

        }

        private async void ToolbarItem_Drop_Tables_Activated(object sender, EventArgs e)
        {



            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(App.DatabaseLocation);
            
                
            await conn.DropTableAsync<Printer>();
            await conn.DropTableAsync<User>();
            await conn.DropTableAsync<Request>();
            await conn.DropTableAsync<PrintColor>();
            await conn.DropTableAsync<Status>();
            
            
        }

        async private void ToolbarItem_Add_Color_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PrintColorDetailPage());

        }

        async private void ToolbarItem_Add_Status_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatusDetailPage());

        }
    }
}