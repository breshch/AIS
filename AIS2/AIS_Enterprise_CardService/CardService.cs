using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private BusinessContext _bc;

        public CardService()
        {
            InitializeComponent();

            DataContext.ChangeServerAndDataBase("95.31.130.52", "AV_New_Dev");
            DataContext.ChangeUser("Breshchenko", "3179");
            _bc = new BusinessContext();
        }

        protected override void OnStart(string[] args)
        {
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        //_bc.EditCurrencyValueSumm("TotalCard", Currency.RUR, 262941.87);

                        while (true)
                        {
                            using (var client = new ImapClient())
                            {
                                client.Connect("imap.gmail.com", 993, true);

                                client.AuthenticationMechanisms.Remove("XOAUTH");

                                client.Authenticate("breshch", "Mp~7200~aA");

                                var query = SearchQuery.DeliveredAfter(DateTime.Now.AddDays(-1)).And(SearchQuery.All);

                                var psbFolder = client.GetFolder("PSB");
                                psbFolder.Open(FolderAccess.ReadOnly);


                                var mails = new List<Mail>();

                                foreach (var uid in psbFolder.Search(query))
                                {
                                    var message = psbFolder.GetMessage(uid);
                                    var textPart = message.BodyParts.First() as TextPart;

                                    var mail = new Mail
                                    {
                                        Date = message.Date.LocalDateTime,
                                        Body = textPart.GetText(Encoding.UTF8)
                                    };
                                    mails.Add(mail);
                                }

                                using (var sw = new StreamWriter(@"D:\C#\AIS2\AIS_Enterprise_CardService\bin\Debug\mails.txt", true))
                                {
                                    foreach (var mail in mails)
                                    {
                                        string availableName = mail.Body.IndexOf("Dostupno") != -1 ? "Dostupno" : "Доступно";

                                        string availableSummString = mail.Body.Substring(mail.Body.IndexOf(availableName) + 9, mail.Body.LastIndexOf("RUR") - 1 - (mail.Body.IndexOf(availableName) + 9)).
                                            Replace(".", ",").Replace(" ", "");

                                        double availableSumm = double.Parse(availableSummString);

                                        int indexSemicolon = mail.Body.IndexOf(";");
                                        indexSemicolon = mail.Body.IndexOf(";", indexSemicolon + 1);

                                        string description = mail.Body.Substring(indexSemicolon + 1, mail.Body.IndexOf(availableName) - 1 - (indexSemicolon + 1));

                                        _bc.AddInfoSafeCard(mail.Date, availableSumm, Currency.RUR, description);

                                        sw.WriteLine(DateTime.Now + "\t" + mail.Date + "\t" + availableSumm + "\t" + description);
                                    }
                                }

                                client.Disconnect(true);
                            }

                            Thread.Sleep(1000 * 60 * 60 * 3);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (var sw = new StreamWriter(@"D:\C#\AIS2\AIS_Enterprise_CardService\bin\Debug\errors.txt", true))
                        {
                            sw.WriteLine(DateTime.Now + "\t" + ex);
                        }
                    }

                    Environment.Exit(0);
                });
        }

        protected override void OnStop()
        {
            if (_bc != null)
            {
                _bc.Dispose();
            }
        }
    }
}
