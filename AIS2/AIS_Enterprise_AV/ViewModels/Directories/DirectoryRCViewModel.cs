using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AIS_Enterprise_AV.Models;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels
{
	public class DirectoryRCViewModel : ViewModelGlobal
	{
		#region Base
		private const int MAXIMUM_PERCENTAGE = 100;

		public DirectoryRCViewModel()
			: base()
		{
			RefreshDirectoryRCs();

			AddCommand = new RelayCommand(Add, CanAdding);

			MinimumPercentes = 0;

			SelectedNewDate = DateTime.Now;
		}

		private void RefreshDirectoryRCs()
		{
			rcs = BC.GetAllRCPercentages();

			Dates = rcs
				.GroupBy(d => d.Date.Date)
				.Select(d => new DateRC
				{
					Date = d.Key,
					DatePlain = d.Key.ToString("dd.MM.yy")
				})
				.OrderByDescending(d => d.Date)
				.ToArray();

			SelectedDate = Dates[0];
		}

		private DirectoryRCPercentage[] rcs;

		private void ClearInputData()
		{
			DirectoryRCName = null;
			Percentes = 0;
		}

		#endregion



		#region Properties

		public DateRC[] Dates { get; set; }
		public DateTime SelectedNewDate { get; set; }
		private DateRC selectedDate;

		public DateRC SelectedDate
		{
			get { return selectedDate; }
			set
			{
				if (value == null)
					return;

				selectedDate = value;

				DirectoryRCs = rcs
					.Where(r => r.Date.Date == selectedDate.Date)
					.ToArray();

				//MaximumPercentes = MAXIMUM_PERCENTAGE - DirectoryRCs.Sum(r => r.Percentes);
			}
		}



		public DirectoryRCPercentage[] DirectoryRCs { get; set; }

		public DirectoryRC SelectedDirectoryRC { get; set; }

		[Required]
		[Display(Name = "Название ЦО")]
		public string DirectoryRCName { get; set; }

		public int Percentes { get; set; }

		public int MinimumPercentes { get; set; }
		public int MaximumPercentes { get; set; }

		[Required]
		[Display(Name = "Компания")]
		public string DescriptionName { get; set; }

		#endregion


		#region Commands

		public RelayCommand AddCommand { get; set; }

		public void Add(object parameter)
		{
			bool hasRC = rcs.Any(x => x.Date.Date == SelectedNewDate.Date && x.DirectoryRCId == DirectoryRCName);
			if (!hasRC)
			{
				BC.AddDirectoryRC(DirectoryRCName, DescriptionName, Percentes, SelectedNewDate);
			}
			else
			{
				BC.EditDirectoryRC(DirectoryRCName, DescriptionName, Percentes, SelectedNewDate);
			}
			

			RefreshDirectoryRCs();

			ClearInputData();
		}

		public bool CanAdding(object parameter)
		{
			return IsValidateAllProperties();
		}


		#endregion
	}
}