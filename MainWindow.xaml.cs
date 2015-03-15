using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EZ_B;
using EZ_B.CameraDetection;

namespace FollowMe
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float moveSensitivivivity = 0.20f;
        int moveSleepTime = 400;
        EZ_B.Joystick.Joystick _joystick = null;


        Camera camera;
        private System.Timers.Timer batteryTimer;
        private readonly UCEZB_Connect ezB_Connect1 = new UCEZB_Connect();
        public MainWindow()
        {
            InitializeComponent();
            ezB_Connect1.EZB.ShowDebugWindow();
            camera = new Camera(ezB_Connect1.EZB);
            camera.OnNewFrame += _camera_OnNewFrame;

            

            
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
           Dispatcher.Invoke(() => TextBoxBattery.Text = ezB_Connect1.EZB.ARDrone.CurrentNavigationData.BatteryLevel.ToString());

            
        }
        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            batteryTimer = new System.Timers.Timer();
            batteryTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            batteryTimer.Interval = 5000;
            batteryTimer.Enabled = true;

            try
            {
                ezB_Connect1.EZB.ARDrone.Connect(EZ_B.ARDrone.ARDrone.ARDroneVersionEnum.V2);

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
            ezB_Connect1.EZB.ARDrone.PlayLedAnimation(EZ_B.ARDrone.Commands.LedAnimationEnum.BlinkRed, 2, 10);
        }

        private void ButtonRefreshJoysticks_Click(object sender, RoutedEventArgs e)
        {
        
            if (_joystick != null) 
            {
                _joystick.Dispose();
                _joystick = null;
         //       Invokers.SetAppendText(textBox1, true, "Disconnected");
            }

            ComboBoxJoysticks.Items.Clear();
            var availableDevices = EZ_B.Joystick.Joystick.GetAvailableDevices();
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
            EZ_B.Joystick.JoystickDevice jd = (EZ_B.Joystick.JoystickDevice)ComboBoxJoysticks.SelectedItem;
            _joystick = new EZ_B.Joystick.Joystick(jd, ezB_Connect1.EZB);
            _joystick.OnControllerAction += new EZ_B.Joystick.Joystick.OnJoystickMoveHandler(_joystick_OnControllerAction);
            _joystick.StartEventWatcher();
        }

        void _joystick_OnControllerAction()
        {
            if (_joystick.ButtonPressed(0))
            {
                Dispatcher.Invoke(() => RadioButton1.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton1.IsChecked = false);
            }

            if (_joystick.ButtonPressed(2))
            {
                Dispatcher.Invoke(() => RadioButton2.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton2.IsChecked = false);
            }

            if (_joystick.ButtonPressed(3))
            {
                Dispatcher.Invoke(() => RadioButton3.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton3.IsChecked = false);
            }

            if (_joystick.ButtonPressed(4))
            {
                Dispatcher.Invoke(() => RadioButton4.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton4.IsChecked = false);
            }

            if (_joystick.ButtonPressed(5))
            {
                Dispatcher.Invoke(() => RadioButton5.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton5.IsChecked = false);
            }

            if (_joystick.ButtonPressed(6))
            {
                Dispatcher.Invoke(() => RadioButton6.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton6.IsChecked = false);
            }

            if (_joystick.ButtonPressed(7))
            {
                Dispatcher.Invoke(() => RadioButton7.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton7.IsChecked = false);
            }

            if (_joystick.ButtonPressed(8))
            {
                Dispatcher.Invoke(() => RadioButton8.IsChecked = true);
            }
            else
            {
                Dispatcher.Invoke(() => RadioButton8.IsChecked = false);
            }


            if (_joystick.AxisXStateChanged())
            {
                Dispatcher.Invoke(() => TextBoxJoystick.Text += _joystick.GetAxisX.ToString());
            }

            if (_joystick.AxisYStateChanged())
            {
                Dispatcher.Invoke(() => TextBoxJoystick.Text += _joystick.GetAxisY.ToString());
            }
        }
    }
}
