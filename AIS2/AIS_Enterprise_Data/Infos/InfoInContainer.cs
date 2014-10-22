using AIS_Enterprise_Data.Currents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoInContainer : InfoBaseContainer
    {
        public InfoInContainer()
        {
            CarParts = new List<CurrentInContainerCarPart>();
        }
    }
}
