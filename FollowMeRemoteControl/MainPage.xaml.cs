using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FollowMeRemoteControl.Resources;
using FollowMeRemoteControl.ViewModels;


namespace FollowMeRemoteControl
{
    public partial class MainPage : PhoneApplicationPage
    {

        RemoteControlClient remoteControlClient = new RemoteControlClient();
        
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            // Datenkontext des Steuerelements LongListSelector auf die Beispieldaten festlegen
            DataContext = App.ViewModel;

            // Beispielcode zur Lokalisierung der ApplicationBar
            //BuildLocalizedApplicationBar();
        }



        public void ButtonStopClick(object sender, RoutedEventArgs e)
        {
            remoteControlClient.StopCompleted += remoteControlClient_StopCompleted;
            remoteControlClient.BeginStop(new AsyncCallback(TaskCompleted), null);
        }

        void remoteControlClient_StopCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
        }

        public void ButtonStartClick(object sender, RoutedEventArgs e)
        {
            remoteControlClient.StartCompleted += remoteControlClient_StartCompleted;
            remoteControlClient.BeginStart(new AsyncCallback(TaskCompleted), null);
        }

        void remoteControlClient_StartCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
        }


        public void TaskCompleted(IAsyncResult R)
        {
            // Write here code to handle the completion of
            // your asynchronous method
        }
    }
}