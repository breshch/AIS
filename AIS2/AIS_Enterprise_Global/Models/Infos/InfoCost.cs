using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Infos
{
    public class InfoCost
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int DirectoryCostItemId { get; set; }
        public virtual DirectoryCostItem DirectoryCostItem { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }

        public bool IsIncoming { get; set; }
        public double Summ { get; set; }

        public virtual List<CurrentNote> CurrentNotes { get; set; }

        public double Weight { get; set; }

        public int? DirectoryTransportCompanyId { get; set; }
        public virtual DirectoryTransportCompany DirectoryTransportCompany { get; set; }
       
        public InfoCost()
        {
            CurrentNotes = new List<CurrentNote>();
        }

        [NotMapped]
        public string ConcatNotes 
        { 
            get
            {
                string notes = "";

                for (int i = 0; i < CurrentNotes.Count; i++)
                {
                    notes += CurrentNotes[i].DirectoryNote.Description;

                    if (i != CurrentNotes.Count - 1)
                    {
                        notes += " + ";
                    }
                }

                if (Weight != 0)
                {
                    notes += " (" + Weight + " кг)";
                }

                return notes;
            }
        }

        [NotMapped]
        public double Incoming
        {
            get
            {
                return IsIncoming ? Summ : 0;
            }
        }

        [NotMapped]
        public double Expense
        {
            get
            {
                return !IsIncoming ? Summ : 0;
            }
        }
    }
}
