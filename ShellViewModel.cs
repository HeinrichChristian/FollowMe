using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using Caliburn.Micro;
using EZ_B;
using EZ_B.ARDrone;
using EZ_B.CameraDetection;
using EZ_B.Joystick;
using Timer = System.Timers.Timer;

namespace FollowMe {
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        #region privates
        private float moveSensitivivivity = 0.20f;
        private int moveSleepTime = 400;
        private Joystick joystick = null;
        private Camera camera;
        private Timer batteryTimer;
        private readonly UCEZB_Connect ezB_Connect1 = new UCEZB_Connect();
        private int _batteryLevel;
        private bool _button1Pressed;
        private bool _button2Pressed;
        private bool _button3Pressed;
        private bool _button4Pressed;
        private bool _button5Pressed;
        private bool _button6Pressed;
        private bool _button7Pressed;
        private bool _button8Pressed;
        private float _leftStickXAxis;
        private float _leftStickYAxis;
        private float _rightStickXAxis;
        private float _rightStickYAxis;
        private float _targetXCoordinate;
        private float _targetYCoordinate;
        private List<JoystickDevice> _availableJoystickDevices;
        private string _qrCode;
        private System.Windows.Forms.Control _cameraPanel = new System.Windows.Forms.Control();
        private JoystickDevice _selectedJoystickDevice;
        private string _droneStatus;
        private CameraForm cameraForm;
        #endregion

 

        #region public properties

        /// <summary>
        /// The battery level of the AR.Drone - value between 0 and 100, lower than 17 is not good
        /// </summary>
        public int BatteryLevel
        {
            get { return _batteryLevel; }
            set
            {
                _batteryLevel = value;
                NotifyOfPropertyChange(() => BatteryLevel);
            }
        }

        /// <summary>
        /// true, if the button 1 of the joystick is pressed
        /// </summary>
        public bool Button1Pressed
        {
            get { return _button1Pressed; }
            set
            {
                _button1Pressed = value;
                NotifyOfPropertyChange(() => Button1Pressed);
            }
        }

        /// <summary>
        /// true, if the button 2 of the joystick is pressed
        /// </summary>
        public bool Button2Pressed
        {
            get { return _button2Pressed; }
            set
            {
                _button2Pressed = value;
                NotifyOfPropertyChange(() => Button2Pressed);
            }
        }

        /// <summary>
        /// true, if the button 3 of the joystick is pressed
        /// </summary>
        public bool Button3Pressed
        {
            get { return _button3Pressed; }
            set
            {
                _button3Pressed = value;
                NotifyOfPropertyChange(() => Button3Pressed);
            }
        }

        /// <summary>
        /// true, if the button 2 of the joystick is pressed
        /// </summary>
        public bool Button4Pressed
        {
            get { return _button4Pressed; }
            set
            {
                _button4Pressed = value;
                NotifyOfPropertyChange(() => Button4Pressed);
            }
        }

        /// <summary>
        /// true, if the button 5 of the joystick is pressed
        /// </summary>
        public bool Button5Pressed
        {
            get { return _button5Pressed; }
            set
            {
                _button5Pressed = value;
                NotifyOfPropertyChange(() => Button5Pressed);
            }
        }

        /// <summary>
        /// true, if the button 6 of the joystick is pressed
        /// </summary>
        public bool Button6Pressed
        {
            get { return _button6Pressed; }
            set
            {
                _button6Pressed = value;
                NotifyOfPropertyChange(() => Button6Pressed);
            }
        }

        /// <summary>
        /// true, if the button 2 of the joystick is pressed
        /// </summary>
        public bool Button7Pressed
        {
            get { return _button7Pressed; }
            set
            {
                _button7Pressed = value;
                NotifyOfPropertyChange(() => Button7Pressed);
            }
        }

