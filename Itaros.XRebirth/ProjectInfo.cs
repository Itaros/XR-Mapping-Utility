using Itaros.XRebirth.Content;
using Itaros.XRebirth.Content.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Itaros.XRebirth
{
    public class ProjectInfo
    {

        public XRebirthInfo XRebirth { get; private set; }
        public ExtensionInfo Extension { get; private set; }

        public ProjectInfo(XRebirthInfo xr, ExtensionInfo ex)
        {
            DataContainers = new Content.DataContainers();

            XRebirth = xr;
            Extension = ex;
        }

        public DataContainers DataContainers { get; private set; }

        public void FillData()
        {
            DirectoryInfo extensionDirectory = new DirectoryInfo(XRebirth.PathToGame+Path.DirectorySeparatorChar+"extensions"+Path.DirectorySeparatorChar+Extension.ExtensionName);
            if (extensionDirectory.Exists)
            {
                DirectoryInfo mapsDirectory = new DirectoryInfo(extensionDirectory.FullName+Path.DirectorySeparatorChar+"maps");
                DataContainers.PresentMapGroups = mapsDirectory.GetDirectories().Select(o => o.Name);
            }
            else
            {
                throw new FileNotFoundException("Can't find extension dir", extensionDirectory.FullName);
            }
        }

        public void FillCurrentMapInfo()
        {
            DirectoryInfo extensionDirectory = new DirectoryInfo(XRebirth.PathToGame + Path.DirectorySeparatorChar + "extensions" + Path.DirectorySeparatorChar + Extension.ExtensionName);
            if (extensionDirectory.Exists)
            {
                DirectoryInfo mapsDirectory = new DirectoryInfo(extensionDirectory.FullName + Path.DirectorySeparatorChar + "maps");
                var mapinfo = DataContainers.CurrentMap;
                DirectoryInfo mapinfoDirectory = new DirectoryInfo(mapsDirectory.FullName+Path.DirectorySeparatorChar+mapinfo.MapName);
                mapinfo.Zones = new ZonesXMLDOM(mapinfoDirectory.FullName+Path.DirectorySeparatorChar+"zones.xml");
                mapinfo.Sectors = new SectorsXMLDOM(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "sectors.xml");
                mapinfo.Clusters = new XMLDOMData(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "clusters.xml");
                mapinfo.Galaxies = new XMLDOMData(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "galaxy.xml");
                mapinfo.Zonehighways = new ZonehighwaysXMLDOM(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "zonehighways.xml");
                mapinfo.Sectors.FindConnections(mapinfo.Zones);
                mapinfo.Zonehighways.FindConnections(mapinfo.Sectors, mapinfo.Zones);
            }
            else
            {
                throw new FileNotFoundException("Can't find extension dir", extensionDirectory.FullName);
            }
        }


        public void FlushDOM()
        {
             DirectoryInfo extensionDirectory = new DirectoryInfo(XRebirth.PathToGame + Path.DirectorySeparatorChar + "extensions" + Path.DirectorySeparatorChar + Extension.ExtensionName);
             if (extensionDirectory.Exists)
             {
                 DirectoryInfo mapsDirectory = new DirectoryInfo(extensionDirectory.FullName + Path.DirectorySeparatorChar + "maps");
                 DirectoryInfo mapinfoDirectory = new DirectoryInfo(mapsDirectory.FullName + Path.DirectorySeparatorChar + DataContainers.CurrentMap.MapName);

                 Encoding encoding = new UTF8Encoding(false);

                 DataContainers.CurrentMap.Zones.FlushTo(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "zones.xml", encoding);
                 DataContainers.CurrentMap.Sectors.FlushTo(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "sectors.xml", encoding);
                 DataContainers.CurrentMap.Clusters.FlushTo(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "clusters.xml", encoding);
                 DataContainers.CurrentMap.Galaxies.FlushTo(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "galaxy.xml", encoding);
                 DataContainers.CurrentMap.Zonehighways.FlushTo(mapinfoDirectory.FullName + Path.DirectorySeparatorChar + "zonehighways.xml", encoding);

             }
             else
             {
                 throw new FileNotFoundException("Can't find extension dir", extensionDirectory.FullName);
             }
        }
    }
}
