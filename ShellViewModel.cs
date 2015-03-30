using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using EZ_B;
using EZ_B.ARDrone;
using EZ_B.Joystick;
using FollowMe.ArDrone;
using FollowMe.Configuration;
using FollowMe.Messages;
using FollowMe.ViewModels;
using Timer = System.Timers.Timer;

namespace FollowMe {
    /// <summary>
    /// The viewModel for the shellview.
    /// </summary>
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : PropertyChangedBase, IShell, IHandle<HuePickerMessage>
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;
        private static readonly ILog Log = LogManager.GetLog(typeof(ShellViewModel));
        private readonly FileBasedTrackingConfigProvider fileBasedTrackingConfigProvider = new FileBasedTrackingConfigProvider();
        #region privates

        /// <summary>
        /// This value is send to the drone
        /// </summary>
        private const float MoveSensitivivivity = 0.20f;

        /// <summary>
        /// After sending a move command this amount of milliseconds the thread sleeps
        /// </summary>
        private const int MoveSleepTimeMilliseconds = 100;

        private Joystick joystick;
        private Camera camera;
        private readonly UCEZB_Connect ezbConnect = new UCEZB_Connect();
        private int batteryLevel;
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
        private CameraForm cameraForm;
        private HuePickerForm huePickerForm;
        private bool connectToDroneEnabled;
        private string selectedJoystick;
        private float maxYaw;
        private int maxVerticalSpeed;
        private float maxEulerAngle;
        private int maxAltitude;
        private bool isOutside;
        private bool flyingWithoutShell;
        private ArDroneConfig arDroneConfig = new ArDroneConfig();
        private bool objectDetectionEnabled;
        private bool cameraStarted;
        private int searchObjectSizePixels;
        private byte colorBrightness;
        private float saturationMin;
        private float saturationMax;
        private float luminanceMin;
        private float luminanceMax;
        private int hueMin;
        private int hueMax;
        private bool huePickerIsVisible;

        #endregion
        
        CameraPreviewForm cameraPreviewForm = new CameraPreviewForm();
        private bool trackingPreviewEnabled;
        private bool searchObjectTopLeft;
        private bool searchObjectTopCenter;
        private bool searchObjectTopRight;
        private bool searchObjectCenterLeft;
        private bool searchObjectCenterCenter;
        private bool searchObjectCenterRight;
        private bool searchObjectBottomLeft;
        private bool searchObjectBottomCenter;
        private bool searchObjectBottomRight;
        private string commandsForAutonomousFlight;
        private bool flyingAtonomous;

        #region public properties

        /// <summary>
        /// The Config of the AR.Drone
        /// </summary>
        public ArDroneConfig ArDroneConfig
        {
            get
            {
                return arDroneConfig;

            }
            set
            {

                arDroneConfig = value;
                NotifyOfPropertyChange(() => ArDroneConfig);
            }
        }

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
        /// If true, the detection of objects is enabled
        /// </summary>
        public bool ObjectDetectionEnabled
        {
            get { return objectDetectionEnabled; }
            set
            {
                objectDetectionEnabled = value;
                NotifyOfPropertyChange(() => ObjectDetectionEnabled);
            }
        }

        public bool TrackingPreviewEnabled
        {
            get { return trackingPreviewEnabled; }
            set
            {
                trackingPreviewEnabled = value;
                NotifyOfPropertyChange(() => TrackingPreviewEnabled);
            }
        }

