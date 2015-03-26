using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using EZ_B;
using EZ_B.ARDrone;
using EZ_B.CameraDetection;
using EZ_B.Joystick;
using FollowMe.ViewModels;
using Timer = System.Timers.Timer;

namespace FollowMe {
    /// <summary>
    /// The viewModel for the shellview.
    /// </summary>
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private readonly IWindowManager windowManager;
        private static readonly ILog Log = LogManager.GetLog(typeof(ShellViewModel));

        #region privates

        /// <summary>
        /// This value is send to the drone
        /// </summary>
        private const float MoveSensitivivivity = 0.20f;

        /// <summary>
        /// After sending a move command this amount of milliseconds the thread sleeps
        /// </summary>
        private const int MoveSleepTimeMilliseconds = 400;

        private Joystick joystick;
        private Camera camera;
        private readonly Timer arDroneStatusTimer;
        private readonly UCEZB_Connect ezbConnect = new UCEZB_Connect();
        private int batteryLevel;
        private bool ardroneAccessPointVisible;
        private bool button1Pressed;
        private bool button2Pressed;
        private bool button3Pressed;
        private bool button4Pressed;
        private bool button5Pressed;
        private bool button6Pressed;
        private bool button7Pressed;
        private bool button8Pressed;
        private float leftStickXAxis;
        private float leftStickYAxis;
        private float rightStickXAxis;
        private float rightStickYAxis;
        private float targetXCoordinate;
        private float targetYCoordinate;
        private List<JoystickDevice> availableJoystickDevices;
        private string qrCode;
        private Control cameraPanel = new Control();
        private JoystickDevice selectedJoystickDevice;
        private string droneStatus;
        private readonly CameraForm cameraForm;
        private bool connectToDroneEnabled;
        private string selectedJoystick;
        private float maxYaw;
        private int maxVerticalSpeed;
        private float maxEulerAngle;
        private int maxAltitude;
        private bool isOutside;
        private bool flyingWithoutShell;

        #endregion

     

        #region public properties

        /// <summary>
        /// The battery level of the AR.Drone - value between 0 and 100, lower than 17 is not good
        /// </summary>
        public int BatteryLevel
        {
            get { return batteryLevel; }
            set
            {
                batteryLevel = value;
                NotifyOfPropertyChange(() => BatteryLevel);
            }
        }

        public bool ConnectToDroneEnabled
        {
            get { return connectToDroneEnabled; }
            set
            {
                connectToDroneEnabled = value;
                NotifyOfPropertyChange(() => ConnectToDroneEnabled);
            }
        }

        /// <summary>
        /// true, if the button 1 of the joystick is pressed
        /// </summary>
        public bool Button1Pressed
        {
            get { return button1Pressed; }
            set
            {
                button1Pressed = value;
                NotifyOfPropertyChange(() => Button1Pressed);
            }
        }
        
        /// <summary>
        /// true, if the button 2 of the joystick is pressed
        /// </summary>
        public bool Button2Pressed
        {
            get { return button2Pressed; }
            set
            {
                button2Pressed = value;
                NotifyOfPropertyChange(() => Button2Pressed);
            }
        }

        /// <summary>
        /// true, if the button 3 of the joystick is pressed
        /// </summary>
        public bool Button3Pressed
        {
            get { return button3Pressed; }
            set
            {
                button3Pressed = value;
                NotifyOfPropertyChange(() => Button3Pressed);
            }
        }

        /// <summary>
        /// true, if the button 2 of the joystick is pressed
        /// </summary>
        public bool Button4Pressed
        {
            get { return button4Pressed; }
            set
            {
                button4Pressed = value;
                NotifyOfPropertyChange(() => Button4Pressed);
            }
        }

        /// <summary>
        /// true, if the button 5 of the joystick is pressed
        /// </summary>
        public bool Button5Pressed
        {
            get { return button5Pressed; }
            set
            {
                button5Pressed = value;
                NotifyOfPropertyChange(() => Button5Pressed);
            }
        }

