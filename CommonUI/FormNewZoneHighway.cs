using CommonUI.Widgets;
using Eto.Forms;
using Itaros.Math;
using Itaros.XRebirth;
using Itaros.XRebirth.Content.DOMEntities;
using Itaros.XRebirth.Content.Specialized;
using Itaros.XRebirth.Manipulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUI
{
    public class FormNewZoneHighway : Form
    {

        public MainForm Host { get; private set; }

        public ProjectInfo ProjectContext
        {
            get { return DataContext as ProjectInfo; }
        }

        public FormNewZoneHighway(MainForm host)
            : base()
        {
            Host = host;

            //Resizable = false;
            Maximizable = false;

            Width = 350;

            BuildUI();
        }

        private DropDown _dropSector;

        private DropDown _dropStartZone;
        private DropDown _dropEndZone;

        private GroupBox _groupStep3CfgBox;

        private void BuildUI()
        {
            _dropSector = new DropDown();
            _dropSector.SelectedValueChanged += OnStep1Selected;
            GroupBox groupStep1 = new GroupBox { Text = "Step 1: Select Sector" };
            groupStep1.Content = _dropSector;

            _dropStartZone = new DropDown();
            _dropEndZone = new DropDown();
            GroupBox groupStep2 = new GroupBox{Text = "Step 2: Select START and END zone connections"};
            groupStep2.Content = new TableLayout(_dropStartZone, _dropEndZone);

            GroupBox groupStep3 = new GroupBox { Text = "Step 3: Set Gate Rules" };
            _groupStep3CfgBox = new GroupBox { Text = "Options" };
            DropDown dropGateRules = new DropDown() { Items = { "Relative", "Arctangent" } };
            dropGateRules.SelectedValueChanged += (sender, e) => {
                BuildStep3UI(dropGateRules.SelectedValue.ToString());
            };
            groupStep3.Content = new TableLayout(new TableRow(dropGateRules), new TableRow(_groupStep3CfgBox) { ScaleHeight = true });

            Button buttonCommence = new Button { Text = "Commit!" };
            buttonCommence.Click += buttonCommence_Click;

            Content = new TableLayout(new TableRow(groupStep1), new TableRow(groupStep2), new TableRow(groupStep3) { ScaleHeight = true }, new TableRow(buttonCommence) { ScaleHeight = false });
        }

        private void BuildStep3UI(string p)
        {
            switch (p)
            {
                default:
                    throw new InvalidOperationException();
                case "Relative":

                    CoordinateInput ciEntryGate = new CoordinateInput { Text = "Entry Gate" };
                    ciEntryGate.OnValueChanged += ciEntryGate_OnValueChanged;
                    CoordinateInput ciExitGate = new CoordinateInput { Text = "Exit Gate" };
                    ciExitGate.OnValueChanged += ciExitGate_OnValueChanged;


                    _groupStep3CfgBox.Content = new TableLayout(new TableRow(ciEntryGate), new TableRow(ciExitGate), null);

                    break;
                case "Arctangent":

                    _groupStep3CfgBox.Content = new TableLayout(new TableRow(new Label { Text = "Not yet implemented"}), null);

                    break;
            }
        }

        void ciExitGate_OnValueChanged(object sender, EventArgs e)
        {
            _exitZoneGatePosition = (sender as CoordinateInput).Value;
        }

        void ciEntryGate_OnValueChanged(object sender, EventArgs e)
        {
            _entryZoneGatePosition = (sender as CoordinateInput).Value;
        }

        private Vector3 _entryZoneGatePosition;
        private Vector3 _exitZoneGatePosition;

        void buttonCommence_Click(object sender, EventArgs e)
        {
            //Creating DOM Entities
            CreateZoneHighway action = new CreateZoneHighway(ProjectContext.DataContainers.CurrentMap)
            {
                HighwayNumber=3,
                Sector = _currentSelectedSector,
                StartZoneConnection = _dropStartZone.SelectedValue as ConnectionEnvelope,
                EndZoneConnection = _dropEndZone.SelectedValue as ConnectionEnvelope,
                PositionStartZoneGate = _entryZoneGatePosition,
                PositionExitZoneGate = _exitZoneGatePosition
            };
            action.Apply();
        }

        private SectorsXMLDOM.SectorNodeEnvelope _currentSelectedSector;

        void OnStep1Selected(object sender, EventArgs e)
        {
            _currentSelectedSector = _dropSector.SelectedValue as SectorsXMLDOM.SectorNodeEnvelope;
            if (_currentSelectedSector != null)
            {
                _dropStartZone.DataStore = _currentSelectedSector.Connections;
                _dropEndZone.DataStore = _currentSelectedSector.Connections;
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            UpdateProjectContext();
        }

        private void UpdateProjectContext()
        {
            if (ProjectContext != null && ProjectContext.DataContainers.CurrentMap != null)
            {
                _dropSector.DataStore = ProjectContext.DataContainers.CurrentMap.Sectors.Sectors;

            }
            else
            {
                //TODO: Set to null
            }
        }

    }
}
