using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content.DOMEntities
{
    public class ConnectionEnvelope : BaseEnvelope
    {

        public enum ConnectionType
        {
            NOTSET,
            zones,
            zonehighways
        }

        public BaseEnvelope ConnectsTo { get; set; }

        private XmlNode _macroRef;
        public string MacroRefString
        {
            get
            {
                XmlAttribute attribute = _macroRef.Attributes["ref"];
                if (attribute != null)
                {
                    return attribute.InnerText;
                }
                else
                {
                    return null;
                }
            }
        }
        public string MacroPathString
        {
            get
            {
                XmlAttribute attribute = _macroRef.Attributes["path"];
                if (attribute != null)
                {
                    return attribute.InnerText;
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void Fetch()
        {
            base.Fetch();

            //Getting macroref
            XmlNode macroRef = _node.SelectSingleNode("./macro");
            if (macroRef != null)
            {
                _macroRef = macroRef;
            }
        }

        public ConnectionEnvelope(XmlNode node) : base(node) { }

        /// <summary>
        /// Factory type to automagically create correct type of connection
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static ConnectionEnvelope Autoresolve(XmlNode connection)
        {
            XmlAttribute refAttrib = connection.Attributes["ref"];
            if (refAttrib != null)
            {
                if (refAttrib.InnerText == "zonehighways")
                {
                    return new ConnectionZonehighwaysEnvelope(connection);
                }
                return new ConnectionEnvelope(connection);
            }
            else
            {
                return new ConnectionEnvelope(connection);
            }
        }

        /// <summary>
        /// It is magically assumed that path evaluation will be performed as I see fit. What a hell...
        /// </summary>
        /// <param name="zonesXMLDOM"></param>
        public void LinkPath(Specialized.SectorsXMLDOM.SectorNodeEnvelope sector)
        {
            if (MacroPathString != null)
            {
                string resolveString = MacroPathString.Replace("../", "");
                ConnectsTo = sector.Connections.FirstOrDefault(o => o.InternalName == resolveString);
            }
        }


        public Math.Vector3 GetPositionVector()
        {
            XmlNode positionNode = _node.SelectSingleNode("./offset/position");
            if (positionNode != null)
            {
                XmlAttribute xa = positionNode.Attributes["x"];
                XmlAttribute ya = positionNode.Attributes["y"];
                XmlAttribute za = positionNode.Attributes["z"];
                decimal x = decimal.Parse(xa.InnerText);
                decimal y = decimal.Parse(ya.InnerText);
                decimal z = decimal.Parse(za.InnerText);
                return new Math.Vector3(x,y,z);
            }
            else
            {
                throw new DataMisalignedException("Connection has no offset settings");
            }
        }
    }
}