        /// <summary>
        /// true, if the button 6 of the joystick is pressed
        /// </summary>
        public bool Button6Pressed
        {
            get { return button6Pressed; }
            set
            {
                button6Pressed = value;
                NotifyOfPropertyChange(() => Button6Pressed);
            }
        }

        /// <summary>
        /// true, if the button 7 of the joystick is pressed
        /// </summary>
        public bool Button7Pressed
        {
            get { return button7Pressed; }
            set
            {
                button7Pressed = value;
                
                NotifyOfPropertyChange(() => Button7Pressed);
            }
        }

        /// <summary>
        /// true, if the button 8 of the joystick is pressed
        /// Used to TakeOff
        /// </summary>
        public bool Button8Pressed
        {
            get { return button8Pressed; }
            set
            {
                button8Pressed = value;
                NotifyOfPropertyChange(() => Button8Pressed);
            }
        }

        /// <summary>
        /// Yaw - X axis of left stick
        /// </summary>
        public float LeftStickXAxis
        {
            get { return leftStickXAxis; }
            set
            {
                leftStickXAxis = value; 
                NotifyOfPropertyChange(() => LeftStickXAxis);
            }
        }

        /// <summary>
        /// Pitch - Y axis of left stick
        /// </summary>
        public float LeftStickYAxis
        {
            get { return leftStickYAxis; }
            set
            {
                leftStickYAxis = value; 
                NotifyOfPropertyChange(() => LeftStickYAxis);
            }
        }

        /// <summary>
        /// Roll - X axis of right stick
        /// </summary>
        public float RightStickXAxis
        {
            get { return rightStickXAxis; }
            set
            {
                rightStickXAxis = value;
                NotifyOfPropertyChange(() => RightStickXAxis);
            }
        }

        /// <summary>
        /// Nick -  Y axis of right stick
        /// </summary>
        public float RightStickYAxis
        {
            get { return rightStickYAxis; }
            set
            {
                rightStickYAxis = value; 
                NotifyOfPropertyChange(() => RightStickYAxis);
            }
        }

        /// <summary>
        /// The X coordinate of a recognized target
        /// </summary>
        public float TargetXCoordinate
        {
            get { return targetXCoordinate; }
            set
            {
                targetXCoordinate = value; 
                NotifyOfPropertyChange(() => TargetXCoordinate);
            }
        }

        /// <summary>
        /// The Y coordinate of a recognized target
        /// </summary>
        public float TargetYCoordinate
        {
            get { return targetYCoordinate; }
            set
            {
                targetYCoordinate = value; 
                NotifyOfPropertyChange(() => TargetYCoordinate);
            }
        }

        /// <summary>
        /// The list of all actual available joysticks on the machine
        /// </summary>
        public List<JoystickDevice> AvailableJoystickDevices
        {
            get
            {
                if (availableJoystickDevices == null)
                {
                    availableJoystickDevices = new List<JoystickDevice>();
                    RefreshJoysticks();
                }
                return availableJoystickDevices;
            }
            set
            {
                availableJoystickDevices = value; 
                NotifyOfPropertyChange(() => AvailableJoystickDevices);
            }
        }

        /// <summary>
        /// The selected joystick
        /// </summary>
        public JoystickDevice SelectedJoystickDevice
        {
            get { return selectedJoystickDevice; }
            set
            {
                selectedJoystickDevice = value;
                if (value != null)
                {
                    ActivateJoystick();
                }
                SelectedJoystick = selectedJoystickDevice.Name;
                NotifyOfPropertyChange(() => SelectedJoystickDevice);
            }
        }

        public string SelectedJoystick
        {
            get { return selectedJoystick; }
            set
            {
                selectedJoystick = value; 
                NotifyOfPropertyChange(() => SelectedJoystick);
            }
        }

        /// <summary>
        /// The text of therecognized QR-Code
        /// </summary>
        public string QrCode
        {
            get { return qrCode; }
            set
            {
                qrCode = value; 
                NotifyOfPropertyChange(() => QrCode);
            }
        }

