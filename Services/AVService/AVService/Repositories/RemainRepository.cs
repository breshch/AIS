using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using AVService.Models.Entities.Currents;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Infos;
using AVService.Models.Entities.Temps;
using EntityFramework.BulkInsert.Extensions;
using Shared.Enums;

namespace AVService.Repositories
{
	public class RemainRepository : BaseRepository
	{
		private readonly UtilRepository utilRepository;

		public RemainRepository(UtilRepository utilRepository)
		{
			this.utilRepository = utilRepository;
		}

		#region DirectoryCarPart

		public DirectoryCarPart AddDirectoryCarPart(string article, string mark, string description, string originalNumber,
			string factoryNumber, string crossNumber, string material, string attachment, string countInBox, bool isImport)
		{
			using (var db = GetContext())
			{
				var carPart = new DirectoryCarPart
				{
					Article = article,
					Mark = mark,
					Description = description,
					OriginalNumber = originalNumber,
					Material = material,
					Attachment = attachment,
					FactoryNumber = factoryNumber,
					CrossNumber = crossNumber,
					CountInBox = countInBox,
					IsImport = isImport
				};

				db.DirectoryCarParts.Add(carPart);
				db.SaveChanges();

				return carPart;
			}
		}

		public DirectoryCarPart[] GetDirectoryCarParts()
		{
			using (var db = GetContext())
			{
				return db.DirectoryCarParts.OrderBy(c => c.Article).ToArray();
			}
		}

		public DirectoryCarPart GetDirectoryCarPart(string article, string mark)
		{
			using (var db = GetContext())
			{
				return db.DirectoryCarParts.FirstOrDefault(c => c.Article == article && c.Mark == mark);
			}
		}

		#endregion


		#region InfoContainer

