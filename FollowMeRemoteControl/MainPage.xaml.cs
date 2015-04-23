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
using System.Windows.Threading;
using FollowMeRemoteControl.FollowMeService;
using System.ComponentModel;


namespace FollowMeRemoteControl
{
    public partial class MainPage : PhoneApplicationPage
    {

        RemoteControlClient remoteControlClient = new RemoteControlClient();
        private DispatcherTimer pollTimer;
        private TargetLocation personLocation; 
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            // Datenkontext des Steuerelements LongListSelector auf die Beispieldaten festlegen
            DataContext = App.ViewModel;

            pollTimer = new System.Windows.Threading.DispatcherTimer();
            pollTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            pollTimer.Interval = new TimeSpan(0, 0, 1);
            pollTimer.Start();
            
        }
          
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // TODO: one call with both results
            try            
            {
                var asyncResult = remoteControlClient.BeginGetPersonLocation(new AsyncCallback(GetPersonLocationTaskCompleted), personLocation);
                
            }
            catch(Exception exception)
            {

            }
            
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

        public void GetPersonLocationTaskCompleted(IAsyncResult asyncResult)
        {         
            var result = remoteControlClient.EndGetPersonLocation(asyncResult);

            if (result.ToString() == TargetLocation.Unknown.ToString())
            {
                Dispatcher.BeginInvoke(
                    () =>
                    {
                        App.ViewModel.PersonDetected = false;
                        App.ViewModel.PersonLocation = string.Empty;
                    });
            }
            else
            {
                Dispatcher.BeginInvoke(
                    () =>
                    {
                        App.ViewModel.PersonDetected = true;
                        App.ViewModel.PersonLocation = result.ToString();
                    });

            }
        }    
    }
}