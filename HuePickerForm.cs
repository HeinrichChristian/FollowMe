using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Controls;
using Caliburn.Micro;
using FollowMe.Messages;

namespace FollowMe
{
    public partial class HuePickerForm : Form
    {
        private readonly IEventAggregator eventAggregator;
        private HuePicker huePicker;

        public HuePickerForm(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            this.eventAggregator = eventAggregator;
            InitializeComponent();
            huePicker1.ValuesChanged += huePicker1_ValuesChanged;
        }

        void huePicker1_ValuesChanged(object sender, EventArgs e)
        {
            //this.hueMaxTextBox.Text = huePicker1.Max.ToString();
            //this.hueMinTextBox.Text = huePicker1.Min.ToString();
            eventAggregator.Publish(new HuePickerMessage(huePicker1), action =>
            {
                Task.Factory.StartNew(action);

            });
        }

        private void btnUebernehmen_Click(object sender, EventArgs e)
        {
            var value = this.huePicker1.Value;
            eventAggregator.Publish(new HuePickerMessage(huePicker1), action =>
            {
                Task.Factory.StartNew(action);

            });
        

    ;
        }
    }
}
