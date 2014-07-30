using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GasStatusService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task.Factory.StartNew(GetBalance);
        }

        protected override void OnStop()
        {
        }

        private void GetBalance()
        {
            try
            {
                while (true)
                {
                    var req = WebRequest.Create("http://95.171.14.129:8097/");

                    req = WebRequest.Create("http://95.171.14.129:8097/");
                    req.Credentials = new NetworkCredential("logistikon", "logist179");

                    var resp = req.GetResponse();
                    var stream = resp.GetResponseStream();

                    string html = null;
                    using (var sr = new StreamReader(stream))
                    {
                        html = sr.ReadToEnd();
                    }

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    var bodyNode = htmlDoc.DocumentNode.SelectNodes("//body//div//table//tr[1]//td[2]").First();

                    html = bodyNode.InnerText;

                    html = html.Substring(html.IndexOf(";") + 1, html.LastIndexOf(".") - html.IndexOf(";") - 2);

                    double balance = double.Parse(html);

                    if (balance < 10000)
                    {
                        string message = "Баланс за ГСМ по Логистикону = " + balance + ". Пожалуйста, пополните баланс.";
                        
                        SendSMS(message, "79264323519", "c476ba9f-ed2c-efb4-a912-d8628af89af7+79264323519@sms.ru");
                        SendSMS(message, "79268613825", "362a7ed6-018c-16b4-756a-f6ec487b3d2a+79268613825@sms.ru");
                        SendSMS(message, "79035423769", "6feba375-3c8e-dd94-19e6-d69cdc52f385+79035423769@sms.ru");
                    }

                    using (var sw = new StreamWriter(@"E:\C#\AIS2\GasStatusService\bin\Debug\index.txt"))
                    {
                        sw.WriteLine(balance.ToString());
                    }

                    Thread.Sleep(1000 * 60 * 60 * 5);
                }
            }
            catch (Exception ex)
            {
                using (var sw = new StreamWriter(@"E:\C#\AIS2\GasStatusService\bin\Debug\error.txt"))
                {
                    sw.WriteLine(ex.ToString());
                }
            }

            Environment.Exit(0);
        }

        private static void SendSMS(string info, string phone, string email)
        {
            try
            {
                var client = new SmtpClient("smtp.mail.ru", 587)
                {
                    Credentials = new NetworkCredential("breshch@mail.ru", "Mp7200aA"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                var message = new MailMessage
                {
                    From = new MailAddress("breshch@mail.ru"),
                    Subject = phone,
                    Body = info
                };
                message.To.Add(new MailAddress(email));

                client.Send(message);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
