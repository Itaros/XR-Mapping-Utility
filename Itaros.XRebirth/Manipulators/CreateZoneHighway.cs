using Itaros.Math;
using Itaros.XRebirth.Content;
using Itaros.XRebirth.Content.DOMEntities;
using Itaros.XRebirth.Content.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Manipulators
{
    public class CreateZoneHighway
    {

        public MapInformation Map { get; set; }

        public CreateZoneHighway(MapInformation data)
        {
            Map = data;
        }

        public SectorsXMLDOM.SectorNodeEnvelope Sector { get; set; }
        public ConnectionEnvelope StartZoneConnection { get; set; }
        public ConnectionEnvelope EndZoneConnection { get; set; }

        public Vector3 PositionStartZoneGate { get; set; }
        public Vector3 PositionExitZoneGate { get; set; }

        public int HighwayNumber { get; set; }
        public string HighwayNumberString
        {
            get
            {
                return HighwayNumber.ToString("D2");
            }
        }

        public void Apply()
        {
            //0)Configuring Vectors
            Vector3 positionStartZone = StartZoneConnection.GetPositionVector();
            Vector3 positionEndZone = EndZoneConnection.GetPositionVector();
            Vector3 highwayReferencePoint = (positionEndZone - positionStartZone) / 2;//I have no idea...
            //1) We need to resolve zone connections to actual zones
            ZonesXMLDOM.ZoneEnvelope StartZone = StartZoneConnection.ConnectsTo as ZonesXMLDOM.ZoneEnvelope;
            ZonesXMLDOM.ZoneEnvelope EndZone = EndZoneConnection.ConnectsTo as ZonesXMLDOM.ZoneEnvelope;
            //2)Let's commit gates.
            //Example connection:
            //<connection ref="highway01connection01_gate">
            //  <offset>
            //    <position x="400" y="0" z="15000" />
            //  </offset>
            //</connection>
            XmlNode gateconnection01 = GetGateConnectionNode("01", PositionStartZoneGate);
            StartZone.AddToConnections(gateconnection01);
            XmlNode gateconnection02 = GetGateConnectionNode("02", PositionExitZoneGate);
            EndZone.AddToConnections(gateconnection02);

            //[position of the zone in the sector] + [position of the gate in the zone] - [position of the highway definition in the sector]
            Vector3 positionEntrypointRelative = positionStartZone + PositionStartZoneGate - highwayReferencePoint;
            Vector3 positionExitpointRelative = positionEndZone + PositionExitZoneGate - highwayReferencePoint;
            //3)Creating zonehighways entry
              //<macro name="bftp_Highway01_macro" class="highway">
              //  <component ref="standardzonehighway" />
              //  <connections>
              //    <connection ref="entrypoint">
              //      <offset>
              //        <position x="400" y="0" z="-135000" />
              //      </offset>
              //    </connection>
              //    <connection ref="exitpoint">
              //      <offset>
              //        <position x="400" y="0" z="125000" />
              //      </offset>
              //    </connection>
              //  </connections>
              //  <properties>
              //    <boundaries>
              //      <boundary class="splinetube">
              //        <splineposition x="0" y="0" z="-135000" tx="0" ty="0" tz="0" weight="0" inlength="0" outlength="1" />
              //        <splineposition x="0" y="0" z="125000" tx="0" ty="0" tz="0" weight="0" inlength="0" outlength="1" />
              //        <size r="200" />
              //      </boundary>
              //    </boundaries>
              //    <controls>
              //      <linear>
              //        <time />
              //      </linear>
              //      <angular>
              //        <roll />
              //      </angular>
              //    </controls>
              //    <identification name="bftp_Highway01" />
              //    <configuration ref="cluster_c_localhighwayconfiguration" />
              //  </properties>
              //</macro>
            string highwayName = "bftp_" + "Highway" + HighwayNumberString;
            XmlNode highwayentry = Map.Zonehighways.CreateRawNode("macro");
            XmlAttribute nameAttribute = Map.Zonehighways.CreateRawAttribute("name", highwayName + "_macro");
            XmlAttribute classAttribute = Map.Zonehighways.CreateRawAttribute("class", "highway");
            highwayentry.Attributes.Append(nameAttribute);
            highwayentry.Attributes.Append(classAttribute);
            XmlNode hwcomponent = Map.Zonehighways.CreateRawNode("component");
            XmlAttribute hwattrib = Map.Zonehighways.CreateRawAttribute("ref", "standardzonehighway");
            hwcomponent.Attributes.Append(hwattrib);
            highwayentry.AppendChild(hwcomponent);
            XmlNode hwconnections = Map.Zonehighways.CreateRawNode("connections");
            hwconnections.InnerXml = "<connection ref=\"entrypoint\"><offset>" + positionEntrypointRelative.GetXRPositionXML()+ "</offset></connection><connection ref=\"exitpoint\"><offset>"+positionExitpointRelative.GetXRPositionXML()+"</offset></connection>";
            highwayentry.AppendChild(hwconnections);
            XmlNode hwproperties = Map.Zonehighways.CreateRawNode("properties");
            hwproperties.InnerXml = "<boundaries><boundary class=\"splinetube\"><splineposition " + positionEntrypointRelative.GetXRArguments() + " tx=\"0\" ty=\"0\" tz=\"0\" weight=\"0\" inlength=\"0\" outlength=\"1\" /><splineposition " + positionExitpointRelative.GetXRArguments() + " tx=\"0\" ty=\"0\" tz=\"0\" weight=\"0\" inlength=\"0\" outlength=\"1\" /><size r=\"200\" /></boundary></boundaries><controls><linear><time/></linear><angular><roll/></angular></controls>";
            highwayentry.AppendChild(hwproperties);
            XmlNode hwidentification = Map.Zonehighways.CreateRawNode("identification");
            XmlAttribute hwidentification_name = Map.Zonehighways.CreateRawAttribute("name", highwayName);
            hwidentification.Attributes.Append(hwidentification_name);
            XmlNode hwconfiguration = Map.Zonehighways.CreateRawNode("configuration");
            XmlAttribute hwconfiguration_ref = Map.Zonehighways.CreateRawAttribute("ref", "cluster_c_localhighwayconfiguration");
            hwconfiguration.Attributes.Append(hwconfiguration_ref);
            highwayentry.AppendChild(hwidentification);
            highwayentry.AppendChild(hwconfiguration);
            Map.Zonehighways.AddToMacros(highwayentry);
            //4)Creating sector connection
              //<connection name="bftp_highway01_connection" ref="zonehighways">
              //  <offset>
              //    <position x="-25166" y="0" z="150000" />
              //  </offset>
              //  <macro ref="bftp_highway01_macro" connection="sector">
              //    <connections>
              //      <connection ref="entrypoint">
              //        <macro path="../../tzonecluster_ita_bftp_00_00_00_connection" connection="highway01connection01_gate" />
              //      </connection>
              //      <connection ref="exitpoint">
              //        <macro path="../../tzonecluster_ita_bftp_00_00_01_connection" connection="highway01connection02_gate" />
              //      </connection>
              //    </connections>
              //  </macro>
              //</connection>
            XmlNode sconnection = Map.Sectors.CreateRawNode("connection");
            XmlAttribute sconnection_name = Map.Sectors.CreateRawAttribute("name", highwayName + "_connection");
            XmlAttribute sconnection_ref = Map.Sectors.CreateRawAttribute("ref", "zonehighways");
            sconnection.Attributes.Append(sconnection_name);
            sconnection.Attributes.Append(sconnection_ref);
            sconnection.InnerXml = "<offset>" + highwayReferencePoint.GetXRPositionXML() + "</offset><macro ref=\"" + highwayName + "_macro" + "\" connection=\"sector\"><connections><connection ref=\"entrypoint\"><macro path=\"../../" + StartZoneConnection.InternalName + "\" connection=\"highway" + HighwayNumberString + "connection01_gate\" /></connection><connection ref=\"exitpoint\"><macro path=\"../../" + EndZoneConnection.InternalName + "\" connection=\"highway" + HighwayNumberString + "connection02_gate\" /></connection></connections></macro>";
            Sector.AddToConnections(sconnection);

        }

        private XmlNode GetGateConnectionNode(string identity, Vector3 gatePosition)
        {
            XmlNode gateconnection = Map.Zones.CreateRawNode("connection");
            XmlAttribute refAttribute = Map.Zones.CreateRawAttribute("ref", "highway" + HighwayNumberString + "connection" + identity + "_gate");
            gateconnection.Attributes.Append(refAttribute);
            XmlNode offset = Map.Zones.CreateRawNode("offset");
            offset.InnerXml=gatePosition.GetXRPositionXML();
            gateconnection.AppendChild(offset);
            return gateconnection;
        }

    }
}
