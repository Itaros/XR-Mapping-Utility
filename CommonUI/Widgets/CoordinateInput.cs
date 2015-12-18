using Eto.Forms;
using Itaros.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUI.Widgets
{
    public class CoordinateInput : Panel
    {

        public CoordinateInput()
            : base()
        {

            BuildUI();
        }

        private TextBox _xBox;
        private TextBox _yBox;
        private TextBox _zBox;

        private string _text;
        public string Text {
            get { return _text; }
            set
            {
                _text = value;
                UpdateFieldLabel();
            }
        }

        private Label _fieldLabel;

        private void BuildUI()
        {

            _xBox = new TextBox();
            _yBox = new TextBox();
            _zBox = new TextBox();

            _xBox.TextChanged += OnValueXChanged;
            _yBox.TextChanged += OnValueYChanged;
            _zBox.TextChanged += OnValueZChanged;

            _fieldLabel = new Label();
            UpdateFieldLabel();

            Content = new TableLayout(new TableRow(
                new TableCell(_fieldLabel)
                ),
                new TableRow(new TableLayout(new TableRow(
                    new TableCell(_xBox) { ScaleWidth = true },
                    new TableCell(_yBox) { ScaleWidth = true },
                    new TableCell(_zBox) { ScaleWidth = true })
                ))
                );

        }

        private Vector3 _internalVector=new Vector3();

        public Vector3 Value { get { return _internalVector; } set { _internalVector = value; } }

        public event EventHandler OnValueChanged;

        private void OnValueXChanged(object sender, EventArgs e)
        {
            TextBox caller = sender as TextBox;
            decimal output;
            if (decimal.TryParse(caller.Text, out output))
            {
                _internalVector.X = output;
            }
            if (OnValueChanged != null) { OnValueChanged(this, EventArgs.Empty); }
        }
        private void OnValueYChanged(object sender, EventArgs e)
        {
            TextBox caller = sender as TextBox;
            decimal output;
            if (decimal.TryParse(caller.Text, out output))
            {
                _internalVector.Y = output;
            }
            if (OnValueChanged != null) { OnValueChanged(this, EventArgs.Empty); }
        }
        private void OnValueZChanged(object sender, EventArgs e)
        {
            TextBox caller = sender as TextBox;
            decimal output;
            if (decimal.TryParse(caller.Text, out output))
            {
                _internalVector.Z = output;
            }
            if (OnValueChanged != null) { OnValueChanged(this, EventArgs.Empty); }
        }


        private void UpdateFieldLabel()
        {
            _fieldLabel.Text = "Coordinates(xyz)["+_text+"]: ";
        }

    }
}
