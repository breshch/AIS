using System;
using System.ComponentModel.DataAnnotations;

namespace AVService.Models.Entities.Helpers
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [MaxLength(64)]
        public string Level { get; set; }

        [MaxLength(256)]
        public string Logger { get; set; }

        [MaxLength(4096)]
        public string Description { get; set; }
    }
}