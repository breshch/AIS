﻿using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    public static class HelperDefaultDataBase
    {
        public static void SetDataBase()
        {
            using (var dc = new DataContext())
            {
                if (dc.Database.Exists())
                {
                    dc.Database.Delete();
                }
            }

            using (var dc = new DataContext())
            {
                dc.Database.Create();
                HelperCalendar.InputDateToDataBase(2014);

                var company = new DirectoryCompany { Name = "АВ" };

                dc.DirectoryCompanies.Add(company);                
                dc.SaveChanges();

                var typeOfPost = new DirectoryTypeOfPost { Name = "Склад"};

                dc.DirectoryTypeOfPosts.Add(typeOfPost);
                dc.SaveChanges();

                var post = new DirectoryPost
                {
                    Name = "Грузчик",
                    DirectoryTypeOfPost = typeOfPost,
                    DirectoryCompany = company,
                    Date = new DateTime(2014, 01, 01),
                    UserWorkerSalary = 25000,
                    UserWorkerHalfSalary = 10000
                };

                dc.DirectoryPosts.Add(post);

                post = new DirectoryPost
                {
                    Name = "Карщик",
                    DirectoryTypeOfPost = typeOfPost,
                    DirectoryCompany = company,
                    Date = new DateTime(2014, 01, 01),
                    UserWorkerSalary = 27000,
                    UserWorkerHalfSalary = 10000
                };

                dc.DirectoryPosts.Add(post);
                dc.SaveChanges();


                var slave = new DirectoryWorker 
                {
                    LastName = "Пупкин",
                    FirstName = "Василий",
                    MidName = "Васильевич",
                    BirthDay = new DateTime(1979, 12, 31),
                    Address = "Москва",
                    CellPhone = "+7985325642",
                    HomePhone = "+7495231568",
                    StartDate = DateTime.Now.AddDays(-40),
                    FireDate = null,
                    Gender = Gender.Male,
                    CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = dc.DirectoryPosts.First(),
                            ChangeDate = DateTime.Now.AddDays(-40)
                        }
                    })
                };

                dc.DirectoryWorkers.Add(slave);

                slave = new DirectoryWorker
                {
                    LastName = "Пупкина",
                    FirstName = "Василиса",
                    MidName = "Васильевна",
                    BirthDay = new DateTime(1917, 10, 15),
                    Address = "Питер",
                    CellPhone = "+7985333642",
                    HomePhone = "+7495231568",
                    StartDate = DateTime.Now.AddDays(-10),
                    FireDate = null,
                    Gender = Gender.Female,
                    CurrentCompaniesAndPosts = new List<CurrentPost>(new[] 
                    { 
                        new CurrentPost
                        {
                            DirectoryPost = dc.DirectoryPosts.First(),
                            ChangeDate = DateTime.Now.AddDays(-10),
                            FireDate = DateTime.Now.AddDays(-6)
                        },

                        new CurrentPost
                        {
                            DirectoryPost = dc.DirectoryPosts.First(p => p.Name == "Карщик"),
                            ChangeDate = DateTime.Now.AddDays(-5)
                        }
                    })
                };

                dc.DirectoryWorkers.Add(slave);
                dc.SaveChanges();
            }
        }
    }
}