        /// <summary>
        /// The control to show the camera view
        /// </summary>
        public Control CameraPanel
        {
            get { return cameraPanel; }
            set
            {
                cameraPanel = value;
                NotifyOfPropertyChange(() => CameraPanel);
            }
        }

        public string DroneStatus
        {
            get { return droneStatus; }
            set
            {
                droneStatus = value;
                NotifyOfPropertyChange(() => DroneStatus);
            }
        }


        

        #endregion
        #region Settings

        /// <summary>
        /// Set true if you are flying outside 
        /// ->  Method:EZ_B.AR­Drone.­Drone­Control.­Set­Is­Outside(­System.­Boolean) 
        /// </summary>
        public bool IsOutside
        {
            get { return isOutside; }
            set
            {
                isOutside = value; 
                NotifyOfPropertyChange(() => IsOutside);
            }
        }

        /// <summary>
        /// Set to TRUE if you are flying with the outside shell 
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Is­Flying­Without­Shell(­System.­Boolean) 
        /// </summary>
        public bool FlyingWithoutShell
        {
            get { return flyingWithoutShell; }
            set
            {
                flyingWithoutShell = value;
                NotifyOfPropertyChange(() => FlyingWithoutShell);
            }
        }

        /// <summary>
        /// Maximum yaw (spin) speed of the AR.Drone, in radians per second. 
        /// Recommanded values goes from (0.7) 40/s to (6.11) 350/s. Others values may cause instability. Default: 3.0 
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Yaw(­System.­Single) 
        /// </summary>
        public float MaxYaw
        {
            get { return maxYaw; }
            set
            {
                maxYaw = value;
                NotifyOfPropertyChange(() => MaxYaw);
            }
        }

        /// <summary>
        /// Maximum vertical speed of the AR.Drone, in milimeters per second. 
        /// Recommanded values goes from 200 to 2000. Others values may cause instability. Default: 1000 
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­SetVZ­Max(­System.­Int32) 
        /// </summary>
        public int MaxVerticalSpeed
        {
            get { return maxVerticalSpeed; }
            set
            {
                maxVerticalSpeed = value; 
                NotifyOfPropertyChange(() => MaxVerticalSpeed);
            }
        }

        /// <summary>
        /// Set maximum bending angle for drone in radians for pitch and roll. 
        /// I.E. Maximum angle for going forward, back, left or right.
        /// This does not affect YAW (spin) Floating point between 0 (0 deg) and 0.52 (32 deg) Default: 0.25 
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Euler­Angle­Max(­System.­Single) 
        /// </summary>
        public float MaxEulerAngle
        {
            get { return maxEulerAngle; }
            set
            {
                maxEulerAngle = value;
                NotifyOfPropertyChange(() => MaxEulerAngle);
            }
        }

        /// <summary>
        /// Maximum drone altitude in millimeters.
        /// Give an integer value between 500 and 5000 to prevent the drone from flying above this limit, 
        /// or set it to 10000 to let the drone fly as high as desired. Default: 3000 
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Altitude­Max(­System.­Int32)
        /// </summary>
        public int MaxAltitude
        {
            get { return maxAltitude; }
            set
            {
                maxAltitude = value;
                NotifyOfPropertyChange(() => MaxAltitude);
            }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// Starts a timer which checks every 5 seconds the battery level of the connected AR.Drone
        /// </summary>
        [ImportingConstructor]
        public ShellViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            //this.ardroneAccessPointVisible = ardroneAccessPointVisible;
            Log.Info("Init");
            ezbConnect.EZB.ShowDebugWindow();
            camera = new Camera(ezbConnect.EZB);
            camera.OnNewFrame += _camera_OnNewFrame;

            RefreshJoysticks();

            cameraForm = new CameraForm();
            cameraForm.Show();

            ConnectToDroneEnabled = true;

            arDroneStatusTimer = new Timer();
            arDroneStatusTimer.Elapsed += OnArDroneStatusTimedEvent;
            arDroneStatusTimer.Interval = 5000;
            arDroneStatusTimer.Enabled = true;
        }

