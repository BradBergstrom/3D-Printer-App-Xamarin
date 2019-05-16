﻿using PrintQue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintQue.ViewModel
{
    public class RequestViewModel : Request
    {
       
        public Printer Printer { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
        public static async Task Insert(RequestViewModel requestViewModel)
        {
            var request = ReturnRequest(requestViewModel);
            await App.MobileService.GetTable<Request>().InsertAsync(request);
        }

        private static Request ReturnRequest(RequestViewModel requestViewModel)
        {
            var request = new Request()
            {
                ID = requestViewModel.ID,
                PrinterID = requestViewModel.PrinterID,
                StatusID = requestViewModel.StatusID,
                UserID = requestViewModel.UserID,
                DateMade = requestViewModel.DateMade,
                DateRequested = requestViewModel.DateRequested,
                Duration = requestViewModel.Duration,
                ProjectName = requestViewModel.ProjectName,
                Description = requestViewModel.Description,
                File = requestViewModel.File,
                Personal = requestViewModel.Personal,

            };
            return request;
        }
        private static RequestViewModel ReturnRequestViewModel(Request request)
        {
            var requestViewModel = new RequestViewModel()
            {
                ID = request.ID,
                PrinterID = request.PrinterID,
                StatusID = request.StatusID,
                UserID = request.UserID,
                DateMade = request.DateMade,
                DateRequested = request.DateRequested,
                Duration = request.Duration,
                ProjectName = request.ProjectName,
                Description = request.Description,
                File = request.File,
                Personal = request.Personal,

            };
            return requestViewModel;
        }
        private static async Task<RequestViewModel> GetForeignKeys(RequestViewModel requestViewModel)
        {
            if (requestViewModel.PrinterID != null)
                requestViewModel.Printer = await PrinterViewModel.SearchByID(requestViewModel.PrinterID);
            if (requestViewModel.StatusID != null)
                requestViewModel.Status = await StatusViewModel.SearchByID(requestViewModel.StatusID);
            if (requestViewModel.UserID != null)
                requestViewModel.User = await UserViewModel.SearchByID(requestViewModel.UserID);
            if (requestViewModel.ID != null)
                requestViewModel.Messages = await MessageViewModel.SearchByRequestID(requestViewModel.ID);

            return requestViewModel;
        }
        //This function gets all requests in the SQLite Database
        public static async Task<List<RequestViewModel>> GetAll()
        {
            List<Request> requests = new List<Request>();
            List<RequestViewModel> requestviewmodel = new List<RequestViewModel>();
            requests = await App.MobileService.GetTable<Request>().ToListAsync();
            foreach (var req  in requests)
            {
                var request = ReturnRequestViewModel(req);
                requestviewmodel.Add(request);
            }
            return requestviewmodel;
        }

        public static async Task Remove(RequestViewModel requestViewModel)
        {
            var request = ReturnRequest(requestViewModel);
            await App.MobileService.GetTable<Request>().DeleteAsync(request);
        }

        public static async Task<int> Update(RequestViewModel requestViewModel)
        {
            var request = ReturnRequest(requestViewModel);
            var test = await SearchByID(requestViewModel.ID);
            if (test != null)
            {
                await App.MobileService.GetTable<Request>().UpdateAsync(request);
                return 1;
            }
            else
            {
                return 0;
            }

        }
        private static List<RequestViewModel> ReturnListRequestViewModel(List<Request> list)
        {
            List<RequestViewModel> trans = new List<RequestViewModel>();
            foreach (var item in list)
            {
                trans.Add(ReturnRequestViewModel(item));
            }
            return trans;
        }
        //This function sorts the requests by statusID
        public static async Task<List<RequestViewModel>> SortByStatus(string searchText = null)
        {
            var status = await StatusViewModel.SearchByName(searchText);
            List<Request> sortedRequests = await App.MobileService.GetTable<Request>().Where(sr => sr.StatusID.Contains(status.ID)).ToListAsync();
            
            return ReturnListRequestViewModel(sortedRequests);

        }

        //Useless??
        public static async Task<RequestViewModel> SearchByName(string searchText = null)
        {
            Request sortedRequests = (await App.MobileService.GetTable<Request>().Where(sr => sr.ProjectName.Contains(searchText)).ToListAsync()).FirstOrDefault();
            return ReturnRequestViewModel(sortedRequests);

        }
        public static async Task<List<RequestViewModel>> SearchByPrinter(PrinterViewModel printerViewModel)
        {
            List<Request> sortedRequests = (await App.MobileService.GetTable<Request>().Where(sr => sr.PrinterID.Contains(printerViewModel.ID)).ToListAsync());
            return ReturnListRequestViewModel(sortedRequests);


        }
        public static async Task<RequestViewModel> SearchByID(string ID)
        {
            Request sortedRequests = (await App.MobileService.GetTable<Request>().Where(sr => sr.ID.Contains(ID)).ToListAsync()).FirstOrDefault();
            return ReturnRequestViewModel(sortedRequests);

        }
        public static async Task<RequestViewModel> SearchProjectNameByUser(RequestViewModel requestViewModel)
        {
            var sortedRequests = (await App.MobileService.GetTable<Request>().Where(r => r.UserID == requestViewModel.UserID && r.ProjectName.Contains(requestViewModel.ProjectName)).ToListAsync()).FirstOrDefault();
            return ReturnRequestViewModel(sortedRequests);
        }
    }
}
