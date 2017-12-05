using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO;
using System.Text;
using Microsoft.Phone.Shell;
using System.Net;
using Parse;
using Microsoft.Phone.Notification;

namespace BluetoothClientWP8
{
    public partial class InsertData : PhoneApplicationPage
    {

        // Constructor
        public InsertData()
        {
            InitializeComponent();
        }

        ProgressIndicator prog;


        private void btnSave_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUsername.Text))
            {
                MessageBox.Show("Please input (Username)");
                return;
            }
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
            string url = "http://roboviper.besaba.com/imaginecup/json/saveData.php";
            Uri uri = new Uri(url, UriKind.Absolute);

            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "sUsername", HttpUtility.UrlEncode(this.txtUsername.Text));
            postData.AppendFormat("&{0}={1}", "sPassword", HttpUtility.UrlEncode(this.txtPassword.Password.ToString()));
            postData.AppendFormat("&{0}={1}", "sName", HttpUtility.UrlEncode(this.txtName.Text));
            postData.AppendFormat("&{0}={1}", "sTel", HttpUtility.UrlEncode(this.txtTel.Text));
            postData.AppendFormat("&{0}={1}", "sEmail", HttpUtility.UrlEncode(this.txtEmail.Text));

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

        private void btnSaveParse_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {

            // Create a new Parse object
            var post = new ParseObject("Installation");
            post["userName"] = "Herwindra";
            post["userPhone"] = "081939115544";
            // Save it to Parse
            //await post.SaveAsync();
            
        }

        private void client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            //Me.txtResult.Text = "Uploading.... " & e.ProgressPercentage & "%"
        }

        private void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Cancelled == false & e.Error == null)
            {

                string[] result = e.Result.ToString().Split('|');
                //*** Check Status
                if (result[0].ToString() == "0")
                {
                    MessageBox.Show(result[1].ToString());
                }
                else
                {
                    MessageBox.Show("Save Data Successfully");
                    this.txtUsername.Text = "";
                    this.txtPassword.Password = "";
                    this.txtConPassword.Password = "";
                    this.txtName.Text = "";
                    this.txtTel.Text = "";
                    this.txtEmail.Text = "";
                }

                prog.IsVisible = false;
            }
        }

    }
}
