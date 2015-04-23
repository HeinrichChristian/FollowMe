using Caliburn.Micro;
using FollowMe.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FollowMe.WebService
{
    /// <summary>
    /// RemoteControl to control the Ar.Drone indirectly.
    /// You can start and stop the person following.
    /// </summary>
    [ServiceBehavior(
    ConcurrencyMode=ConcurrencyMode.Single,
    InstanceContextMode=InstanceContextMode.Single)]
    public class RemoteControl : IRemoteControl
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(RemoteControl));
        private readonly IEventAggregator eventAggregator;

        public RemoteControl(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Start the person following
        /// </summary>
        public void Start()
        {
            Log.Info("Start - BECAUSE OF SECURITY REASONS NOT IMPLEMENTED");
            
        }

        /// <summary>
        /// Stop the Person following
        /// </summary>
        public void Stop()
        {
            Log.Info("Stop");
            eventAggregator.Publish(new StopMessage(), action =>
            {
                Task.Factory.StartNew(action);

            });
        }


        public Enums.TargetLocation GetPersonLocation()
        {
            Log.Info("GetPersonLocation");
            // TODO: logic
            return Enums.TargetLocation.CenterCenter;
        }

        public Enums.TargetLocation GetDangerLocation()
        {
            Log.Info("GetDangerLocation");
            // TODO: logic
            return Enums.TargetLocation.Unknown;
        }


        public PersonAndDangerLocation GetPersonAndDangerLocation()
        {
            Log.Info("GetPersonAndDangerLocation");
            // TODO: logic
            return new PersonAndDangerLocation { DangerLocation = Enums.TargetLocation.Unknown, PersonLocation = Enums.TargetLocation.CenterCenter };
        }
    }
}
