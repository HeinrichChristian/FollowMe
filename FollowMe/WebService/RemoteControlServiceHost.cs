using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FollowMe.WebService
{
    public sealed class RemoteControlServiceHost
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(RemoteControlServiceHost));

        private static ServiceHost remoteControlServiceHost;

        private static readonly RemoteControlServiceHost instance = null;

        private static readonly object padlock = new object();


     
        private RemoteControlServiceHost()
        {
            Log.Info("private RemoteControlServiceHost()");
        }

        
        public static ServiceHost Instance
        {
            get
            {
                lock(padlock)
                {                    
                    if(remoteControlServiceHost == null)
                    {
                        remoteControlServiceHost = new ServiceHost(typeof(RemoteControl));
                        remoteControlServiceHost.Open();
                    }

                }
                return remoteControlServiceHost;
            }
        }
    }
}
