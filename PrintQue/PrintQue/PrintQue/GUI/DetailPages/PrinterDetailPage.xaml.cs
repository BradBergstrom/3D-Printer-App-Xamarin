﻿using PrintQue.GUI.SelectorPages;
using PrintQue.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrintQue.GUI.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrinterDetailPage : ContentPage
    {

        public PrinterDetailPage()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            
        }
        async void Status_Selector_Tapped(object sender, EventArgs e)
        {
            var page = new StatusSelectorPage();
            page.StatusNames.ItemSelected += (source, args) =>
            {
                Status_Picker.Text = args.SelectedItem.ToString();
                Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }
        async void Color_Selector_Tapped(object sender, EventArgs e)
        {
            var page = new ColorSelectorPage();
            page.ColorNames.ItemSelected += (source, args) =>
            {
                Color_Picker.Text = args.SelectedItem.ToString();
                Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }
        async private void ToolbarItem_Save_Activated(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "Are you sure you want to Create this Printer?", "Yes", "No");
            if (response)
            {
                var exists = Printer.SearchByName(ent_Name.Text);
                if (exists != null)
                {
                    await DisplayAlert("ERROR", "Name already Used. Please choose another", "OK");
                }
                else
                {
                    var status = await Status.SearchByName(Status_Picker.Text);
                    var printColor = await PrintColor.SearchByName(Color_Picker.Text);
                    var printer = new Printer()
                    {
                        Name = ent_Name.Text,
                        StatusID = status.ID,
                        ColorID = printColor.ID,
                        ProjectsQueued = 0,
                    };

                    var rows = await Printer.Insert(printer);
                    if (rows > 0)
                    {
                        await DisplayAlert("Success!", "Printer was successfully save!", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Failure", "Printer was not saved!", "OK");

                    }
                }


            }

        }
    }
}