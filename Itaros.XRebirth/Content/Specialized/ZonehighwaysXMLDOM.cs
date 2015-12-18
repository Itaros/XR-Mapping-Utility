using Itaros.XRebirth.Content.DOMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content.Specialized
{
    public class ZonehighwaysXMLDOM : XMLDOMData
    {

        public ZonehighwaysXMLDOM(string fullpath)
            : base(fullpath)
        {

        }

        private List<ZonehighwayEnvelope> _zonehighways = new List<ZonehighwayEnvelope>();
        public IEnumerable<ZonehighwayEnvelope> ZoneHighways { get { return _zonehighways; } }


        protected override void Load()
        {
            base.Load();

            var highways = _dom.SelectNodes("/macros/macro");
            foreach (XmlNode highway in highways)
            {
                _zonehighways.Add(new ZonehighwayEnvelope(highway));
            }
        }

        public class ZonehighwayEnvelope : BaseEnvelope
        {
            public ZonehighwayEnvelope(XmlNode node) : base(node) { }

            public List<ConnectionEnvelope> Connections = new List<ConnectionEnvelope>();

        }


        public void FindConnections(SectorsXMLDOM sectorsXMLDOM, ZonesXMLDOM zonesXMLDOM)
        {
            //Acquiring connections
            foreach (ZonehighwayEnvelope zonehighway in _zonehighways)
            {
                zonehighway.Connections.Clear();
                zonehighway.Connections = sectorsXMLDOM.RequestRefs(zonehighway.InternalName);
            }
            //Acquiring weird path connections to zones
            foreach (ZonehighwayEnvelope zonehighway in _zonehighways)
            {
                foreach (ConnectionZonehighwaysEnvelope highwayConnection in zonehighway.Connections)
                {
                    highwayConnection.Entrypoint.LinkPath(highwayConnection.Parent as Specialized.SectorsXMLDOM.SectorNodeEnvelope);
                    highwayConnection.Exitpoint.LinkPath(highwayConnection.Parent as Specialized.SectorsXMLDOM.SectorNodeEnvelope);
                }
            }
        }

        public void AddToMacros(XmlNode node)
        {
            XmlNode macros = _dom.SelectSingleNode("/macros");
            macros.AppendChild(node);
        }
    }
}
