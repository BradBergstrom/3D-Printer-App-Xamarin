﻿using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using PrintQue.GUI.SelectorPages;
using PrintQue.GUI.UserPages;
using PrintQue.Models;
using PrintQue.Widgets.CalendarWidget;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrintQue.GUI.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestDetailPage : ContentPage
	{
        private DateTime _dateTimeRequestSet;
        private Request  _request;


        public RequestDetailPage (Request request)
		{

			InitializeComponent ();
            
            if (request == null)
            {
                ToolbarItems.RemoveAt(1);
                ToolbarItems.RemoveAt(1);
            }
            else
            {
                
                if(request.printer != null)
                {
                    ToolbarItems.RemoveAt(1);
                    ToolbarItems.RemoveAt(1);
                }
            }
            BindingContext = request;
            _request = request;


        }
        private async void SelectFile_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();

                // User cancelled file selection
                if (fileData == null)
                    return;
                _request.File = JsonConvert.SerializeObject(fileData);
                SelectedFileLabel.Text = fileData.FileName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }
        private void ToolbarItem_Message_Activated(object sender, EventArgs e)
        {
            DisplayAlert("Message Clicked!", "W00t!", "OK");
        }
        private void ToolbarItem_Delete_Activated(object sender, EventArgs e)
        {
            DisplayAlert("Delete Clicked!", "W00t!", "OK");
        }
        private async void ToolbarItem_Save_Activated(object sender, EventArgs e)
        {
            var exists = await Request.SearchByName(ent_ProjectName.Text);
            if (exists == null)
            {

                var user =      await User.SearchByEmail(Users_Picker.Text);
                var printer =   await Printer.SearchByName(Printers_Picker.Text);
                var status =    await Status.SearchByName(Status_Picker.Text);
                var request = new Request()
                {
                    ProjectName = ent_ProjectName.Text,
                    //File = _request.File,
                    DateRequested = new DateTime(_dateTimeRequestSet.Year, _dateTimeRequestSet.Month, _dateTimeRequestSet.Day),
                    Duration = Convert.ToInt32(lbl_sli_duration.Text),
                    DateMade = DateTime.Now,
                    UserID = user.ID,
                    user = user,
                    PrinterID = printer.ID,
                    printer = printer,
                    StatusID = status.ID,
                    status = status,
                    Personal = PersonalUse_Picker.Text,
                    Description = edi_Description.Text,
                };
                await Request.Insert(request);

                await Navigation.PopAsync();

            }
            else
            {
                
                await DisplayAlert("ERROR", "Project Name already Used. Please choose another", "OK");

            }
        }
        private void OnDateTimeSubmitted(DateTime datetime)
        {
            _dateTimeRequestSet = datetime;
            PrintTimeLabel.Text = "Print Time: " + datetime.ToString();
        }
        private async void ScheduleDay_Clicked(object sender, EventArgs e)
        {
            var page = new UserScheduleDateTimePage();

            page.OnDateTimeSubmitted += OnDateTimeSubmitted;

            await Navigation.PushAsync(page);
        }
        async void Printer_Selector_Tapped(object sender, EventArgs e)
        {
            var page = new PrinterSelectorPage();
            page.PrinterNames.ItemSelected += (source, args) =>
            {
                Printers_Picker.Text = args.SelectedItem.ToString();
                Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }
        async void User_Selector_Tapped(object sender, EventArgs e)
        {
            var page = new UserSelectorPage();
            page.UserNames.ItemSelected += (source, args) =>
            {
                Users_Picker.Text = args.SelectedItem.ToString();
                Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
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
        
        async void PersonalUse_Selector_Tapped(object sender, EventArgs e)
        {
            var page = new PersonalUseSelector();
            page.PersonalUse.ItemSelected += (source, args) =>
            {
                PersonalUse_Picker.Text = args.SelectedItem.ToString();
                Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }
    }
}