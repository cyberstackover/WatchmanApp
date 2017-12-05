using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Microsoft.Phone.Shell;
using System.Windows.Controls;
using System.Net;
using BluetoothClientWP8.Resources;

namespace BluetoothClientWP8 
{
    public partial class ContactMenu : PhoneApplicationPage
    {

        // Constructor
        public ContactMenu()
        {
            InitializeComponent();
            // anda bisa mengganti SlideTransitionMode sesuai dengan keinginan
            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeIn };
            navigateInTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeOut };
            navigateOutTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);
        }

        ProgressIndicator prog;

        private void btnLogin_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            string url = "http://roboviper.besaba.com/imaginecup/json/checkLogin.php";
            Uri uri = new Uri(url, UriKind.Absolute);

            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "sUsername", HttpUtility.UrlEncode(this.txtUsername.Text));
            postData.AppendFormat("&{0}={1}", "sPassword", HttpUtility.UrlEncode(this.txtPassword.Password.ToString()));

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

        private void btnInsert_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InsertData.xaml", UriKind.Relative));
        }

        private void btnView_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ContactList.xaml", UriKind.Relative));
        }

        private void client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            //Me.txtResult.Text = "Uploading.... " & e.ProgressPercentage & "%"
        }

        private void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Cancelled == false & e.Error == null)
            {
                prog.IsVisible = false;

                string[] result = e.Result.ToString().Split('|');
                string strStatus = result[0].ToString();
                string strMemberID = result[1].ToString();
                string strError = result[2].ToString();

                if (strStatus == "0")
                {
                    MessageBox.Show(strError);
                }
                else
                {
                    NavigationService.Navigate(new Uri("/DetailPage.xaml?sMemberID=" + strMemberID, UriKind.Relative));
                }

            }
        }


    }

}
