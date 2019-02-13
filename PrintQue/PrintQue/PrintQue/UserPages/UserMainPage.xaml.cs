﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void PrinterStatusButton_Clicked(object sender, EventArgs e)
        {

        }

        private void CreateRequestButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserSubmitRequestPage());
        }
    }
}