using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        using (var client = new ImapClient())
                        {
                            client.Connect("imap.gmail.com", 993, true);

                            // Note: since we don't have an OAuth2 token, disable
                            // the XOAUTH2 authentication mechanism.
                            client.AuthenticationMechanisms.Remove("XOAUTH");

                            client.Authenticate("breshch", "Mp~7200~aA");

                            // let's search for all messages received after Jan 12, 2013 with "MailKit" in the subject...
                            var query = SearchQuery.DeliveredAfter(DateTime.Now.AddDays(-1)).And(SearchQuery.All);

                            // The Inbox folder is always available on all IMAP servers...
                            var psbFolder = client.GetFolder("PSB");
                            psbFolder.Open(FolderAccess.ReadOnly);

                            using (var sw = new StreamWriter(@"D:\C#\AIS2\AIS_Enterprise_CardService\bin\Debug\mails.txt"))
                            {

                                foreach (var uid in psbFolder.Search(query))
                                {
                                    var textPart = psbFolder.GetMessage(uid).BodyParts.First() as TextPart;
                                    sw.WriteLine(textPart.GetText(Encoding.UTF8));
                                }

                                //sw.WriteLine("Total messages: {0}", psbFolder.Count);
                                //sw.WriteLine("Recent messages: {0}", psbFolder.Recent);

                                //for (int i = 0; i < psbFolder.Count; i++)
                                //{
                                //    var message = psbFolder.GetMessage(i);
                                //    sw.WriteLine("Subject: {0}", message.Subject);
                                //}
                            }

                            client.Disconnect(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (var sw = new StreamWriter(@"D:\C#\AIS2\AIS_Enterprise_CardService\bin\Debug\errors.txt", true))
                        {
                            sw.WriteLine(ex);
                        }
                    }

                    Environment.Exit(0);
                });
        }

        protected override void OnStop()
        {
        }
    }
}