        /// <summary>
        /// true, if the button 2 of the joystick is pressed
        /// </summary>
        public bool Button8Pressed
        {
            get { return _button8Pressed; }
            set
            {
                _button8Pressed = value;
                NotifyOfPropertyChange(() => Button8Pressed);
            }
        }

        public float LeftStickXAxis
        {
            get { return _leftStickXAxis; }
            set
            {
                _leftStickXAxis = value; 
                NotifyOfPropertyChange(() => LeftStickXAxis);
            }
        }

        public float LeftStickYAxis
        {
            get { return _leftStickYAxis; }
            set
            {
                _leftStickYAxis = value; 
                NotifyOfPropertyChange(() => LeftStickYAxis);
            }
        }

        public float RightStickXAxis
        {
            get { return _rightStickXAxis; }
            set
            {
                _rightStickXAxis = value;
                NotifyOfPropertyChange(() => RightStickXAxis);
            }
        }

        public float RightStickYAxis
        {
            get { return _rightStickYAxis; }
            set
            {
                _rightStickYAxis = value; 
                NotifyOfPropertyChange(() => RightStickYAxis);
            }
        }

        public float TargetXCoordinate
        {
            get { return _targetXCoordinate; }
            set
            {
                _targetXCoordinate = value; 
                NotifyOfPropertyChange(() => TargetXCoordinate);
            }
        }

        public float TargetYCoordinate
        {
            get { return _targetYCoordinate; }
            set
            {
                _targetYCoordinate = value; 
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
                if (_availableJoystickDevices == null)
                {
                    _availableJoystickDevices = new List<JoystickDevice>();
                    RefreshJoysticks();
                }
                return _availableJoystickDevices;
            }
            set
            {
                _availableJoystickDevices = value; 
                NotifyOfPropertyChange(() => AvailableJoystickDevices);
            }
        }

        /// <summary>
        /// The selected joystick
        /// </summary>
        public JoystickDevice SelectedJoystickDevice
        {
            get { return _selectedJoystickDevice; }
            set
            {
                _selectedJoystickDevice = value;
                if (value != null)
                {
                    ActivateJoystick();
                }
                NotifyOfPropertyChange(() => SelectedJoystickDevice);
            }
        }

        /// <summary>
        /// The text of therecognized QR-Code
        /// </summary>
        public string QrCode
        {
            get { return _qrCode; }
            set
            {
                _qrCode = value; 
                NotifyOfPropertyChange(() => QrCode);
            }
        }

        /// <summary>
        /// The control to show the camera view
        /// </summary>
        public System.Windows.Forms.Control CameraPanel
        {
            get { return _cameraPanel; }
            set
            {
                _cameraPanel = value;
                NotifyOfPropertyChange(() => CameraPanel);
            }
        }