		public int[] GetContainerYears(bool isIncoming)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Where(c => c.IsIncoming == isIncoming)
					.Select(c => c.DatePhysical.Year)
					.Distinct()
					.OrderBy(c => c)
					.ToArray();
			}
		}

		public int[] GetContainerMonthes(int selectedYear, bool isIncoming)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Where(c => c.IsIncoming == isIncoming && c.DatePhysical.Year == selectedYear)
					.Select(c => c.DatePhysical.Month)
					.Distinct()
					.OrderBy(c => c)
					.ToArray();
			}
		}

		public InfoContainer[] GetContainers(int year, int month, bool isIncoming)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Where(
					c => c.IsIncoming == isIncoming && c.DatePhysical.Year == year && c.DatePhysical.Month == month)
					.OrderBy(c => c.DatePhysical)
					.ToArray();
			}
		}

		public InfoContainer[] GetInfoContainers(List<InfoContainer> containers)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.ToList()
					.Where(c => containers.Any(c2 => c.Name == c2.Name && c.DatePhysical.Date == c2.DatePhysical.Date
													 && c.Description == c2.Description && c.IsIncoming == c2.IsIncoming))
					.Select(c => new InfoContainer
					{
						Id = c.Id,
						Name = c.Name,
						DatePhysical = c.DatePhysical,
						Description = c.Description,
						IsIncoming = c.IsIncoming,
						CarParts = containers.First(c2 => c.Name == c2.Name && c.DatePhysical.Date == c2.DatePhysical.Date
														  && c.Description == c2.Description && c.IsIncoming == c2.IsIncoming).CarParts
					})
					.ToArray();
			}
		}

		public InfoContainer GetInfoContainer(int containerId)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Include(c => c.CarParts).First(c => c.Id == containerId);
			}
		}

		public void RemoveInfoContainer(InfoContainer container)
		{
			using (var db = GetContext())
			{
				var carParts = db.InfoContainers.Find(container.Id).CarParts.ToArray();
				db.CurrentContainerCarParts.RemoveRange(carParts);

				db.InfoContainers.Remove(container);
				db.SaveChanges();
			}
		}

		public InfoContainer AddInfoContainer(string name, string description, DateTime datePhysical, DateTime? dateOrder,
			bool isIncoming, List<CurrentContainerCarPart> carParts)
		{
			using (var db = GetContext())
			{
				var container = new InfoContainer
				{
					Name = name,
					Description = description,
					DatePhysical = datePhysical,
					DateOrder = dateOrder,
					IsIncoming = isIncoming,
					CarParts = carParts.Select(c => new CurrentContainerCarPart
					{
						DirectoryCarPartId = c.DirectoryCarPartId,
						CountCarParts = c.CountCarParts,
					}).ToList()
				};

				db.InfoContainers.Add(container);
				db.SaveChanges();

				return container;
			}
		}

		public void EditInfoContainer(int containerId, string name, string description, DateTime datePhysical,
			DateTime? dateOrder, bool isIncoming, List<CurrentContainerCarPart> carParts)
		{
			using (var db = GetContext())
			{
				var container = db.InfoContainers.Find(containerId);
				container.Name = name;
				container.Description = description;
				container.DatePhysical = datePhysical;
				container.DateOrder = dateOrder;
				container.IsIncoming = isIncoming;

				db.SaveChanges();

				container.CarParts.Clear();
				container.CarParts = carParts;

				db.SaveChanges();
			}
		}

		public InfoCarPartMovement[] GetMovementsByDates(DirectoryCarPart selectedDirectoryCarPart,
			DateTime selectedDateFrom, DateTime selectedDateTo)
		{
			using (var db = GetContext())
			{
				return (from container in db.InfoContainers
						where
							DbFunctions.DiffDays(selectedDateFrom, container.DatePhysical) >= 0 &&
							DbFunctions.DiffDays(container.DatePhysical, selectedDateTo) >= 0
						join carPart in db.CurrentContainerCarParts on container.Id equals carPart.InfoContainerId
						where carPart.DirectoryCarPartId == selectedDirectoryCarPart.Id
						orderby container.DatePhysical descending
						select new InfoCarPartMovement
						{
							Date = container.DatePhysical,
							FullDescription = container.Name + " " + container.Description,
							Incoming = container.IsIncoming ? carPart.CountCarParts : default(int?),
							Outcoming = !container.IsIncoming ? carPart.CountCarParts : default(int?)
						})
					.ToArray();
			}
		}

		public void RemoveContainers(int year, int month)
		{
			using (var db = GetContext())
			{
				var removingContainers = db.InfoContainers
					.Where(c => c.DatePhysical.Year == year && c.DatePhysical.Month == month).ToArray();

				if (removingContainers.Any())
				{
					db.InfoContainers.RemoveRange(removingContainers);
					db.SaveChanges();
				}
			}
		}

		#endregion


		#region InfoLastMonthDayRemain

		public InfoLastMonthDayRemain GetInfoLastMonthDayRemain(DateTime date, int carPartId)
		{
			using (var db = GetContext())
			{
				return db.InfoLastMonthDayRemains.FirstOrDefault(p => p.DirectoryCarPartId == carPartId &&
																	  (p.Date.Year == date.Year && p.Date.Month == date.Month));
			}
		}

		public int GetInfoCarPartIncomingCountTillDate(DateTime date, int carPartId, bool isIncoming)
		{
			using (var db = GetContext())
			{
				DateTime firstDateInMonth = new DateTime(date.Year, date.Month, 1);
				var containers = db.InfoContainers.Include(c => c.CarParts).Where(c => c.IsIncoming == isIncoming &&
																					   (DbFunctions.DiffDays(firstDateInMonth,
																						   c.DatePhysical) >= 0 &&
																						DbFunctions.DiffDays(c.DatePhysical, date) >=
																						0)).ToList();

				return containers.Sum(c => c.CarParts.Where(p => p.DirectoryCarPartId == carPartId).Sum(c2 => c2.CountCarParts));
			}
		}

		public InfoLastMonthDayRemain AddInfoLastMonthDayRemain(DirectoryCarPart carPart, DateTime date, int count)
		{
			using (var db = GetContext())
			{
				var lastMonthDayRemain = new InfoLastMonthDayRemain
				{
					DirectoryCarPart = carPart,
					Count = count,
					Date = date,
				};
				db.InfoLastMonthDayRemains.Add(lastMonthDayRemain);
				db.SaveChanges();

				return lastMonthDayRemain;
			}
		}

		public void RemoveInfoLastMonthDayRemains(int year, int month)
		{
			using (var db = GetContext())
			{
				db.InfoLastMonthDayRemains
					.RemoveRange(db.InfoLastMonthDayRemains
						.Where(r => r.Date.Year == year && r.Date.Month == month));

				db.SaveChanges();
			}
		}

		#endregion


		#region CurrentCarParts

		public CurrentCarPart AddCurrentCarPart(DirectoryCarPart directoryCarPart, DateTime priceDate, double priceBase,
			double? priceBigWholesale, double? priceSmallWholesale)
		{
			using (var db = GetContext())
			{
				var currentCarPart = new CurrentCarPart
				{
					DirectoryCarPart = directoryCarPart,
					Date = priceDate,
					PriceBase = priceBase,
					PriceBigWholesale = priceBigWholesale,
					PriceSmallWholesale = priceSmallWholesale,
				};

				db.CurrentCarParts.Add(currentCarPart);
				db.SaveChanges();

				return currentCarPart;
			}
		}

		public CurrentCarPart AddCurrentCarPartNoSave(DateTime priceDate, double priceBase, double? priceBigWholesale,
			double? priceSmallWholesale, Currency currency, string fullName)
		{
			using (var db = GetContext())
			{
				var currentCarPart = new CurrentCarPart
				{
					Date = priceDate,
					Currency = currency,
					PriceBase = priceBase,
					PriceBigWholesale = priceBigWholesale,
					PriceSmallWholesale = priceSmallWholesale,
					FullName = fullName
				};

				return currentCarPart;
			}
		}

		public CurrentCarPart[] GetCurrentCarParts()
		{
			using (var db = GetContext())
			{
				return db.CurrentCarParts.ToArray();
			}
		}

		public CurrentCarPart GetCurrentCarPart(int directoryCarPartId, DateTime date)
		{
			using (var db = GetContext())
			{
				return db.CurrentCarParts.Where(c => c.DirectoryCarPartId == directoryCarPartId)
					.OrderByDescending(c => c.Date)
					.FirstOrDefault(c => DbFunctions.DiffDays(date, c.Date) < 0);
			}
		}

		public ArticlePrice[] GetArticlePrices(DateTime date, Currency currency)
		{
			using (var db = GetContext())
			{
				var directoryCarParts = db.DirectoryCarParts.ToList();
				var currentCarParts = db.CurrentCarParts.ToList();

				var articlePrices = new List<ArticlePrice>();
				foreach (var directoryCarPart in directoryCarParts)
				{
					var currentCarPart = currentCarParts
						.Where(c => c.DirectoryCarPartId == directoryCarPart.Id && c.Currency == currency)
						.OrderByDescending(c => c.Date)
						.FirstOrDefault(c => date.Date >= c.Date.Date);

					if (currentCarPart != null)
					{
						articlePrices.Add(new ArticlePrice
						{
							Article = directoryCarPart.Article,
							Mark = directoryCarPart.Mark,
							Description = directoryCarPart.Description,
							PriceRUR = currentCarPart.PriceBase
						});
					}
				}

				return articlePrices.ToArray();
			}
		}

		#endregion


		#region CurrentCarPartsRemainsToDate

		public void SetRemainsToFirstDateInMonth()
		{
			using (var db = GetContext())
			{
				var isProcessing = utilRepository.GetParameterValue<bool>(ParameterType.IsProcessingLastDateInMonthRemains);

				var firstDateIneMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
				if (!isProcessing)
				{
					utilRepository.EditParameter(ParameterType.IsProcessingLastDateInMonthRemains, true);
					if (!db.InfoLastMonthDayRemains.Any(d => DbFunctions.DiffDays(d.Date, firstDateIneMonth) == 0))
					{
						var lastDateInMonth = firstDateIneMonth.AddDays(-1);
						var carPartRemains = GetRemainsToDate(lastDateInMonth);

						db.InfoLastMonthDayRemains.AddRange(carPartRemains.Select(c => new InfoLastMonthDayRemain
						{
							Count = c.Remain,
							Date = firstDateIneMonth,
							DirectoryCarPartId = c.Id
						}));
						db.SaveChanges();
					}
					utilRepository.EditParameter(ParameterType.IsProcessingLastDateInMonthRemains, false);
				}
			}
		}

		public CarPartRemain[] GetRemainsToDate(DateTime date)
		{
			using (var db = GetContext())
			{
				var lastMonthDayRemains =
					db.InfoLastMonthDayRemains.Where(p => (p.Date.Year == date.Year && p.Date.Month == date.Month)).ToList();

				var firstDateInMonth = new DateTime(date.Year, date.Month, 1);

				var containers = db.InfoContainers.Include(c => c.CarParts).Where(c =>
					(DbFunctions.DiffDays(firstDateInMonth, c.DatePhysical) >= 0 && DbFunctions.DiffDays(c.DatePhysical, date) >= 0))
					.ToList();

				var currentCarParts = db.CurrentCarParts.ToArray();
				var directoryCarParts = db.DirectoryCarParts.ToArray();
				var carPartsRUR = new List<ArticlePrice>();
				foreach (var directoryCarPart in directoryCarParts)
				{
					var currentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == directoryCarPart.Id)
						.OrderByDescending(c => c.Date)
						.FirstOrDefault(c => c.Date.Date <= date.Date && c.Currency == Currency.RUR);

					if (currentCarPart != null)
					{
						var articlePrice = new ArticlePrice
						{
							CarPartId = directoryCarPart.Id,
							Article = directoryCarPart.Article,
							Mark = directoryCarPart.Mark,
							Description = directoryCarPart.Description,
							PriceRUR = currentCarPart.PriceBase
						};
						carPartsRUR.Add(articlePrice);
					}
				}

				var carPartsId = lastMonthDayRemains.Select(r => r.DirectoryCarPartId).
					Union(containers.SelectMany(c => c.CarParts.Select(p => p.DirectoryCarPartId))).Distinct().ToList();

				var carParts = db.DirectoryCarParts.ToList();
				var marks = carParts.Select(c => c.Mark).Distinct().ToList();

				var carPartRemains = new List<CarPartRemain>();
				foreach (var carPartId in carPartsId)
				{
					var carPart = carPartsRUR.FirstOrDefault(c => c.CarPartId == carPartId);
					var baseArticle = carParts.First(c => c.Id == carPartId).Article.ToLower();

					if (carPart == null)
					{
						bool isFound = false;
						foreach (var mark in marks)
						{
							var tmpMark = mark != null ? mark.ToLower() : null;
							carPart = carPartsRUR.FirstOrDefault(c => c.Article.ToLower() == baseArticle &&
																	  (c.Mark == null && tmpMark == null ||
																	   c.Mark != null && c.Mark.ToLower() == tmpMark));
							if (carPart != null)
							{
								isFound = true;
								break;
							}
						}

						if (!isFound)
						{
							using (var sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "NonInDB.txt"), true))
							{
								sw.WriteLine(baseArticle);
							}
							continue;
						}
					}

					var lastMonthDayRemain = lastMonthDayRemains.FirstOrDefault(r => r.DirectoryCarPartId == carPartId);
					if (lastMonthDayRemain == null)
					{
						bool isFound = false;
						foreach (var mark in marks)
						{
							var tmpMark = mark != null ? mark.ToLower() : null;
							var directoryCarPart = carParts.FirstOrDefault(c => c.Article.ToLower() == baseArticle &&
																				(c.Mark == null && tmpMark == null ||
																				 c.Mark != null && c.Mark.ToLower() == tmpMark));
							if (directoryCarPart != null)
							{
								lastMonthDayRemain = lastMonthDayRemains.FirstOrDefault(r => r.DirectoryCarPartId == directoryCarPart.Id);
								if (lastMonthDayRemain != null)
								{
									isFound = true;
									break;
								}
							}
						}

						if (!isFound)
						{
							continue;
						}
					}

					var remains = lastMonthDayRemain.Count;
					remains +=
						containers.Sum(
							c =>
								c.CarParts.Where(p => p.DirectoryCarPartId == carPartId)
									.Sum(c2 => (c.IsIncoming ? c2.CountCarParts : -c2.CountCarParts)));

					if (remains <= 0)
					{
						continue;
					}

					var carPartRemain = new CarPartRemain
					{
						Id = carPartId,
						Article = carPart.Article + carPart.Mark,
						Description = carPart.Description,
						PriceRUR = carPart.PriceRUR,
						PriceUSD = carPart.PriceUSD,
						Remain = remains
					};

					carPartRemains.Add(carPartRemain);
				}

				return carPartRemains.ToArray();
			}
		}

		#endregion

		public void AddCurrentCarParts(CurrentCarPart[] carParts)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(carParts);
			}
		}

		public void AddtDirectoryCarParts(DirectoryCarPart[] carParts)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(carParts);
			}
		}

		public void AddInfoLastMonthDayRemains(InfoLastMonthDayRemain[] remains)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(remains);
			}
		}

		public void AddInfoContainers(InfoContainer[] containers)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(containers);
			}
		}

		public void AddCurrentContainerCarPart(CurrentContainerCarPart[] carParts)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(carParts);
			}
		}
	}
}
