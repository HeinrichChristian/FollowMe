using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowMe.WebService
{
    /// <summary>
    /// RemoteControl to control the Ar.Drone indirectly.
    /// You can start and stop the person following.
    /// </summary>
    public class RemoteControl : IRemoteControl
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(RemoteControl));

        /// <summary>
        /// Start the person following
        /// </summary>
        public void Start()
        {
            Log.Info("Start");
            
        }

        /// <summary>
        /// Stop the Person following
        /// </summary>
        public void Stop()
        {
            Log.Info("Stop");
            
        }


        public Enums.TargetLocation GetPersonLocation()
        {
            Log.Info("GetPersonLocation");
            return Enums.TargetLocation.Unknown;
        }

        public Enums.TargetLocation GetDangerLocation()
        {
            Log.Info("GetDangerLocation");
            return Enums.TargetLocation.Unknown;
        }
    }
}
