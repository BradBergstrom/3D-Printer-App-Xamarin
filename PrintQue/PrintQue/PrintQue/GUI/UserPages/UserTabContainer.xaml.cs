﻿using PrintQue.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrintQue
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserTabContainer : TabbedPage
	{

		public UserTabContainer ()
		{
			InitializeComponent ();

		}
        async private void ToolbarItem_Run_Activated(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "You are about to logout. Are you sure?", "Yes", "No");
            if (response)
            {
                App.LoggedInUser = null;
                await Navigation.PopAsync();
            }
        }

    }
}