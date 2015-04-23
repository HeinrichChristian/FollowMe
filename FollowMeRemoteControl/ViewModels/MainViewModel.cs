using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FollowMeRemoteControl.Resources;
using System.Windows;

namespace FollowMeRemoteControl.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
         
        }

        /// <summary>
        /// Eine Auflistung für ItemViewModel-Objekte.
        /// </summary>
     
        private bool personDetected;

        private bool dangerDetected;

        private string personLocation;

        

        public bool PersonDetected
        {
            get
            {
                return personDetected;
            }

            set
            {
                personDetected = value;
                NotifyPropertyChanged("PersonDetected");
            }
        }

        public bool DangerDetected
        {
            get
            {
                return dangerDetected;
            }

            set
            {
                dangerDetected = value;
                NotifyPropertyChanged("DangerDetected");
            }

        }

       
        public string PersonLocation
        {
            get { return personLocation; }
            
            set
            {
                personLocation = value;
                NotifyPropertyChanged("PersonLocation");
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Erstellt einige ItemViewModel-Objekte und fügt diese zur Items-Auflistung hinzu.
        /// </summary>
        public void LoadData()
        {
            // TODO connect to service

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}