        public string DroneStatus
        {
            get { return _droneStatus; }
            set
            {
                _droneStatus = value;
                NotifyOfPropertyChange(() => DroneStatus);
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ShellViewModel()
        {
            ezB_Connect1.EZB.ShowDebugWindow();
            camera = new Camera(ezB_Connect1.EZB);
            camera.OnNewFrame += _camera_OnNewFrame;

            RefreshJoysticks();

            cameraForm = new CameraForm();
            cameraForm.Show();
        }

        public void ButtonConnect(object sender, RoutedEventArgs e)
        {
            batteryTimer = new Timer();
            batteryTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            batteryTimer.Interval = 5000;
            batteryTimer.Enabled = true;

            try
            {
                ezB_Connect1.EZB.ARDrone.Connect(ARDrone.ARDroneVersionEnum.V2);

                if (ezB_Connect1.EZB.ARDrone.IsConnected)
                {
                    DroneStatus = "Verbunden";
                }
                else
                {
                    DroneStatus = "Nicht Verbunden";
                }
                //Debug.WriteLine(ezB_Connect1.EZB.ARDrone.GetControlConfig());
            }
            catch (Exception exception)
            {
                DroneStatus = "Fehler " + exception;
                //MessageBox.Show("Fehler: " + exception.ToString());
            }
        }

        public void ButtonDisconnect()
        {
            ezB_Connect1.EZB.ARDrone.Disconnect();
        }


        public void ButtonShowCamera(object sender, RoutedEventArgs e)
        {
            camera.StartCamera(
                  new ValuePair(Camera.CAMERA_NAME_AR_DRONE, Camera.CAMERA_NAME_AR_DRONE),
                    cameraForm.ExternalCameraPanel,
                  320,
                  240);

            ezB_Connect1.EZB.ARDrone.StartVideo();
        }

        public void ButtonDisconnect(object sender, RoutedEventArgs e)
        {
            ezB_Connect1.EZB.ARDrone.Disconnect();
        }

        public void ButtonBlinkLeds(object sender, RoutedEventArgs e)
        {
            ezB_Connect1.EZB.ARDrone.PlayLedAnimation(Commands.LedAnimationEnum.BlinkRed, 2, 10);
        }

        public void ButtonRefreshJoysticks(object sender, RoutedEventArgs e)
        {
            RefreshJoysticks();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("Battery: " + ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel);
            
            BatteryLevel = ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel;
                    //if (ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel > 17)
                    //{
                    //    TextBoxBattery.Background = Brushes.GreenYellow;
                    //}
                    //else
                    //{
                    //    TextBoxBattery.Background = Brushes.OrangeRed;
                    //}
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

        private void ActivateJoystick()
        {
            JoystickDevice jd = SelectedJoystickDevice;
            joystick = new Joystick(jd, ezB_Connect1.EZB);
            joystick.OnControllerAction += new Joystick.OnJoystickMoveHandler(_joystick_OnControllerAction);
            joystick.StartEventWatcher();
        }

        void _joystick_OnControllerAction()
        {
            // Button 1 -> blink LEDs
            if (joystick.ButtonPressed(0))
            {
                Button1Pressed = true;
                if (ezB_Connect1.EZB.ARDrone.IsConnected)
                    ezB_Connect1.EZB.ARDrone.PlayLedAnimation(Commands.LedAnimationEnum.BlinkRed, 2, 10);
            }
            else
            {
                Button1Pressed = false;
            }

            if (joystick.ButtonPressed(1))
            {
                Button2Pressed = true;
            }
            else
            {
                Button2Pressed = false;
            }

            if (joystick.ButtonPressed(2))
            {
                Button3Pressed = true;
            }
            else
            {
                Button3Pressed = false;
            }

            if (joystick.ButtonPressed(3))
            {
                Button4Pressed = true;
            }
            else
            {
                Button4Pressed = false;
            }

            if (joystick.ButtonPressed(4))
            {
                Button5Pressed = true;
            }
            else
            {
                Button5Pressed = false;
            }

            if (joystick.ButtonPressed(5))
            {
                Button6Pressed = true;
            }
            else
            {
                Button6Pressed = false;
            }

            if (joystick.ButtonPressed(6))
            {
                Button7Pressed = true;
            }
            else
            {
                Button7Pressed = false;
            }

            // Button 8 -> Takeoff (deadman switch)
            if (joystick.ButtonPressed(7))
            {
                ezB_Connect1.EZB.ARDrone.TakeOff();
                Button8Pressed = true;
            }
            else
            {
                ezB_Connect1.EZB.ARDrone.Land();
                Button8Pressed = false;
            }

            // left stick
            if (joystick.AxisXStateChanged())
            {
                LeftStickXAxis = joystick.GetAxisX;
            }

            if (joystick.AxisYStateChanged())
            {
                LeftStickYAxis = joystick.GetAxisY;
            }

            // right stick
            if (joystick.AxisZStateChanged())
            {
                RightStickXAxis = joystick.GetAxisZ;
            }
            if (joystick.AxisRzStateChanged())
            {
                RightStickYAxis = joystick.GetAxisRz;
            }
        }
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
            catch (Exception e)
            {

            }

        }
    }
}