        /// <summary>
        /// Connect to AR.Drone.
        /// The PC must first be connected via WIFI to the AR.Drone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonConnect(object sender, RoutedEventArgs e)
        {
            try
            {
                ezbConnect.EZB.ARDrone.Connect(ARDrone.ARDroneVersionEnum.V2);
                 var controlConfig = ezbConnect.EZB.ARDrone.GetControlConfig();
                Log.Info(controlConfig);
                if (!string.IsNullOrEmpty(controlConfig) && ezbConnect.EZB.ARDrone.IsConnected)
                {
                    DroneStatus = "Verbunden";
                    ConnectToDroneEnabled = false;
                }
                else
                {
                    DroneStatus = "Nicht Verbunden";
                }
            }
            catch (Exception exception)
            {
                DroneStatus = "Fehler " + exception;
                Log.Error(exception);
            }
        }

        /// <summary>
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Yaw(­System.­Single) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendMaxYaw(object sender, RoutedEventArgs e)
        {
            Log.Info("SetYaw: {0}", MaxYaw);
            ezbConnect.EZB.ARDrone.SetYaw(MaxYaw);
            
        }

        /// <summary>
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­SetVZ­Max(­System.­Int32) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendMaxVerticalSpeed(object sender, RoutedEventArgs e)
        {
            Log.Info("SetVZMax: {0}", MaxVerticalSpeed);
            ezbConnect.EZB.ARDrone.SetVZMax(MaxVerticalSpeed);
        }

        /// <summary>
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Euler­Angle­Max(­System.­Single)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendMaxEulerAngle(object sender, RoutedEventArgs e)
        {
            Log.Info("SetEulerAngleMax: {0}", MaxEulerAngle);
            ezbConnect.EZB.ARDrone.SetEulerAngleMax(MaxEulerAngle);
        }

        /// <summary>
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Altitude­Max(­System.­Int32)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendMaxAltitude(object sender, RoutedEventArgs e)
        {
            Log.Info("SetAltitudeMax: {0}", MaxAltitude);
            ezbConnect.EZB.ARDrone.SetAltitudeMax(MaxAltitude);
        }

        /// <summary>
        ///  ->  Method:EZ_B.AR­Drone.­Drone­Control.­Set­Is­Outside(­System.­Boolean) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendIsOutside(object sender, RoutedEventArgs e)
        {
            Log.Info("SetIsOutside: {0}", IsOutside);
            ezbConnect.EZB.ARDrone.SetIsOutside(IsOutside);
        }

        /// <summary>
        /// -> Method:EZ_B.AR­Drone.­Drone­Control.­Set­Is­Flying­Without­Shell(­System.­Boolean) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendFlyingWithoutShell(object sender, RoutedEventArgs e)
        {
            Log.Info("SetIsFlyingWithoutShell: {0}", FlyingWithoutShell);
            ezbConnect.EZB.ARDrone.SetIsFlyingWithoutShell(FlyingWithoutShell);
        }
     
        /// <summary>
        /// Start the camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonShowCamera(object sender, RoutedEventArgs e)
        {
            Log.Info("StartCamera");
            camera.StartCamera(
                  new ValuePair(Camera.CAMERA_NAME_AR_DRONE, Camera.CAMERA_NAME_AR_DRONE),
                    cameraForm.ExternalCameraPanel,
                  320,
                  240);

            ezbConnect.EZB.ARDrone.StartVideo();
        }

        /// <summary>
        /// Disconnect from AR.Drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonDisconnect(object sender, RoutedEventArgs e)
        {
            Log.Info("Disconnect");
            ezbConnect.EZB.ARDrone.Disconnect();
        }

        /// <summary>
        /// Play LED animation on AR.Drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonBlinkLeds(object sender, RoutedEventArgs e)
        {
            Log.Info("PlayLedAnimation");
            ezbConnect.EZB.ARDrone.PlayLedAnimation(Commands.LedAnimationEnum.BlinkRed, 2, 10);
        }