        public bool CameraStarted
        {
            get { return cameraStarted; }
            set
            {
                cameraStarted = value;
                NotifyOfPropertyChange(() => CameraStarted);
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

        /// <summary>
        /// 
        /// </summary>
        public bool HuePickerIsVisible
        {
            get { return huePickerIsVisible; }
            set
            {
                huePickerIsVisible = value;
                NotifyOfPropertyChange(() => HuePickerIsVisible);
            }
        }

        #endregion

        #region Object tracking

        public int SearchObjectSizePixels
        {
            get { return searchObjectSizePixels; }
            set
            {
                searchObjectSizePixels = value;
                NotifyOfPropertyChange(() => SearchObjectSizePixels);
            }
        }

        public byte ColorBrightness
        {
            get { return colorBrightness; }
            set
            {
                colorBrightness = value; 
                NotifyOfPropertyChange(() => ColorBrightness);
            }
        }

        public float SaturationMin
        {
            get { return saturationMin; }
            set
            {
                saturationMin = value; 
                NotifyOfPropertyChange(() => SaturationMin);
            }
        }

        public float SaturationMax
        {
            get { return saturationMax; }
            set
            {
                saturationMax = value;
                NotifyOfPropertyChange(() => SaturationMax);
            }
        }

        public float LuminanceMin
        {
            get { return luminanceMin; }
            set
            {
                luminanceMin = value;
                NotifyOfPropertyChange(() => LuminanceMin);
            }
        }

        public float LuminanceMax
        {
            get { return luminanceMax; }
            set
            {
                luminanceMax = value;
                NotifyOfPropertyChange(() => LuminanceMax);
            }
        }

        public int HueMin
        {
            get { return hueMin; }
            set
            {
                hueMin = value;
                NotifyOfPropertyChange(() => HueMin);
            }
        }

        public int HueMax
        {
            get { return hueMax; }
            set
            {
                hueMax = value;
                NotifyOfPropertyChange(() => HueMax);
            }
        }

        #region Visualization of target in camera view
        public bool SearchObjectTopLeft
        {
            get { return searchObjectTopLeft; }
            set
            {
                searchObjectTopLeft = value;
                NotifyOfPropertyChange(() => SearchObjectTopLeft);
            }
        }

        public bool SearchObjectTopCenter
        {
            get { return searchObjectTopCenter; }
            set
            {
                searchObjectTopCenter = value;
                NotifyOfPropertyChange(() => SearchObjectTopCenter);
            }
        }

        public bool SearchObjectTopRight
        {
            get { return searchObjectTopRight; }
            set
            {
                searchObjectTopRight = value;
                NotifyOfPropertyChange(() => SearchObjectTopRight);
            }
        }

        public bool SearchObjectCenterLeft
        {
            get { return searchObjectCenterLeft; }
            set
            {
                searchObjectCenterLeft = value;
                NotifyOfPropertyChange(() => SearchObjectCenterLeft);
            }
        }

        public bool SearchObjectCenterCenter
        {
            get { return searchObjectCenterCenter; }
            set
            {
                searchObjectCenterCenter = value; 
                NotifyOfPropertyChange(() => SearchObjectCenterCenter);
            }
        }

        public bool SearchObjectCenterRight
        {
            get { return searchObjectCenterRight; }
            set
            {
                searchObjectCenterRight = value;
                NotifyOfPropertyChange(() => SearchObjectCenterRight);
            }
        }

        public bool SearchObjectBottomLeft
        {
            get { return searchObjectBottomLeft; }
            set
            {
                searchObjectBottomLeft = value; 
                NotifyOfPropertyChange(() => SearchObjectBottomLeft);
            }
        }

        public bool SearchObjectBottomCenter
        {
            get { return searchObjectBottomCenter; }
            set
            {
                searchObjectBottomCenter = value;
                NotifyOfPropertyChange(() => SearchObjectBottomCenter);
            }
        }

        public bool SearchObjectBottomRight
        {
            get { return searchObjectBottomRight; }
            set
            {
                searchObjectBottomRight = value;
                NotifyOfPropertyChange(() => SearchObjectBottomRight);
            }
        }
        #endregion

        #endregion

        #region Autonomous flight

        public string CommandsForAutonomousFlight
        {
            get { return commandsForAutonomousFlight; }
            set
            {
                commandsForAutonomousFlight = value; 
                NotifyOfPropertyChange(() => CommandsForAutonomousFlight);
            }
        }

        public bool FlyingAtonomous
        {
            get { return flyingAtonomous; }
            set
            {
                flyingAtonomous = value;
                NotifyOfPropertyChange(() => FlyingAtonomous);
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
        public ShellViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            if (windowManager == null) throw new ArgumentNullException("windowManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.Subscribe(this);

            Log.Info("Init");
            ezbConnect.EZB.ShowDebugWindow();
           
            RefreshJoysticks();

            ConnectToDroneEnabled = true;
            HuePickerIsVisible = false;

            var arDroneStatusTimer = new Timer();
            arDroneStatusTimer.Elapsed += OnArDroneStatusTimedEvent;
            arDroneStatusTimer.Interval = 5000;
            arDroneStatusTimer.Enabled = true;


            var trackingConfig = fileBasedTrackingConfigProvider.LoadTrackingConfig();

            SearchObjectSizePixels = trackingConfig.SearchObjectSizePixels;
            HueMax = trackingConfig.HueMax;
            HueMin = trackingConfig.HueMin;
            SaturationMax = trackingConfig.SaturationMax;
            SaturationMin = trackingConfig.SaturationMin;
            LuminanceMax = trackingConfig.LuminanceMax;
            LuminanceMin = trackingConfig.LuminanceMin;

        }

        #region buttons
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
                if (!string.IsNullOrEmpty(controlConfig))
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
            camera = new Camera(ezbConnect.EZB);
            camera.OnNewFrame += _camera_OnNewFrame;


            //cameraForm = new CameraForm();
            //cameraForm.Show();

            Log.Info("StartCamera");
            
            cameraPreviewForm.Show();
            camera.StartCamera(
                    new ValuePair(Camera.CAMERA_NAME_AR_DRONE, Camera.CAMERA_NAME_AR_DRONE),
                    cameraPreviewForm.panel1,
//                    cameraForm.ExternalCameraPanel,
                    
                    320,
                    240);
            //camera.QuadBottomY = 33;
            CameraStarted = true;
        }

        public void ButtonStopCamera(object sender, RoutedEventArgs e)
        {
            CameraStarted = false;

            cameraPreviewForm.Hide();
            //cameraForm.Dispose();
            //cameraForm = null;
            camera.StopCamera();   
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
            ConnectToDroneEnabled = true;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonShowActualConfigOfArDrone(object sender, RoutedEventArgs e)
        {
            Log.Info("GetControlConfig");
            var controlConfig = ezbConnect.EZB.ARDrone.GetControlConfig();

            Log.Info(controlConfig);
            var arDroneConfigProvider = new ControlConfigBasedArDroneConfigProvider(controlConfig);
            ArDroneConfig = arDroneConfigProvider.GetArDroneConfig();
            windowManager.ShowWindow(new ControlConfigViewModel(controlConfig));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonSendDefaultValues(object sender, RoutedEventArgs e)
        {
            Log.Info("SendDefaultValues");
            ezbConnect.EZB.ARDrone.SendDefaultValues();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonShowHuePicker(object sender, RoutedEventArgs e)
        {
            huePickerForm = new HuePickerForm(this.eventAggregator, new HuePickerMessage(HueMin, HueMax));
            huePickerForm.Show();
            HuePickerIsVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonHideHuePicker(object sender, RoutedEventArgs e)
        {
            huePickerForm.Hide();
            HuePickerIsVisible = false;
        }

        public void ButtonSaveTrackingConfig(object sender, RoutedEventArgs e)
        {
            fileBasedTrackingConfigProvider.StoreTrackingConfig(new TrackingConfig()
            {
                Date = DateTime.Now,
                HueMax = HueMax,
                HueMin = HueMin,
                SearchObjectSizePixels = SearchObjectSizePixels,
                LuminanceMax = LuminanceMax,
                LuminanceMin = LuminanceMin,
                SaturationMax = SaturationMax,
                SaturationMin = SaturationMin
            });
        }


        public void ButtonStartAutonomousFlight(object sender, RoutedEventArgs e)
        {
            FlyingAtonomous = true;

            if (SearchObjectCenterLeft || SearchObjectBottomLeft || SearchObjectTopLeft)
            {
                // steer right
                CommandsForAutonomousFlight = CommandsForAutonomousFlight + "\nRoll Right";
                //RollRight();
            }

            if (SearchObjectCenterRight || SearchObjectTopRight || SearchObjectBottomRight)
            {
                // steer left
                CommandsForAutonomousFlight = CommandsForAutonomousFlight + "\nRoll Left";
                //RollLeft();
            }
        }

        public void ButtonStopAutonomousFlight(object sender, RoutedEventArgs e)
        {
            FlyingAtonomous = false;
        }
        #endregion

     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnArDroneStatusTimedEvent(object source, ElapsedEventArgs e)
        {
            BatteryLevel = ezbConnect.EZB.ARDrone.CurrentNavigationData.BatteryLevel;
        }

        /// <summary>
        /// 
        /// </summary>
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
            joystick = new Joystick(joystickDevice, ezbConnect.EZB) {EventWatcherResolution = 100};
            joystick.OnControllerAction += _joystick_OnControllerAction;
            joystick.StartEventWatcher();
        }

        /// <summary>
        /// Called on a controller action.
        /// Check every button if pressed and every stick if moved and execute the defined methods.
        /// </summary>
        private void _joystick_OnControllerAction()
        {

            // Button 8 -> Takeoff (deadman switch)
            if (joystick.ButtonPressed(7) && !Button8Pressed)
            {
                Log.Info("TakeOff");
                ezbConnect.EZB.ARDrone.TakeOff();
                Button8Pressed = true;
            }
            else if (joystick.ButtonPressed(7) == false)
            {
                Log.Info("Land");
                ezbConnect.EZB.ARDrone.Land();
                Button8Pressed = false;
            }

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
            //  Must be called before take-off (start engines). Must be called on a flat surface. This flattens the trim values for the surface. 
            if (joystick.ButtonPressed(1))
            {
                Button2Pressed = true;
                ezbConnect.EZB.ARDrone.SetFlatTrim();
            }
            else
            {
                Button2Pressed = false;
            }

            

            Button3Pressed = joystick.ButtonPressed(2);

            Button4Pressed = joystick.ButtonPressed(3);

            Button5Pressed = joystick.ButtonPressed(4);

            Button6Pressed = joystick.ButtonPressed(5);

            // Emergency
            if (joystick.ButtonPressed(6))
            {
                ezbConnect.EZB.ARDrone.Emergency();
                Log.Info("Emergency");
                Button7Pressed = true;
            }
            else
            {
                Button7Pressed = false;
            }

            // left stick, X axis -> yaw
            if (joystick.AxisXStateChanged())
            {
                LeftStickXAxis = joystick.GetAxisX;

                if (joystick.GetAxisX > 0.3)
                {
                    YawLeft();
                }

                if (joystick.GetAxisX < -0.3)
                {
                    YawRight();
                }
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

                // to the right
                if (joystick.GetAxisZ > 0.3)
                {
                    RollRight();
                }

                // to the left
                if (joystick.GetAxisZ < -0.3)
                {
                    RollLeft();
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

        private void RollLeft()
        {
            Log.Info("joystick.GetAxisZ {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisZ,
                MoveSensitivivivity, 0, 0, 0);
            ezbConnect.EZB.ARDrone.SetProgressiveInputValues(-MoveSensitivivivity, 0, 0, 0);
            Thread.Sleep(MoveSleepTimeMilliseconds);
            ezbConnect.EZB.ARDrone.Hover();
        }

        private void RollRight()
        {
            Log.Info("joystick.GetAxisZ {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisZ,
                -MoveSensitivivivity, 0, 0, 0);
            ezbConnect.EZB.ARDrone.SetProgressiveInputValues(MoveSensitivivivity, 0, 0, 0);
            Thread.Sleep(MoveSleepTimeMilliseconds);
            ezbConnect.EZB.ARDrone.Hover();
        }

        private void YawRight()
        {
            Log.Info("joystick.GetAxisY {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisY, 0, 0, 0,
                -MoveSensitivivivity);
            ezbConnect.EZB.ARDrone.SetProgressiveInputValues(0, 0, 0, -MoveSensitivivivity);
            Thread.Sleep(MoveSleepTimeMilliseconds);
            ezbConnect.EZB.ARDrone.Hover();
        }

        private void YawLeft()
        {
            Log.Info("joystick.GetAxisY {0} -> SetProgressiveInputValues '{1}', '{2}', '{3}', '{4}'", joystick.GetAxisY, 0, 0, 0,
                MoveSensitivivivity);
            ezbConnect.EZB.ARDrone.SetProgressiveInputValues(0, 0, 0, MoveSensitivivivity);
            Thread.Sleep(MoveSleepTimeMilliseconds);
            ezbConnect.EZB.ARDrone.Hover();
        }

        /// <summary>
        /// New camera frame available, so we look again for markers
        /// </summary>
        void _camera_OnNewFrame()
        {
            ObjectLocation objectLocation = null;

            if(ObjectDetectionEnabled)
            {
                try
                {
                    objectLocation = camera.CameraCustomColorDetection.GetObjectLocationByColor(
                                        TrackingPreviewEnabled,
                                        SearchObjectSizePixels, 
                                        HueMin, 
                                        HueMax,
                                        SaturationMin, 
                                        SaturationMax, 
                                        LuminanceMin, 
                                        LuminanceMax);
        
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }

            camera.UpdatePreview(255);


            if (objectLocation != null && objectLocation.IsObjectFound)
            {
                
                SearchObjectBottomLeft = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Bottom;
                SearchObjectBottomCenter = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Middle && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Bottom;
                SearchObjectBottomRight = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Bottom;
                
                SearchObjectCenterLeft = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Middle;
                SearchObjectCenterCenter = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Middle && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Middle;
                SearchObjectCenterRight = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Middle;
                
                SearchObjectTopLeft = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Left && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Top;
                SearchObjectTopCenter = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Middle && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Top;
                SearchObjectTopRight = objectLocation.HorizontalLocation == ObjectLocation.HorizontalLocationEnum.Right && objectLocation.VerticalLocation == ObjectLocation.VerticalLocationEnum.Top;
                

                TargetXCoordinate = objectLocation.CenterX;
                Log.Info("Object detected: X = {0}", objectLocation.CenterX);
                TargetYCoordinate = objectLocation.CenterY;
                Log.Info("Object detected: Y = {0}", objectLocation.CenterY);
            }
        }

        public void Handle(HuePickerMessage message)
        {
            HueMax = message.HueMax;
            HueMin = message.HueMin;
        }
    }
}