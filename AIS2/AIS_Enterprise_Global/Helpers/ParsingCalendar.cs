﻿using AIS_Enterprise_Data;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    public static class ParsingCalendar
    {
        public async static Task GetCalendar(BusinessContext bc, int year)
        {
            string page = "http://www.superjob.ru/proizvodstvennyj_kalendar/" + year;

            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(page))
                {
                    using (HttpContent content = response.Content)
                    {
                        string html = await content.ReadAsStringAsync();

                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        var cells = doc.DocumentNode.SelectNodes(@"//div[@class='pk_cells']");

                        var holidays = new List<DateTime>();

                        int currentMonth = 1;
                        foreach (var monthCells in cells)
                        {
                            foreach (var nodeTd in monthCells.ChildNodes.Where(n => n.Attributes.Any(a => a.Value == "pk_holiday pie")))
                            {
                                int day = int.Parse(nodeTd.InnerText);
                                var holiday = new DateTime(year, currentMonth, day);

                                holidays.Add(holiday);
                            }

                            currentMonth++;
                        }

                        bc.SetHolidays(year, holidays);
                    }
                }
            }
        }
    }
}