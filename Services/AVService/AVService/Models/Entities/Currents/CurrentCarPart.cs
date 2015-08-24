using System;
using System.ComponentModel.DataAnnotations.Schema;
using AVService.Models.Entities.Directories;
using Shared.Enums;

namespace AVService.Models.Entities.Currents
{
    public class CurrentCarPart
    {
        public int Id { get; set; }

        public int DirectoryCarPartId { get; set; }
        public virtual DirectoryCarPart DirectoryCarPart { get; set; }
        
        public DateTime Date { get; set; }
        public Currency Currency { get; set; }
        public double PriceBase { get; set; }
        public double? PriceBigWholesale { get; set; }
        public double? PriceSmallWholesale { get; set; }

        private string _fullName;

	    [NotMapped]
	    public string FullName { get; set; }

	    //public string FullName //TODO Refactor
		//{
		//    get { return _fullName; }
		//    set { _fullName = value; }
		//}
	}
}
