using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using RemainsWeb.Infrastructure;
using RemainsWeb.Models;

namespace RemainsWeb.Controllers
{
	public class MainController : Controller
	{
		// GET: Main
		public ActionResult Index()
		{
			var currentRemains = CustomCache.GetRemainsAndCosts();

			return View(currentRemains);
		}
	}
}