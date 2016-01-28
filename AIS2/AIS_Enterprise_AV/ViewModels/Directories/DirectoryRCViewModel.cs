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
			rcsName = BC.GetDirectoryRCs();

			RCsName = rcsName.Select(x => new RCFullName
			{
				Id	= x.Id,
				Name = x.Name + " / " + x.DescriptionName
			})
			.ToArray();

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
			Percentes = 0;
		}

		#endregion



		#region Properties

		public string TextWarning { get; set; }

		private DirectoryRC[] rcsName;

		public DateRC[] Dates { get; set; }
		public DateTime SelectedNewDate { get; set; }
		private DateRC selectedDate;

		public RCFullName[] RCsName { get; set; }
		public RCFullName SelectedRCName { get; set; }

		public DateRC SelectedDate
		{
			get { return selectedDate; }
			set
			{
				if (value == null)
					return;

				selectedDate = value;

				var rcPercentages = BC.GetRCPercentages(selectedDate.Date.Year, selectedDate.Date.Month);

				RCNamePercentages = rcPercentages
					.Select(x => new RCNamePercentage
					{
						Name = rcsName.First(r => r.Id == x.DirectoryRCId).Name,
						Company = rcsName.First(r => r.Id == x.DirectoryRCId).DescriptionName,
						Percentage = x.Percentage
					})
					.ToArray();

				int count = rcsName.Count() - RCNamePercentages.Count();

				var rcsId = rcPercentages.Select(x => x.DirectoryRCId).ToArray();
				SelectedRCName = RCsName.FirstOrDefault(x => !rcsId.Contains(x.Id)) ?? RCsName.First();

				MaximumPercentes = MAXIMUM_PERCENTAGE - RCNamePercentages.Sum(r => r.Percentage);

				TextWarning = "Осталось заполнить " + count + " ЦО и " + MaximumPercentes + "%";
			}
		}



		public RCNamePercentage[] RCNamePercentages { get; set; }

		public int Percentes { get; set; }

		public int MinimumPercentes { get; set; }
		public int MaximumPercentes { get; set; }

		#endregion


		#region Commands

		public RelayCommand AddCommand { get; set; }

		public void Add(object parameter)
		{
			bool hasRC = rcs.Any(x => x.Date.Date == SelectedNewDate.Date && x.DirectoryRCId == SelectedRCName.Id);
			if (!hasRC)
			{
				BC.AddDirectoryRC(SelectedRCName.Id, Percentes, SelectedNewDate);
			}
			else
			{
				BC.EditDirectoryRC(SelectedRCName.Id, Percentes, SelectedNewDate);
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