using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Controls;

namespace FollowMe.Messages
{
    public class HuePickerMessage
    {
        private readonly HuePicker huePicker;

        public HuePickerMessage(HuePicker huePicker)
        {
            this.huePicker = huePicker;
        }

        public HuePicker HuePicker
        {
            get { return huePicker; }
        }
    }
}
