using System;
using System.ComponentModel.DataAnnotations;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Helpers
{
    public class Log
    {
        public int Id { get; set; }

		public int UserId { get; set; }
	    //public virtual DirectoryUser User { get; set; }

        public DateTime Date { get; set; }

        public string Application { get; set; }

        public string Message { get; set; }
    }
}