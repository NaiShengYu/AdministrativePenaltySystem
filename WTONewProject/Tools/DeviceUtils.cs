using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WTONewProject.Tools
{
    class DeviceUtils
    {
        public static void phone(string mobile)
        {
            if (mobile == null || "".Equals(mobile))
            {
                return;
            }
            Device.OpenUri(new Uri("tel:" + mobile));
        }

        public static void sms(string mobile)
        {
            if (mobile == null || "".Equals(mobile))
            {
                return;
            }
            Device.OpenUri(new Uri("sms:" + mobile));
        }
    }
}
