using Itaros.XRebirth.Content.DOMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content.Specialized
{
    public class ZonesXMLDOM : XMLDOMData
    {

        public ZonesXMLDOM(string fullpath)
            : base(fullpath)
        {

        }

        public List<ZoneEnvelope> Zones = new List<ZoneEnvelope>();


        protected override void Load()
        {
            base.Load();

            var zonesNodes = _dom.SelectNodes("/macros/macro[@class='zone']");
            foreach (XmlNode node in zonesNodes)
            {
                Zones.Add(new ZoneEnvelope(node));
            }
            //foreach (XmlNode node in _zonesNodes)
            //{
            //    Console.WriteLine(node.Attributes["name"].InnerText);
            //}
        }

        public class ZoneEnvelope : BaseEnvelope
        {

            public ZoneEnvelope(XmlNode node)
                : base(node)
            {

            }

            //Inserts node into connections
            public void AddToConnections(XmlNode connection)
            {
                _node.SelectSingleNode("./connections").AppendChild(connection);
            }
        }

    }
}
