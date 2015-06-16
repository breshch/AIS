using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AIS_Enterprise_CardService.Cards;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace AIS_Enterprise_CardService
{
    public partial class CardService : ServiceBase
    {
        public CardService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
			Initialize();

			Observable.Interval(new TimeSpan(0, 3, 0, 0))
				.Subscribe(x => Initialize());
        }


		private static void Initialize()
		{
			using (var client = new ImapClient())
			{
				client.Connect("imap.gmail.com", 993, true);

				client.AuthenticationMechanisms.Remove("XOAUTH");

				client.Authenticate("breshch", "Mp~7200~aA");

				var query = SearchQuery.DeliveredAfter(DateTime.Now.AddDays(-1)).And(SearchQuery.All);

				var cardsFolder = client.GetFolder("Cards");
				cardsFolder.Open(FolderAccess.ReadOnly);


				foreach (var uid in cardsFolder.Search(query))
				{
					var message = cardsFolder.GetMessage(uid);
					var textPart = message.BodyParts.First() as TextPart;

					var date = message.Date.LocalDateTime;
					var body = textPart.GetText(Encoding.UTF8);

					using (var bc = new BusinessContext())
					{
						if (!bc.IsNewMessage(date, body))
						{
							continue;
						}
					}

					var subject = message.Subject;

					string bankName = subject.Substring(subject.LastIndexOf(" ")).Trim();

					CardBase card = null;

					switch (bankName)
					{
						case "VTB24":
							card = new CardVTB24(body);
							break;
						case "PSB":
							card = new CardPSB(body);
							break;
					}

					double? sum = card.GetSum();

					if (sum != null)
					{
						using (var bc = new BusinessContext())
						{
							bc.AddInfoSafeCard(date, sum.Value, Currency.RUR, body, bankName);
						}
					}

					using (var sw = new StreamWriter(@"C:\CardService\mails.txt", true))
					{
						sw.WriteLine(DateTime.Now + "\t" + date + "\t" + sum + "\t" + body);
					}
				}

				client.Disconnect(true);
			}
		}
    }
}
