using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EZ_B;
using FollowMe.Configuration;
using FollowMe.Enums;
using FollowMe.Interfaces;

namespace FollowMe.Autopilot
{
    public class Autopilot
    {
        private readonly ITargetLocator targetLocator;
        private readonly IFlyingRobot flyingRobot;
        private readonly Camera camera;
        private readonly TrackingConfig trackingConfig;
        private static readonly ILog Log = LogManager.GetLog(typeof(Autopilot));
        private int frameCounter = 0;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        static BackgroundWorker backgroundWorker;

        AutopilotCameraForm autopilotCameraForm = new AutopilotCameraForm();

        private Task cameraTask;
        public Autopilot(ITargetLocator targetLocator, IFlyingRobot flyingRobot, Camera camera, TrackingConfig trackingConfig)
        {
            if (targetLocator == null) throw new ArgumentNullException("targetLocator");
            if (flyingRobot == null) throw new ArgumentNullException("flyingRobot");
            if (camera == null) throw new ArgumentNullException("camera");
            if (trackingConfig == null) throw new ArgumentNullException("trackingConfig");
            this.targetLocator = targetLocator;
            this.flyingRobot = flyingRobot;
            this.camera = camera;
            this.trackingConfig = trackingConfig;

            cancellationTokenSource = new CancellationTokenSource();
            
           
        }

        internal void StartAutonomousFlight()
        {
            Log.Info("StartAutonomousFlight()");
            autopilotCameraForm.Show();
            cancellationToken = cancellationTokenSource.Token;
            Task.Factory.StartNew(GetTargetLocation, cancellationToken);

            camera.OnNewFrame += camera_OnNewFrame;


        }

        internal void StopAutonomousFlight()
        {
            Log.Info("StopAutonomousFlight()");


            cancellationTokenSource.Cancel();   
            cancellationTokenSource.Dispose();


            autopilotCameraForm.Hide();
            autopilotCameraForm.Dispose();
        }


        void GetTargetLocation()
        {
            while (true)
            {
                // Poll on this property if you have to do 
                // other cleanup before throwing. 
                if (cancellationToken.IsCancellationRequested)
                {
                    Log.Info("Cancel requested");
                    // Clean up here, then...
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
                }

                var targetLocation = targetLocator.GetTargetLocation(trackingConfig, true);
                Log.Info("TargetLocation = ", targetLocation);

                switch (targetLocation)
                {
                        case TargetLocation.BottomRight:
                            Log.Info("Pitch down, yaw right");
                        break;
                        case TargetLocation.BottomLeft:
                            Log.Info("Pitch down, yaw left");
                        break;
                        case TargetLocation.BottomCenter:
                            Log.Info("Pitch down");
                        break;
                        case TargetLocation.CenterRight:
                            Log.Info("yaw right");
                        break;
                        case TargetLocation.CenterLeft:
                            Log.Info("yaw left");
                        break;
                        case TargetLocation.CenterCenter:
                            Log.Info(" OK - hold ");
                        break;
                        case TargetLocation.TopRight:
                            Log.Info("pitch up, yaw right");
                        break;
                        case TargetLocation.TopLeft:
                            Log.Info("pitch up, yaw left");
                        break;
                        case TargetLocation.TopCenter:
                            Log.Info("pitch up");
                        break;
                }
                Thread.Sleep(100);
            }
            //camera.OnNewFrame += camera_OnNewFrame;
        }

        void camera_OnNewFrame()
        {
            Log.Info("Frame {0}", frameCounter++);
        }
    }
}
