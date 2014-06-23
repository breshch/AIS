using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AIS_Enterprise_AV.Helpers.ExcelToDB
{
    public static class ConvertingExcelToDB
    {
        private const string PATH_FIRE_WORKERS = "ТабельXML/firehuy.txt";
        private const string PATH_TABEL_WORKERS = "ТабельXML/huy.xml";

        private class WorkerFire
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime FireDate { get; set; }
        }

        private static List<WorkerFire> _workersFire = new List<WorkerFire>();

        private static void AddingFireWorkers()
        {
            using (StreamReader sr = new StreamReader(PATH_FIRE_WORKERS))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] mass = line.Split(' ');
                    string lastName = mass[0];
                    string firstName = mass[1];
                    DateTime fireDate = DateTime.Parse(mass[2]);

                    WorkerFire worker = new WorkerFire();
                    worker.LastName = lastName;
                    worker.FirstName = firstName;
                    worker.FireDate = fireDate;

                    _workersFire.Add(worker);
                }
            }
        }



        private static void AddingWorkers(BusinessContext bc)
        {
            var doc = XDocument.Load(PATH_TABEL_WORKERS);
            var worksheets = doc.Root.Elements("{urn:schemas-microsoft-com:office:spreadsheet}Worksheet").ToList();

            var lastDate = DateTime.MinValue;

            foreach (var worksheet in worksheets.Where(w => w.Attribute("{urn:schemas-microsoft-com:office:spreadsheet}Name").Value != "Шаблон"))
            {
                string[] date = worksheet.Attribute("{urn:schemas-microsoft-com:office:spreadsheet}Name").Value.Split('.');

                int month = int.Parse(date[0]);
                int year = int.Parse("20" + date[1]);

                if (year != 2014 || year == 2014 && month != 4 && month != 5)
                {
                    continue;
                }
                //if (year != 2014)
                //{
                //    continue;
                //}
                Debug.WriteLine(year + " " + month);

                var rows = worksheet.Elements("{urn:schemas-microsoft-com:office:spreadsheet}Table").Elements("{urn:schemas-microsoft-com:office:spreadsheet}Row").ToList();
                for (int row = 3; row < rows.Count; row += 2)
                {
                    var cellRegularDays = worksheet.Element("{urn:schemas-microsoft-com:office:spreadsheet}Table").Elements("{urn:schemas-microsoft-com:office:spreadsheet}Row").ToList()[row].Elements("{urn:schemas-microsoft-com:office:spreadsheet}Cell").ToList();
                    var cellOverTimeDays = worksheet.Element("{urn:schemas-microsoft-com:office:spreadsheet}Table").Elements("{urn:schemas-microsoft-com:office:spreadsheet}Row").ToList()[row + 1].Elements("{urn:schemas-microsoft-com:office:spreadsheet}Cell").ToList();

                    string[] workerFio = cellRegularDays[1].Value.Split(' ');

                    string lastName = workerFio[0].Trim();
                    string firstName = workerFio[1].Trim();
                    string midName = "";

                    if (workerFio.Count() == 3)
                    {
                        midName = workerFio[2].Trim();
                    }

                    int indexPostName = 0;

                    string postName = "";
                    for (int i = 0; i < cellOverTimeDays.Count; i++)
                    {
                        postName = cellOverTimeDays[i].Value.ToLower();

                        switch (postName)
                        {
                            case "заведующий складом":
                                postName = "ЗавСкладом";
                                break;
                            case "зам. зав. складом":
                                postName = "ЗамЗавСкладом";
                                break;
                            case "карщик-кладовщик":
                                postName = "КарщикКладовщик";
                                break;
                            case "оклейщица":
                                postName = "Оклейщик";
                                break;
                            case "бригадир-оклейщик":
                                postName = "БригадирОклейщик";
                                break;

                        }

                        if (postName != "")
                        {
                            postName = postName[0].ToString().ToUpper() + postName.Substring(1);
                            if (bc.ExistsDirectoryPost(postName))
                            {
                                indexPostName = i;
                                break;
                            }
                        }
                    }

                    Debug.WriteLine(lastName + " " + firstName + " " + midName + " : " + postName);

                    var worker = bc.GetDirectoryWorker(lastName, firstName);

                    int day = 0;
                    if (worker == null)
                    {
                        day = 1;

                        if (cellRegularDays[3].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value == "" || cellRegularDays[3].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value == "0")
                        {
                            for (int i = 4; i < cellRegularDays.Count; i++)
                            {
                                if (cellRegularDays[i].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value != "" && cellRegularDays[i].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value != "0")
                                {
                                    day = i - 2;
                                    break;
                                }

                                if (cellRegularDays[i].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value == "")
                                {
                                    bool isBreak = false;
                                    for (int j = i + 1; j < cellRegularDays.Count; j++)
                                    {
                                        if (cellRegularDays[j].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value != "" && cellRegularDays[j].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value != "0")
                                        {
                                            day = j - 2;
                                            isBreak = true;
                                            break;
                                        }
                                    }

                                    if (isBreak)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        var currentPost = new CurrentPost
                        {
                            ChangeDate = new DateTime(year, month, day),
                            DirectoryPost = bc.GetDirectoryPost(postName),
                        };

                        worker = bc.AddDirectoryWorker(lastName, firstName, midName, Gender.Male, DateTime.Now, "1", "1", "1", new DateTime(year, month, day), null, currentPost);

                        day--;
                    }

                    var infoMonth = new InfoMonth
                    {
                        Date = new DateTime(year, month, 1),
                        BirthDays = 500
                    };

                    worker.InfoMonthes.Add(infoMonth);

                    int countDaysInMonth = DateTime.DaysInMonth(year, month);
                    for (int i = 3 + day; i < countDaysInMonth + 3; i++)
                    {
                        var workerDate = new DateTime(year, month, day + 1);

                        if (worker.FireDate != null)
                        {
                            break;
                        }

                        var infoDate = new InfoDate
                        {
                            Date = workerDate,
                            DescriptionDay = DescriptionDay.Был
                        };

                        string value = cellRegularDays[i].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value;

                        if (value != "")
                        {
                            double result;
                            if (double.TryParse(value, out result))
                            {
                                infoDate.CountHours = result;
                            }

                            if (cellOverTimeDays[i + indexPostName - 1].Value != "")
                            {
                                if (double.TryParse(cellOverTimeDays[i + indexPostName - 1].Element("{urn:schemas-microsoft-com:office:spreadsheet}Data").Value, out result))
                                {
                                    infoDate.CountHours += result;
                                }
                            }

                            if (infoDate.CountHours == 0)
                            {
                                if (!bc.IsWeekend(workerDate))
                                {
                                    infoDate.DescriptionDay = DescriptionDay.П;
                                }

                                infoDate.CountHours = null;
                            }

                            worker.InfoDates.Add(infoDate);
                        }

                        var workerFire = _workersFire.FirstOrDefault(w => w.FirstName == worker.FirstName && w.LastName == worker.LastName);

                        if (workerFire != null)
                        {
                            if (workerDate.Date == workerFire.FireDate.Date)
                            {
                                worker.FireDate = workerFire.FireDate;
                            }
                        }

                        day++;

                        if (lastDate.Date < workerDate.Date)
                        {
                            lastDate = workerDate;
                        }
                    }

                    string fio = "";
                    string[] fioMass = null;

                    do
                    {
                        if (rows.Count() <= row + 2)
                        {
                            break;
                        }

                        var cells3 = worksheet.Element("{urn:schemas-microsoft-com:office:spreadsheet}Table").Elements("{urn:schemas-microsoft-com:office:spreadsheet}Row").ToList()[row + 2].Elements("{urn:schemas-microsoft-com:office:spreadsheet}Cell").ToList();
                        if (cells3.Count() > 1)
                        {
                            fio = cells3[1].Value;
                            fioMass = fio.Split(' ');
                        }
                        else
                        {
                            fio = "";
                            fioMass = null;
                        }

                        if (fio == "" || bc.ExistsDirectoryPost(fio) || fioMass == null || fioMass.Count() < 2)
                        {
                            row++;
                        }
                    } while (fio == "" || bc.ExistsDirectoryPost(fio) || fioMass == null || fioMass.Count() < 2);
                }

                bc.SaveChanges();
            }

            bc.EditParameter("LastDate", lastDate.ToString());
        }

        public static void ConvertExcelToDB(BusinessContext bc)
        {
            _workersFire.Clear();
            AddingFireWorkers();

            AddingWorkers(bc);
        }
    }
}
