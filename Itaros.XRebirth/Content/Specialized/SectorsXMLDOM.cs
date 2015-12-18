using Itaros.XRebirth.Content.DOMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content.Specialized
{
    public class SectorsXMLDOM : XMLDOMData
    {

        public SectorsXMLDOM(string fullpath)
            : base(fullpath)
        {

        }

        public List<SectorNodeEnvelope> Sectors = new List<SectorNodeEnvelope>();

        protected override void Load()
        {
            base.Load();

            var sectors = _dom.SelectNodes("/macros/macro[@class='sector']");
            Sectors = sectors.OfType<XmlNode>().Select(n => new SectorNodeEnvelope(n)).ToList();
        }


        public class SectorNodeEnvelope : BaseEnvelope
        {

            public List<ConnectionEnvelope> Connections = new List<ConnectionEnvelope>();

            public SectorNodeEnvelope(XmlNode node) : base(node) { }

            protected override void Fetch()
            {
                base.Fetch();

                var connections = _node.SelectNodes("./connections/connection");
                foreach (XmlNode connection in connections)
                {
                    var env = ConnectionEnvelope.Autoresolve(connection);
                    env.Parent = this;
                    Connections.Add(env);
                }
            }


            public void AddToConnections(XmlNode node)
            {
                XmlNode connections = _node.SelectSingleNode("./connections");
                connections.AppendChild(node);
            }
        }



        public List<ConnectionEnvelope> RequestRefs(string reftarget)
        {
            List<ConnectionEnvelope> connections = new List<ConnectionEnvelope>();
            foreach (SectorNodeEnvelope sector in Sectors)
            {
                foreach (ConnectionEnvelope connection in sector.Connections)
                {
                    if (connection.MacroRefString == reftarget)
                    {
                        connections.Add(connection);
                    }
                }
            }
            return connections;
        }

        public void FindConnections(ZonesXMLDOM zonesXMLDOM)
        {
            foreach (var sector in Sectors)
            {
                foreach (var connection in sector.Connections)
                {
                    connection.ConnectsTo = zonesXMLDOM.Zones.FirstOrDefault(z => z.InternalName == connection.MacroRefString);
                }
            }
        }
    }
}
