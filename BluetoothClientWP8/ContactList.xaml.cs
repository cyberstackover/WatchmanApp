using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Microsoft.Phone.Shell;
using System.Net;

namespace BluetoothClientWP8
{
    public partial class ContactList : PhoneApplicationPage
    {

        // Constructor
        public ContactList()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        WebClient client;
        ProgressIndicator prog;

        private void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string url = "http://roboviper.besaba.com/imaginecup/json/getJSON.php";
            Uri uri = new Uri(url);
            client = new WebClient();
            client.AllowReadStreamBuffering = true;

            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;

            client.DownloadStringAsync(uri);

            //*** SystemTray ProgressBar ***'
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Downloading....";
            SystemTray.SetProgressIndicator(this, prog);
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {


            if (e.Cancelled == false & e.Error == null)
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Result));
                ObservableCollection<Customer> list = new ObservableCollection<Customer>();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Customer>));
                list = (ObservableCollection<Customer>)serializer.ReadObject(ms);

                List<Customer> myCustomer = new List<Customer>();

                foreach (Customer cm in list)
                {
                   /* string sCustomerID = cm.CustomerID.ToString();
                    string sName = cm.Name.ToString();
                    string sEmail = cm.Email.ToString();
                    string sCountryCode = cm.CountryCode.ToString();
                    string sBudget = cm.Budget.ToString();
                    string sUsed = cm.Used.ToString();

                    myCustomer.Add(new Customer(sCustomerID, sName, sEmail, sCountryCode, sBudget, sUsed));
                */
                    string sMemberID = cm.MemberID.ToString();
                    string sUsername = cm.Username.ToString();
                    string sPassword = cm.Password.ToString();
                    string sName = cm.Name.ToString();
                    string sTel = cm.Tel.ToString();
                    string sEmail = cm.Email.ToString();

                    myCustomer.Add(new Customer(sMemberID, sUsername, sPassword, sName, sTel, sEmail));
              

                     }

                this.CustomerList.ItemsSource = myCustomer;

                prog.IsVisible = false;
            }

        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

        }

    }

    [DataContract]
    public class Customer
    {
        [DataMember]
        public string MemberID { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Tel { get; set; }

        [DataMember]
        public string Email { get; set; }

        public Customer(string strMemberID, string strUsername, string strPassword
            , string strName
            , string strTel
            , string strEmail) 
        {
            this.MemberID = strMemberID;
            this.Username = strUsername;
            this.Password = strPassword;
            this.Name = strName;
            this.Tel = strTel;
            this.Email = strEmail;
        }

    }
}
