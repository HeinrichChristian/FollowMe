﻿using System;
using Caliburn.Micro;
using EZ_B;
using FollowMe.Enums;
using FollowMe.Interfaces;

namespace FollowMe.EzRobot
{
    public class EzRobotCameraTargetLocator : ITargetLocator
    {
        private readonly Camera camera;
        private static readonly ILog Log = LogManager.GetLog(typeof(EzRobotCameraTargetLocator));

        public EzRobotCameraTargetLocator(Camera camera)
        {
            if (camera == null) throw new ArgumentNullException("camera");
            this.camera = camera;
        }

        public TargetLocation GetTargetLocation(bool trackingPreviewEnabled, int searchObjectSizePixels, int hueMin, int hueMax, float saturationMin, float saturationMax, float luminanceMin, float luminanceMax)
        {
            var targetLocation = TargetLocation.Unknown;
            ObjectLocation objectLocation = null;
            try
            {
                objectLocation = camera.CameraCustomColorDetection.GetObjectLocationByColor(
                                    trackingPreviewEnabled,
                                    searchObjectSizePixels, 
                                    hueMin, 
                                    hueMax,
                                    saturationMin, 
                                    saturationMax, 
                                    luminanceMin, 
                                    luminanceMax);
        
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            camera.UpdatePreview(255);


            if (objectLocation != null && objectLocation.IsObjectFound)
            {


                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Bottom)
                {
                    targetLocation = TargetLocation.BottomLeft;
                }

                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Middle &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Bottom)
                {
                    targetLocation = TargetLocation.BottomCenter;
                }

                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Bottom)
                {
                    targetLocation = TargetLocation.BottomRight;
                }

                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Middle)
                {
                    targetLocation = TargetLocation.CenterLeft;
                }

                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Middle &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Middle)
                {
                    targetLocation = TargetLocation.CenterCenter;
                }

                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Middle)
                {
                    targetLocation = TargetLocation.CenterRight;
                }
                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Top)
                {
                    targetLocation = TargetLocation.TopLeft;
                }

                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Middle &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Top)
                {
                    targetLocation = TargetLocation.TopCenter;
                }
                
                if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right &&
                    objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Top)
                {
                    targetLocation = TargetLocation.TopRight;
                }


                //TargetXCoordinate = objectLocation.CenterX;
                Log.Info("Object detected: X = {0}", objectLocation.CenterX);
                //TargetYCoordinate = objectLocation.CenterY;
                Log.Info("Object detected: Y = {0}", objectLocation.CenterY);
            }

            return targetLocation;
        }
    }
}