        public void ButtonShowActualConfigOfArDrone(object sender, RoutedEventArgs e)
        {
            Log.Info("GetControlConfig");
            var controlConfig = ezbConnect.EZB.ARDrone.GetControlConfig();
            
            Log.Info(controlConfig);
            this.windowManager.ShowWindow(new ControlConfigViewModel(controlConfig));
        }

        /// <summary>
        /// Refresh the joystick list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonRefreshJoysticks(object sender, RoutedEventArgs e)
        {
            RefreshJoysticks();
        }


        private void OnArDroneStatusTimedEvent(object source, ElapsedEventArgs e)
        {
            
            //ConnectToDroneEnabled = IsArdroneNetworkVisible();
            BatteryLevel = ezbConnect.EZB.ARDrone.CurrentNavigationData.BatteryLevel;
        }

        private void RefreshJoysticks()
        {
            if (joystick != null)
            {
                joystick.Dispose();
                joystick = null;
            }

            AvailableJoystickDevices.Clear();
            var availableDevices = Joystick.GetAvailableDevices();
            if (availableDevices != null)
            {
                foreach (var availableDevice in availableDevices)
                {
                    AvailableJoystickDevices.Add(availableDevice);
                }
            }
        }

        /// <summary>
        /// Activate the selected joystick for EZB.
        /// Define method _joystick_OnControllerAction to be called on a controller action.
        /// </summary>
        private void ActivateJoystick()
        {
            JoystickDevice joystickDevice = SelectedJoystickDevice;
            joystick = new Joystick(joystickDevice, ezbConnect.EZB);
            joystick.OnControllerAction += _joystick_OnControllerAction;
            joystick.StartEventWatcher();
        }

