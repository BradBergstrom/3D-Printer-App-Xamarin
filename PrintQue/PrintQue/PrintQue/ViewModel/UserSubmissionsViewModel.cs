﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PrintQue.ViewModel
{
    public class UserSubmissionsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RequestViewModel> requests { get; set; } = new ObservableCollection<RequestViewModel>();
        private bool _refreshList;
        public bool RefreshList
        {
            get { return _refreshList; }
            set
            {
                _refreshList = value;
                OnPropertyChanged("RefreshList");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserSubmissionsViewModel()
        {
            //UpdateRequestsList();
        }
        public async void UpdateRequestsList()
        {
            RefreshList = true;
            var temp = await RequestViewModel.SearchByUser(App.LoggedInUser.ID);

            if (temp != null)
            {
                requests.Clear();
                foreach (var req in temp)
                {
                    if (req.PrinterId != null)
                        req.Printer = await PrinterViewModel.SearchByID(req.PrinterId);
                    if (req.StatusId != null)
                        req.Status = await StatusViewModel.SearchByID(req.StatusId);
                    if (req.ApplicationUserId != null)
                        req.User = await UserViewModel.SearchByID(req.ApplicationUserId);
                    if (req.Id != null)
                        req.Messages = await MessageViewModel.SearchByRequestID(req.Id);
                    requests.Add(req);
                }
            }
            RefreshList = false;
        }

    }
}
