using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using AIS_Enterprise_Gas;
using HtmlAgilityPack;

namespace AIS_Enterprise_Gas
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
	        var gas = new GasProcessing();
            Task.Factory.StartNew(gas.GetBalance);
        }
    }
}
