using System;
using System.Windows.Forms;
using AForge.Controls;

namespace FollowMe
{
    public partial class HuePickerForm : Form
    {
        private HuePicker huePicker;

        public HuePickerForm()
        {
            InitializeComponent();
            huePicker1.ValuesChanged += huePicker1_ValuesChanged;
        }

        void huePicker1_ValuesChanged(object sender, EventArgs e)
        {
            this.hueMaxTextBox.Text = huePicker1.Max.ToString();
            this.hueMinTextBox.Text = huePicker1.Min.ToString();
        }

        private void btnUebernehmen_Click(object sender, EventArgs e)
        {
            var value = this.huePicker1.Value;
        }
    }
}
