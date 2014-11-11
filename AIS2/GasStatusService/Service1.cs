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

                    var bodyNode = htmlDoc.DocumentNode.SelectNodes("//body//div//table//tr[1]//td[1]").First();

                    html = bodyNode.InnerText;

                    html = html.Substring(html.IndexOf(";") + 1, html.LastIndexOf(".") - html.IndexOf(";") - 2);

                    double balance = double.Parse(html);

                    if (balance < 10000)
                    {
                        string message = "Баланс за ГСМ по Логистикону = " + balance;

                        SendEmail(message, "breshch@gmail.com");
                        SendEmail(message, "vaukalak@gmail.com");
                        SendEmail(message, "lelia2502@yandex.ru");
                        SendEmail(message, "natacha_buch@mail.ru");
                    }

                    using (var sw = new StreamWriter(@"D:\C#\AIS2\GasStatusService\bin\Release\index.txt"))
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

        private static void SendEmail(string info, string email)
        {
            try
            {
                var fromAddress = new MailAddress("LogistikonCompany@gmail.com");
                var toAddress = new MailAddress(email);
                const string fromPassword = "Mp7200aA";
                const string subject = "Закончились средства на ГСМ !!!";
                string body = info;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                using (var sw = new StreamWriter(@"D:\C#\AIS\AIS2\GasStatusService\bin\Release\error.txt"))
                {
                    sw.WriteLine("success");
                }
            }
            catch (Exception ex)
            {
                using (var sw = new StreamWriter(@"D:\C#\AIS\AIS2\GasStatusService\bin\Release\error.txt"))
                {
                    sw.WriteLine(ex.ToString());
                }
            }
        }
    }
}
