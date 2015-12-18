using Eto.Forms;
using Itaros.XRebirth;
using Itaros.XRebirth.Content.DOMEntities;
using Itaros.XRebirth.Content.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUI
{
    public class FormManagerZoneHighways : Form
    {

        public MainForm Host { get; private set; }

        public ProjectInfo ProjectContext {
            get { return DataContext as ProjectInfo; }
        }

        private GridView<ZonehighwaysXMLDOM.ZonehighwayEnvelope> _gridListing;

        public FormManagerZoneHighways(MainForm host)
            : base()
        {
            Host = host;

            _gridListing = new GridView<ZonehighwaysXMLDOM.ZonehighwayEnvelope>();
            _gridListing.Columns.Add(
                new GridColumn()
                {
                    HeaderText = "Highway Name",
                    DataCell = new TextBoxCell() { Binding = Binding.Property <ZonehighwaysXMLDOM.ZonehighwayEnvelope, string>(m=>m.InternalName)}
                });
            _gridListing.Columns.Add(
                new GridColumn()
                {
                    HeaderText = "Entrypoint",
                    DataCell = new TextBoxCell() { Binding = Binding.Delegate<ZonehighwaysXMLDOM.ZonehighwayEnvelope, string>(m=>((ConnectionZonehighwaysEnvelope)m.Connections[0]).Entrypoint.ConnectsTo.InternalName.ToString()) }
                });
            _gridListing.Columns.Add(
                new GridColumn()
                {
                    HeaderText = "Exitpoint",
                    DataCell = new TextBoxCell() { Binding = Binding.Delegate<ZonehighwaysXMLDOM.ZonehighwayEnvelope, string>(m => ((ConnectionZonehighwaysEnvelope)m.Connections[0]).Exitpoint.ConnectsTo.InternalName.ToString()) }
                });

            GroupBox groupKnownHighways = new GroupBox { Text = "Known Zone Highways" };
            groupKnownHighways.Content = _gridListing;

            Button buttonCreateNewHighway = new Button { Text = "Create new Zone Highway" };
            buttonCreateNewHighway.Click += (sender, e) => { Host.OpenFormNewZoneHighway(); };

            UpdateProjectContext();
            Content = new TableLayout(new TableRow(groupKnownHighways) { ScaleHeight = true }, new TableRow(buttonCreateNewHighway));
        }


        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            UpdateProjectContext();
        }

        private void UpdateProjectContext()
        {
            if (ProjectContext != null && ProjectContext.DataContainers.CurrentMap!=null)
            {
                _gridListing.DataStore = ProjectContext.DataContainers.CurrentMap.Zonehighways.ZoneHighways;
            }
            else
            {
                //TODO: Set to null
            }
        }

    }
}
