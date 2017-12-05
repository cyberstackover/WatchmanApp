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
using System.Windows.Controls;
using System.Net;
using BluetoothClientWP8.Resources;

namespace BluetoothClientWP8
{
    public partial class DetailPage : PhoneApplicationPage
    {
        public DetailPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        ProgressIndicator prog;
        string strMemberID = "";


        private void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string url = "http://roboviper.besaba.com/imaginecup/json/getByMemberID.php";
            Uri uri = new Uri(url, UriKind.Absolute);


            NavigationContext.QueryString.TryGetValue("sMemberID", out strMemberID);

            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "sMemberID", HttpUtility.UrlEncode(strMemberID));

            WebClient client = default(WebClient);
            client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            client.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();

            client.UploadStringCompleted += client_UploadStringCompleted;
            client.UploadProgressChanged += client_UploadProgressChanged;

            client.UploadStringAsync(uri, "POST", postData.ToString());

            prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.IsVisible = true;
            prog.Text = "Loading....";
            SystemTray.SetProgressIndicator(this, prog);

        }

        private void client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            //Me.txtResult.Text = "Uploading.... " & e.ProgressPercentage & "%"
        }

        private void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {

            if (e.Cancelled == false && e.Error == null)
            {
                Member myMember = ReadToObject(e.Result.ToString());
                this.txtUsername.Text = myMember.Username.ToString();
                this.txtPassword.Password = myMember.Password.ToString();
                this.txtConPassword.Password = myMember.Password.ToString();
                this.txtName.Text = myMember.Name.ToString();
                this.txtTel.Text = myMember.Tel.ToString();
                this.txtEmail.Text = myMember.Email.ToString();

                prog.IsVisible = false;
            }
        }

        public static Member ReadToObject(string json)
        {
            Member deserializedMember = new Member();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedMember.GetType());
            deserializedMember = ser.ReadObject(ms) as Member;
            ms.Close();
            return deserializedMember;
        }

        private void btnDelete_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            /* disini program untuk DELETE */
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.Text = "Updated...";
            SystemTray.SetProgressIndicator(this, prog); 
        }

        private void btnSave_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPassword.Password.ToString()))
            {
                MessageBox.Show("Please input (Password)");
                return;
            }
            if (string.IsNullOrEmpty(this.txtConPassword.Password.ToString()))
            {
                MessageBox.Show("Please input (Confirm Username)");
                return;
            }
            if (this.txtPassword.Password.ToString() != this.txtConPassword.Password.ToString())
            {
                MessageBox.Show("Password Not Match!");
                return;
            }
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                MessageBox.Show("Please input (Name)");
                return;
            }
            if (string.IsNullOrEmpty(this.txtTel.Text))
            {
                MessageBox.Show("Please input (Tel)");
                return;
            }
            if (string.IsNullOrEmpty(this.txtEmail.Text))
            {
                MessageBox.Show("Please input (Email)");
                return;
            }

            string url = "http://roboviper.besaba.com/imaginecup/json/updateData.php";
            Uri uri = new Uri(url, UriKind.Absolute);


            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "sMemberID", HttpUtility.UrlEncode(strMemberID));
            postData.AppendFormat("&{0}={1}", "sPassword", HttpUtility.UrlEncode(this.txtPassword.Password.ToString()));
            postData.AppendFormat("&{0}={1}", "sName", HttpUtility.UrlEncode(this.txtName.Text));
            postData.AppendFormat("&{0}={1}", "sTel", HttpUtility.UrlEncode(this.txtTel.Text));
            postData.AppendFormat("&{0}={1}", "sEmail", HttpUtility.UrlEncode(this.txtEmail.Text));

            WebClient client = default(WebClient);
            client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            client.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();

            client.UploadStringCompleted += client_SaveUploadStringCompleted;
            client.UploadProgressChanged += client_SaveUploadProgressChanged;

            client.UploadStringAsync(uri, "POST", postData.ToString());

            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.Text = "Connecting update to server....";
            SystemTray.SetProgressIndicator(this, prog);
        }

        private void client_SaveUploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {

            if (e.Cancelled == false && e.Error == null)
            {
                string[] result = e.Result.ToString().Split('|');
                //*** Check Status
                if (result[0].ToString() == "0")
                {
                    MessageBox.Show(result[1].ToString());
                }
                else
                {
                    MessageBox.Show("Update Data Successfully");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }

                prog.IsVisible = false;
            }
        }

        private void client_SaveUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            //Me.txtResult.Text = "Uploading.... " & e.ProgressPercentage & "%"
        }
    }

    [DataContract]
    public class Member
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

        public Member()
        {
        }

        public Member(string strMemberID, string strUsername, string strPassword
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
