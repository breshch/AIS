using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AVService.Models.Entities.Currents;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Temps;

namespace AVService.Models.Entities.Infos
{
    public class InfoCost : IncomingAndExpenseAndSumm
    {
        public int Id { get; set; }

        public Guid GroupId { get; set; }

        public DateTime Date { get; set; }

        public int DirectoryCostItemId { get; set; }
        public virtual DirectoryCostItem DirectoryCostItem { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }
        public virtual List<CurrentNote> CurrentNotes { get; set; }

        public double Weight { get; set; }

        public int? DirectoryTransportCompanyId { get; set; }
        public virtual DirectoryTransportCompany DirectoryTransportCompany { get; set; }

        public InfoCost()
        {
            CurrentNotes = new List<CurrentNote>();
        }

	    [NotMapped]
	    public string ConcatNotes { get; set; }

	    //public string ConcatNotes //TODO Refactor
		//{
		//    get
		//    {
		//        string notes = "";

		//        for (int i = 0; i < CurrentNotes.Count; i++)
		//        {
		//            notes += CurrentNotes[i].DirectoryNote.Description;

		//            if (i != CurrentNotes.Count - 1)
		//            {
		//                notes += " + ";
		//            }
		//        }

		//        if (Weight != 0)
		//        {
		//            notes += " (" + Weight + " кг)";
		//        }

		//        return notes;
		//    }
		//}

		[NotMapped]
        public bool IsReturn { get; set; }
    }
}
