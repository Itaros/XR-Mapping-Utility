using CommonUI.Dialogs;
using Eto.Forms;
using Itaros.XRebirth;
using Itaros.XRebirth.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUI
{
    public class MainForm : Form
    {

        public ProjectInfo Project { get; private set; }

        public MainForm()
            : base()
        {
            Project = new ProjectInfo(
                null,
                null
                );

            Project.FillData();

            Title = "XRebirth Mapping Utility";

            BuildUI();

        }

        private FormManagerZoneHighways _formManagerZoneHighways;

        private GroupBox _mapInspectorGroup;
        private GridView<XMLDOMData> _mapFileStatusView;

        private GroupBox _highwayActionsGroup;

        private void BuildUI()
        {
            TextBox labelBindedXRPath = new TextBox() { DataContext = Project, Enabled = false };
            if (Project.XRebirth != null)
            {
                labelBindedXRPath.TextBinding.BindDataContext<ProjectInfo>(m => m.XRebirth.PathToGame.ToString());
            }
            TextBox labelBindedExtensionName = new TextBox() { DataContext = Project, Enabled = false };
            if (Project.Extension != null)
            {
                labelBindedExtensionName.TextBinding.BindDataContext<ProjectInfo>(m => m.Extension.ExtensionName.ToString());
            }

            Button buttonSelectXRPath = new Button { Text = "Browse..." };
            buttonSelectXRPath.Click += (sender, e) =>
                {
                    FileDialog selectXR = new OpenFileDialog { CheckFileExists = true };
                    selectXR.Filters.Add(new FileDialogFilter("XRebirth.exe", "XRebirth.exe"));
                    var result = selectXR.ShowDialog(this);
                    if (result == DialogResult.Ok)
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(selectXR.FileName);
                        Project.XRebirth = new XRebirthInfo(info.Directory.FullName);
                        Project.FillData();
                        BuildUI();//I am so cheap...
                    }
                };

            Button buttonSelectExtension = new Button { Text = "Select..." };
            buttonSelectExtension.Click += (sender, e) =>
                {
                    SelectExtensionDialog dialog = new SelectExtensionDialog { Extensions = Project.XRebirth.GetExtensionNames() };
                    string selected = dialog.ShowModal(this);
                    if (selected != null)
                    {
                        Project.Extension = new ExtensionInfo(selected);
                        Project.FillData();
                        BuildUI();//I am so cheap...
                    }
                };

            GroupBox groupProjectInfo = new GroupBox() { Text = "Project Info" };
            groupProjectInfo.Content = new TableLayout(new TableRow(new Label() { Text = "Current Project:" }), new TableRow(new TableLayout(new TableRow(new TableCell(labelBindedXRPath) { ScaleWidth = true }, new TableCell(buttonSelectXRPath)))), new TableRow(new TableLayout(new TableRow(new TableCell(labelBindedExtensionName) { ScaleWidth = true }, new TableCell(buttonSelectExtension)))));

            DropDown boxSelectMap = new DropDown() { DataContext = Project, DataStore = Project.DataContainers.PresentMapGroups };
            boxSelectMap.SelectedValueChanged += boxSelectMap_SelectedValueChanged;

            _mapInspectorGroup = new GroupBox(){ DataContext = Project };
            _mapInspectorGroup.BindDataContext<GroupBox, ProjectInfo, string>(c => c.Text, m => m.DataContainers.CurrentMapName.ToString(), DualBindingMode.OneWay);
            _mapFileStatusView = new GridView<XMLDOMData>();
            _mapFileStatusView.Columns.Add(
                new GridColumn()
                {
                    DataCell = new TextBoxCell() { Binding = Binding.Property<XMLDOMData,string>(m=>m.Filename) },
                    HeaderText = "Filename"
                });
            _mapFileStatusView.Columns.Add(
                new GridColumn()
                {
                    DataCell = new CheckBoxCell() { Binding = Binding.Property<XMLDOMData, bool>(m => m.IsValid).Convert(v => (bool?)v, v => v == null ? false : (bool)v) },
                    HeaderText = "Is Available"
                });
            _mapInspectorGroup.Content = new TableLayout(_mapFileStatusView);

            GroupBox groupMapSelectorInfo = new GroupBox() { Text = "Galaxy Map" };
            groupMapSelectorInfo.Content = new TableLayout(new TableRow(boxSelectMap), new TableRow(_mapInspectorGroup));

            Button manageSectorHighwaysButton = new Button() { Text = "Manage Zone Highways" };
            manageSectorHighwaysButton.Click += (sender, e) => { OpenFormZoneHighwaysManager(); };

            Button manageZoneHighwayGates = new Button() { Text = "Manage Zone Highway Gates", Enabled = false };

            Button applyDomButton = new Button { Text = "Apply DOM" };
            applyDomButton.Click += applyDomButton_Click;

            _highwayActionsGroup = new GroupBox() { Text = "Actions" };
            _highwayActionsGroup.Content = new TableLayout(new TableRow(manageSectorHighwaysButton), new TableRow(manageZoneHighwayGates), new TableRow { ScaleHeight = true }, new TableRow(applyDomButton));

            TableLayout masterLayout = new TableLayout(new TableRow(groupProjectInfo), new TableRow(groupMapSelectorInfo), new TableRow(_highwayActionsGroup));


            UpdateProjectContext();
            Content = masterLayout;

            Width = 350;
            Height = 500;
        }

        void applyDomButton_Click(object sender, EventArgs e)
        {
            Project.FlushDOM();
        }

        public void OpenFormZoneHighwaysManager()
        {
            _formManagerZoneHighways = new FormManagerZoneHighways(this) { DataContext = Project };
            _formManagerZoneHighways.Show();
        }

        private FormNewZoneHighway _formNewZoneHighway;
        public void OpenFormNewZoneHighway()
        {
            _formNewZoneHighway = new FormNewZoneHighway(this) { DataContext = Project };
            _formNewZoneHighway.Show();
        }

        private void UpdateProjectContext()
        {
            _mapFileStatusView.DataStore = Project.DataContainers.CurrentMap != null ? Project.DataContainers.CurrentMap.AllXMLDOMFiles : null;
        }

        void boxSelectMap_SelectedValueChanged(object sender, EventArgs e)
        {
            var widget = sender as DropDown;
            var context = widget.DataContext as ProjectInfo;

            context.DataContainers.CurrentMapName = widget.SelectedValue as string;
            _mapInspectorGroup.UpdateBindings(BindingUpdateMode.Destination);

            context.FillCurrentMapInfo();

            UpdateProjectContext();
        }

    }
}
