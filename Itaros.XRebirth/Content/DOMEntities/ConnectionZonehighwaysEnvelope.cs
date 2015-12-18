using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content.DOMEntities
{
    public class ConnectionZonehighwaysEnvelope : ConnectionEnvelope
    {

        public ConnectionZonehighwaysEnvelope(XmlNode node)
            : base(node)
        {

        }

        protected override void Fetch()
        {
            base.Fetch();

            Entrypoint = new ConnectionEnvelope(_node.SelectSingleNode("./macro/connections/connection[@ref='entrypoint']"));
            Exitpoint = new ConnectionEnvelope(_node.SelectSingleNode("./macro/connections/connection[@ref='exitpoint']"));
        }

        public ConnectionEnvelope Entrypoint;
        public ConnectionEnvelope Exitpoint;



    }
}
