using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EZ_B;
using EZ_B.ARDrone;
using EZ_B.CameraDetection;
using EZ_B.Joystick;

namespace FollowMe
{
    /// <summary>
    /// Interaktionslogik für DroneControlView.xaml
    /// </summary>
    public partial class DroneControlView : Window
    {
        float moveSensitivivivity = 0.20f;
        int moveSleepTime = 400;
        Joystick joystick = null;


        Camera camera;
        private Timer batteryTimer;
        private readonly UCEZB_Connect ezB_Connect1 = new UCEZB_Connect();

        public DroneControlView()
        {
            InitializeComponent();
            ezB_Connect1.EZB.ShowDebugWindow();
            camera = new Camera(ezB_Connect1.EZB);
            camera.OnNewFrame += _camera_OnNewFrame;

            RefreshJoysticks();

            
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            camera.StopCamera();

            base.OnClosing(e);
           
        }

        void _camera_OnNewFrame()
        {
            camera.UpdatePreview();
            try
            {
                ObjectLocation objectLocationByColor = camera.CameraBasicColorDetection.GetObjectLocationByColor(true, ColorDetection.ColorEnum.Red, 50, 80);

                if (objectLocationByColor != null)
                {
                    Debug.WriteLine( objectLocationByColor.VerticalLocation.ToString());
                    Dispatcher.Invoke(() => TextBoxXCoordinate.Text = objectLocationByColor.VerticalLocation.ToString());
                        //.CenterX.ToString();
                    Dispatcher.Invoke(() => TextBoxYCoordinate.Text = objectLocationByColor.HorizontalLocation.ToString());
                        // CenterY.ToString();
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
                Dispatcher.Invoke(() =>     TextBoxQrCode.Text = objectLocationByQrCode.QRCodeText);
                Debug.WriteLine(objectLocationByQrCode.QRCodeText);
                }
                else
                {
                    Dispatcher.Invoke(() => TextBoxQrCode.Text = string.Empty);
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

        private  void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("Battery: " + ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel);
           Dispatcher.Invoke(
               () =>
               {
                   TextBoxBattery.Text = ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel.ToString();
                   if (ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel > 17)
                   {
                       TextBoxBattery.Background =  Brushes.GreenYellow;
                   }
                   else
                   {
                       TextBoxBattery.Background = Brushes.OrangeRed;
                   }
               });
            
        }
        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
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
                    TextConnectionStatus.Text = "Verbunden";
                }
                else
                {
                    TextConnectionStatus.Text = "Nicht Verbunden";
                }
                Debug.WriteLine(ezB_Connect1.EZB.ARDrone.GetControlConfig());
            }
            catch(Exception exception)
            {
                TextConnectionStatus.Text = "Fehler";
                MessageBox.Show("Fehler: " + exception.ToString());
            }
        }

        private void ButtonShowCamera_Click(object sender, RoutedEventArgs e)
        {
            camera.StartCamera(
                  new ValuePair(Camera.CAMERA_NAME_AR_DRONE, Camera.CAMERA_NAME_AR_DRONE),
                  CameraPanel,
                  320,
                  240);

            ezB_Connect1.EZB.ARDrone.StartVideo();
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            ezB_Connect1.EZB.ARDrone.Disconnect();
        }

        private void ButtonBlinkLeds_Click(object sender, RoutedEventArgs e)
        {
            ezB_Connect1.EZB.ARDrone.PlayLedAnimation(Commands.LedAnimationEnum.BlinkRed, 2, 10);
        }

        private void ButtonRefreshJoysticks_Click(object sender, RoutedEventArgs e)
        {
            RefreshJoysticks();
        }

        private void RefreshJoysticks()
        {
            if (joystick != null)
            {
                joystick.Dispose();
                joystick = null;
            }

            ComboBoxJoysticks.Items.Clear();
            var availableDevices = Joystick.GetAvailableDevices();
            if (availableDevices != null)
            {
                foreach (var availableDevice in availableDevices)
                {
                    ComboBoxJoysticks.Items.Add(availableDevice);
                }
            }
        }

        private void ComboBoxJoysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JoystickDevice jd = (JoystickDevice)ComboBoxJoysticks.SelectedItem;
            joystick = new Joystick(jd, ezB_Connect1.EZB);
            joystick.OnControllerAction += new Joystick.OnJoystickMoveHandler(_joystick_OnControllerAction);
            joystick.StartEventWatcher();
        }

        void _joystick_OnControllerAction()
        {
            // Button 1 -> blink LEDs
            if (joystick.ButtonPressed(0))
            {
                Dispatcher.Invoke(() => RadioButton1.IsChecked = true);
                if(ezB_Connect1.EZB.ARDrone.IsConnected)
                    ezB_Connect1.EZB.ARDrone.PlayLedAnimation(Commands.LedAnimationEnum.BlinkRed, 2, 10);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton1.IsChecked = false);
            }

            if (joystick.ButtonPressed(1))
            {
                Dispatcher.Invoke(() => RadioButton2.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton2.IsChecked = false);
            }

            if (joystick.ButtonPressed(2))
            {
                Dispatcher.Invoke(() => RadioButton3.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton3.IsChecked = false);
            }

            if (joystick.ButtonPressed(3))
            {
                Dispatcher.Invoke(() => RadioButton4.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton4.IsChecked = false);
            }

            if (joystick.ButtonPressed(4))
            {
                Dispatcher.Invoke(() => RadioButton5.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton5.IsChecked = false);
            }

            if (joystick.ButtonPressed(5))
            {
                Dispatcher.Invoke(() => RadioButton6.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton6.IsChecked = false);
            }

            if (joystick.ButtonPressed(6))
            {
                Dispatcher.Invoke(() => RadioButton7.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton7.IsChecked = false);
            }

            // Button 8 -> Takeoff (deadman switch)
            if (joystick.ButtonPressed(7))
            {
                ezB_Connect1.EZB.ARDrone.TakeOff();
                Dispatcher.Invoke(() => RadioButton8.IsChecked = true);
            }
            else
            {
                ezB_Connect1.EZB.ARDrone.Land();
                Dispatcher.Invoke(() => RadioButton8.IsChecked = false);
            }

            // left stick
            if (joystick.AxisXStateChanged())
            {
                Dispatcher.Invoke(() => TextBoxLeftXAxes.Text = joystick.GetAxisX.ToString(CultureInfo.InvariantCulture));
            }

            if (joystick.AxisYStateChanged())
            {
                Dispatcher.Invoke(() => TextBoxLeftYAxes.Text = joystick.GetAxisY.ToString(CultureInfo.InvariantCulture));
            }

            // right stick
            if (joystick.AxisZStateChanged())
            {
                Dispatcher.Invoke(() => TextBoxRightXAxes.Text = joystick.GetAxisZ.ToString(CultureInfo.InvariantCulture));
            }
            if (joystick.AxisRzStateChanged())
            {
                Dispatcher.Invoke(() => TextBoxRightYAxes.Text = joystick.GetAxisRz.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
