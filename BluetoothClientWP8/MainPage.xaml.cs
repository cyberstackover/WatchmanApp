using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BluetoothClientWP8.Resources;
using Windows.Networking.Sockets;
using Windows.Networking.Proximity;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Threading;
using BluetoothConnectionManager;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation; //Provides the Geocoordinate class.
//using Windows.Services.Maps; error

namespace BluetoothClientWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ConnectionManager connectionManager;
        private StateManager stateManager;
        GeoCoordinate currentLocation = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //Thread.Sleep(2000);
            // anda bisa mengganti SlideTransitionMode sesuai dengan keinginan
            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeIn };
            navigateInTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeOut };
            navigateOutTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);

            connectionManager = new ConnectionManager();
            connectionManager.MessageReceived += connectionManager_MessageReceived;
            stateManager = new StateManager();          
            // Get current location.
            GetLocation();
        }

        private async void btnFindCoordinate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var locator = new Geolocator();

            if (!locator.LocationStatus.Equals(PositionStatus.Disabled))
            {
                var position = await locator.GetGeopositionAsync();
                tbkLatitudeValue.Text = position.Coordinate.Latitude.ToString();
                tbkLongitudeValue.Text = position.Coordinate.Longitude.ToString();
               
               // tbkAddress.Text = "town = " + result.Locations[0].Address.Town;
               // tbkAddress.Text = "town = " + position.CivicAddress.City.ToString();
        
            }

            else
            {
                MessageBox.Show("Service Geolocation not enabled!", AppResources.ApplicationTitle, MessageBoxButton.OK);
                return;
            }
        }

        private async void GetLocation()
        {
            // Get current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            currentLocation = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);
        }

        async void connectionManager_MessageReceived(string message)
        {
            Debug.WriteLine("Message received:" + message);
            string[] messageArray = message.Split(':');
            switch (messageArray[0])
            {
              /*  case "LED_RED":
                    stateManager.RedLightOn = messageArray[1] == "ON" ? true : false;
                    Dispatcher.BeginInvoke(delegate()
                    {
                        RedButton.Background = stateManager.RedLightOn ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
                    });
                break;
               */
            /*    case "LED_GREEN":
                    stateManager.GreenLightOn = messageArray[1] == "ON" ? true : false;
                    Dispatcher.BeginInvoke(delegate()
                    {
                        GreenButton.Background = stateManager.GreenLightOn ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Black);
                    });
                break;
              
             */
                /*  case "LED_YELLOW":
                    stateManager.YellowLightOn = messageArray[1] == "ON" ? true : false;
                    Dispatcher.BeginInvoke(delegate()
                    {
                        YellowButton.Background = stateManager.YellowLightOn ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Black);
                    });
                break;
               */ 
                case "PROXIMITY":
                    stateManager.BodyDetected = messageArray[1] == "DETECTED" ? true : false;
                    if (stateManager.BodyDetected)
                    {
                        Dispatcher.BeginInvoke(delegate()
                        {
                            BodyDetectionStatus.Text = "Danger !!!";
                            BodyDetectionStatus.Foreground = new SolidColorBrush(Colors.Red);

                            SmsComposeTask SMSCompose = new SmsComposeTask();
                            SMSCompose.To = "085731524564";
                            SMSCompose.Body = "I'am in danger now, my position at " + "http://www.bing.com/maps/default.aspx?cp=" + tbkLatitudeValue.Text + "~" + tbkLongitudeValue.Text + "&lvl=16&style=r&v=2";
                            SMSCompose.Show();
                        });
                        await connectionManager.SendCommand("TURN_ON_RED");
                } else {
                        Dispatcher.BeginInvoke(delegate()
                        {
                            BodyDetectionStatus.Text = "No body detected";
                            BodyDetectionStatus.Foreground = new SolidColorBrush(Colors.White);
                        });
                    }
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            connectionManager.Initialize();
            stateManager.Initialize();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            connectionManager.Terminate();
        }


        private void btnContact_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ContactMenu.xaml", UriKind.Relative));
        }

        private void ImportantPlace_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ImportantPlace.xaml", UriKind.Relative));
        }
        

        private void MapsButton_Click_1(System.Object sender, System.Windows.RoutedEventArgs e) 
        {
            NavigationService.Navigate(new Uri("/BingMaps.xaml", UriKind.Relative));
        }

        private void About(System.Object sender, System.Windows.RoutedEventArgs e) 
        {
            NavigationService.Navigate(new Uri("/AboutMenu.xaml", UriKind.Relative));
        }

        private void ConnectAppToDeviceButton_Click_1(object sender, RoutedEventArgs e)
        {
            AppToDevice();
        }

        private async void AppToDevice()
        {
            ConnectAppToDeviceButton.Content = "Connecting...";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            var pairedDevices = await PeerFinder.FindAllPeersAsync();

            if (pairedDevices.Count == 0)
            {
                Debug.WriteLine("No paired devices were found.");
            }
            else
            { 
                foreach (var pairedDevice in pairedDevices)
                {
                    if (pairedDevice.DisplayName == DeviceName.Text)
                    {
                        connectionManager.Connect(pairedDevice.HostName);
                        ConnectAppToDeviceButton.Content = "Connected";
                        DeviceName.IsReadOnly = true;
                        ConnectAppToDeviceButton.IsEnabled = false;
                        continue;
                    }           
                }
            }
        }

        private async void RedButton_Click_1(object sender, RoutedEventArgs e)
        {
            string command = stateManager.RedLightOn ? "TURN_OFF_RED" : "TURN_ON_RED";
            await connectionManager.SendCommand(command);
        }

        private async void GreenButton_Click_1(object sender, RoutedEventArgs e)
        {
            string command = stateManager.GreenLightOn ? "TURN_OFF_GREEN" : "TURN_ON_GREEN";
            await connectionManager.SendCommand(command);
        }

        private async void YellowButton_Click_1(object sender, RoutedEventArgs e)
        {
            string command = stateManager.YellowLightOn ? "TURN_OFF_YELLOW" : "TURN_ON_YELLOW";
            await connectionManager.SendCommand(command);
        }

        private void Broadcast(object sender, RoutedEventArgs e)
        {
            SmsComposeTask SMSCompose = new SmsComposeTask();
            SMSCompose.To = "08999511619";
            //SMSCompose.Body = "I'am in danger now, my position at " + "Latitude : " + tbkLatitudeValue.Text + "Longitude : " + tbkLongitudeValue.Text;
            SMSCompose.Body = "I'am in danger now, my position at " + "http://www.bing.com/maps/default.aspx?cp=" + tbkLatitudeValue.Text + "~" + tbkLongitudeValue.Text + "&lvl=16&style=r&v=2";
            SMSCompose.Show();
          
           // SMSCompose
        }
    }
}