using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace GasStatusWeb.Controllers
{
	public class MainController : Controller
	{
		// GET: Main
		public ActionResult Index()
		{
			var gasBalanceStatus = GetGasBalanceStatus();

			return View(gasBalanceStatus);
		}

		public double Index2()
		{
			return GetGasBalanceStatus();
		}

		private double GetGasBalanceStatus()
		{
			try
			{
				var req = WebRequest.Create("http://afs.ngk-interoil.ru:8097/");

				req = WebRequest.Create("http://afs.ngk-interoil.ru:8097/");
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

				double balance = Math.Round(double.Parse(html), 2);

				return balance;
			}
			catch (Exception ex)
			{
				return -1;
			}
		}
	}
}
