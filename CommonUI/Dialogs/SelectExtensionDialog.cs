using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUI.Dialogs
{
    public class SelectExtensionDialog : Dialog<string>
    {

        public SelectExtensionDialog()
            : base()
        {

            Title = "Select Extension...";            

            BuildUI();

        }

        private DropDown _dropExtensions;

        private string[] _extensions = null;
        public string[] Extensions 
        {
            get { return _extensions; }
            set
            {
                _extensions = value;
                UpdateBindings();
            }
        }

        private void BuildUI()
        {
            _dropExtensions = new DropDown { DataStore = Extensions };

            Button buttonOk = new Button { Text = "Select" };
            buttonOk.Click += (sender, e) =>
            {
                Result = _dropExtensions.SelectedValue != null ? _dropExtensions.SelectedValue.ToString() : null;
                this.Close();
            };
            Button buttonNo = new Button { Text = "Cancel" };
            buttonNo.Click += (sender, e) => { Result = null; this.Close(); };

            this.AbortButton = buttonNo;
            this.DefaultButton = buttonOk;

            Content = new TableLayout(new TableRow(_dropExtensions), new TableRow(new TableLayout(new TableRow(new TableCell(buttonOk) { ScaleWidth = true }, new TableCell(buttonNo)))) { ScaleHeight = false });

            Size = new Eto.Drawing.Size(400,90);//Autosizing freaks out!
        }

        public override void UpdateBindings(BindingUpdateMode mode = BindingUpdateMode.Source)
        {
            _dropExtensions.DataStore = Extensions;
            base.UpdateBindings(mode);
        }

    }
}
