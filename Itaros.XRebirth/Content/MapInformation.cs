using Itaros.XRebirth.Content.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itaros.XRebirth.Content
{
    public class MapInformation
    {

        public string MapName { get; private set; }

        public MapInformation(string name)
        {
            MapName = name;
        }

        public ZonesXMLDOM Zones { get; set; }
        public SectorsXMLDOM Sectors { get; set; }
        public XMLDOMData Clusters { get; set; }
        public XMLDOMData Galaxies { get; set; }
        public ZonehighwaysXMLDOM Zonehighways { get; set; }

        public IEnumerable<XMLDOMData> AllXMLDOMFiles
        {
            get
            {
                List<XMLDOMData> statlist = new List<XMLDOMData>();
                statlist.Add(Zones);
                statlist.Add(Sectors);
                statlist.Add(Clusters);
                statlist.Add(Galaxies);
                statlist.Add(Zonehighways);
                return statlist;
            }
        }

    }
}
