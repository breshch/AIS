using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;

namespace AIS_Enterprise_Data.Infos
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

		private bool _isReturn;

		[NotMapped]
		public bool IsReturn
		{
			get
			{
				return _isReturn;
			}
			set
			{
				_isReturn = value;
				OnChangeIsReturn();
			}
		}

		public static event Action OnChangeIsReturn = delegate { };
	}
}
