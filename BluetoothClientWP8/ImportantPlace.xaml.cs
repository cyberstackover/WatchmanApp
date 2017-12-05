using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace BluetoothClientWP8
{
    public partial class ImportantPlace : PhoneApplicationPage
    {
        public ImportantPlace()
        {
            InitializeComponent();
        }
        private void Pemadam_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = "113";
            phoneCallTask.DisplayName = "Fire Fighter / Pemadam Kebakaran";
            phoneCallTask.Show(); 
        }

        private void btnPolisi_Click(object sender, RoutedEventArgs e)
        {
           PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = "112";
            phoneCallTask.DisplayName = "Police / Kantor Polisi";
            phoneCallTask.Show();
  
        }
 

        private void btnHospital_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = "119";
            phoneCallTask.DisplayName = "Hospital / Rumah Sakit";
            phoneCallTask.Show();
        }
        private void SarButton_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = "02165701116";
            phoneCallTask.DisplayName = "Rescue / SAR";
            phoneCallTask.Show();
        }
        private void btnPln_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = "123";
            phoneCallTask.DisplayName = "Electrical / PLN";
            phoneCallTask.Show();
        }
        private void DerekButton_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = "02180880123";
            phoneCallTask.DisplayName = "Tow Vehicle / Kendaraan Derek";
            phoneCallTask.Show();
        }
    }
}