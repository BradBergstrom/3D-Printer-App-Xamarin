﻿using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintQue.Models
{
    public class Status
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Unique,NotNull,MaxLength(10)]
        public string Name { get; set; }
        public static async Task<int> Insert(Status status)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(App.DatabaseLocation);

            var rows = await conn.InsertAsync(status);
            await conn.CloseAsync();
            return rows;
        }
        public static async Task<List<Status>> GetStatuses()
        {
            List<Status> statuses = new List<Status>();
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(App.DatabaseLocation);
            
            statuses = await conn.Table<Status>().ToListAsync();

            await conn.CloseAsync();
            return statuses;
        }
        public static async Task<Status> SearchStatuses(string searchText = null)
        {
            List<Status> statuses = await GetStatuses();

            return statuses.FirstOrDefault(g => g.Name == searchText);
        }

    }
}
