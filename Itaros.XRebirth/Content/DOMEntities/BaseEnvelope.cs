using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content.DOMEntities
{
    public abstract class BaseEnvelope
    {

        public BaseEnvelope Parent { get; set; }

        protected BaseEnvelope(XmlNode node)
        {
            _node = node;
            Fetch();
        }

        public string InternalName { get; private set; }

        protected XmlNode _node;

        protected virtual void Fetch()
        {
            XmlAttribute nameAttrib = _node.Attributes["name"];
            if (nameAttrib != null)
            {
                InternalName = nameAttrib.InnerText;
            }
            else
            {
                InternalName = null;
            }
        }

        public override string ToString()
        {
            return InternalName;
        }

    }
}