        /// <summary>
        /// Called on a controller action.
        /// Check every button if pressed and every stick if moved and execute the defined methods.
        /// </summary>
        private void _joystick_OnControllerAction()
        {
            // Button 1 -> blink LEDs
            if (joystick.ButtonPressed(0))
            {
                Button1Pressed = true;
                if (ezbConnect.EZB.ARDrone.IsConnected)
                    ezbConnect.EZB.ARDrone.PlayLedAnimation(Commands.LedAnimationEnum.BlinkRed, 2, 10);
            }
            else
            {
                Button1Pressed = false;
            }

            // Button 2
            Button2Pressed = joystick.ButtonPressed(1);

            Button3Pressed = joystick.ButtonPressed(2);

            Button4Pressed = joystick.ButtonPressed(3);

            Button5Pressed = joystick.ButtonPressed(4);

            Button6Pressed = joystick.ButtonPressed(5);

            //  Must be called before take-off (start engines). Must be called on a flat surface. This flattens the trim values for the surface. 
            if (joystick.ButtonPressed(6))
            {
                Button7Pressed = true;
                ezbConnect.EZB.ARDrone.SetFlatTrim();
            }
            else
            {
                Button7Pressed = false;
            }

            // Button 8 -> Takeoff (deadman switch)
            if (joystick.ButtonPressed(7))
            {
                ezbConnect.EZB.ARDrone.TakeOff();
                Button8Pressed = true;
            }
            else
            {
                ezbConnect.EZB.ARDrone.Land();
                Button8Pressed = false;
            }

            // left stick, X axis -> yaw
            if (joystick.AxisXStateChanged())
            {
                LeftStickXAxis = joystick.GetAxisX;

                //ezB_Connect1.EZB.ARDrone.SetProgressiveInputValues(0, 0, 0, joystick.GetAxisX);
                //Thread.Sleep(moveSleepTimeMilliseconds);
                //ezB_Connect1.EZB.ARDrone.Hover();

            }
            // left stick, Y axis -> pitch
            if (joystick.AxisYStateChanged())
            {
                LeftStickYAxis = joystick.GetAxisY;

                if (joystick.GetAxisY > 0.3)
                {
                    Log.Info("joystick.GetAxisY {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisY, 0, 0, -MoveSensitivivivity, 0);
                    ezbConnect.EZB.ARDrone.SetProgressiveInputValues(0, 0, -MoveSensitivivivity, 0);
                    Thread.Sleep(MoveSleepTimeMilliseconds);
                    ezbConnect.EZB.ARDrone.Hover();
                }

                if (joystick.GetAxisY < - 0.3)
                {
                    Log.Info("joystick.GetAxisY {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisY, 0, 0, MoveSensitivivivity, 0);
                    ezbConnect.EZB.ARDrone.SetProgressiveInputValues(0, 0, MoveSensitivivivity, 0);
                    Thread.Sleep(MoveSleepTimeMilliseconds);
                    ezbConnect.EZB.ARDrone.Hover();
                }
            }

            // right stick, X axis-> roll
            if (joystick.AxisZStateChanged())
            {
                RightStickXAxis = joystick.GetAxisZ;

                if (joystick.GetAxisZ > 0.3)
                {
                    Log.Info("joystick.GetAxisZ {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisZ, -MoveSensitivivivity, 0, 0, 0);
                    ezbConnect.EZB.ARDrone.SetProgressiveInputValues(-MoveSensitivivivity, 0, 0, 0);
                    Thread.Sleep(MoveSleepTimeMilliseconds);
                    ezbConnect.EZB.ARDrone.Hover();
                }

                if (joystick.GetAxisZ < -0.3)
                {
                    Log.Info("joystick.GetAxisZ {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisZ, MoveSensitivivivity, 0, 0, 0);
                    ezbConnect.EZB.ARDrone.SetProgressiveInputValues(MoveSensitivivivity, 0, 0, 0);
                    Thread.Sleep(MoveSleepTimeMilliseconds);
                    ezbConnect.EZB.ARDrone.Hover();
                }


            }
            // right stick, Y axis -> nick
            if (joystick.AxisRzStateChanged())
            {
                RightStickYAxis = joystick.GetAxisRz;

                if (joystick.GetAxisRz > 0.3)
                {
                    Log.Info("joystick.GetAxisRz {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisRz, 0, MoveSensitivivivity, 0, 0);
                    ezbConnect.EZB.ARDrone.SetProgressiveInputValues(0, MoveSensitivivivity, 0, 0);
                    Thread.Sleep(MoveSleepTimeMilliseconds);
                    ezbConnect.EZB.ARDrone.Hover();
                }

                if (joystick.GetAxisRz < -0.3)
                {
                    Log.Info("joystick.GetAxisRz {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisRz, 0, -MoveSensitivivivity, 0, 0);
                    ezbConnect.EZB.ARDrone.SetProgressiveInputValues(0, -MoveSensitivivivity, 0, 0);
                    Thread.Sleep(MoveSleepTimeMilliseconds);
                    ezbConnect.EZB.ARDrone.Hover();
                }

                
            }
        }

        /// <summary>
        /// New camera frame available, so we look again for markers
        /// </summary>
        void _camera_OnNewFrame()
        {
            camera.UpdatePreview();
            try
            {
                ObjectLocation objectLocationByColor = camera.CameraBasicColorDetection.GetObjectLocationByColor(true, ColorDetection.ColorEnum.Red, 50, 80);
                if (objectLocationByColor != null)
                {
                    Debug.WriteLine(objectLocationByColor.VerticalLocation.ToString());
                    TargetXCoordinate = objectLocationByColor.CenterX;
                    TargetYCoordinate = objectLocationByColor.CenterY;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            try
            {
                ObjectLocation objectLocationByQrCode = camera.CameraQRCodeDetection.GetObjectLocationByQRCode();

                if (objectLocationByQrCode != null)
                {
                    QrCode = objectLocationByQrCode.QRCodeText;
                    Debug.WriteLine(objectLocationByQrCode.QRCodeText);
                }
                else
                {
                    QrCode = string.Empty;
                }


                //    if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left)
                //        btnLeft_Click(null, null);
                //    else if (objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right)
                //        btnRight_Click(null, null);
                //}
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

        }
    }
}