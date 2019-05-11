﻿using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using SQLiteNetExtensionsAsync.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PrintQue.Models
{
    public class User : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }



        
        private string email;
        [MaxLength(50), Unique]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }



        [MaxLength(50)]
        public string Name { get; set; }
        public int Admin { get; set; }
        
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Message> Messages { get; set; } = new List<Message>();


        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Request> Requests { get; set; } = new List<Request>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public static async Task<int> Login(string email, string password)
        {
            bool isUsernameEmpty = string.IsNullOrEmpty(email);
            bool isPasswordEmpty = string.IsNullOrEmpty(password);
            if (isUsernameEmpty || isPasswordEmpty)
            {
                //then show error
                return 0;
            }
            else
            {
                //admin
                if (email.Equals("admin"))
                {
                    // TODO(VorpW): Assign App.LoggedInUserID when an admin logs in
                    return 1;
                }
                else
                {

                    var user = await SearchByEmail(email.ToString());
                    if (user != null)
                    {
                        if (user.Password.Contains(password.ToString()))
                        {
                            App.LoggedInUserID = user.ID;
                            return 2;
                        }
                    }



                }
                return 0;

            }
        }
        public static async Task<int> Insert(User user)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(App.DatabaseLocation);

            var rows = await conn.InsertAsync(user);
            
            return rows;
        }
        
        public static async Task<List<User>> GetAll()
        {
            List<User> users = new List<User>();
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(App.DatabaseLocation);
            users = await conn.GetAllWithChildrenAsync<User>();            
            return users;
        }
        public static async Task<User> SearchByID(int ID)
        {
            List<User> users = await GetAll();
            return users.FirstOrDefault(u => u.ID == ID);

        }
        public static async Task<User> SearchByEmail(string email)
        {
            List<User> users = await GetAll();
            return users.FirstOrDefault(u => u.Email.Contains(email));

        }

    }
}
