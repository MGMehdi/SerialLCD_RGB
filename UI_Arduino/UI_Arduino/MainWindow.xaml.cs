using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI_Arduino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort arduino = new SerialPort();
        private int redValue = 0, greenValue = 0, blueValue = 0;
        private Ellipse ellipse = new Ellipse();
        SolidColorBrush solidColor = new SolidColorBrush();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Load(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (String p in ports)
            {
                cmbComPort.Items.Add(p);
            }

            txbRed.Text = redValue.ToString();
            txbGreen.Text = greenValue.ToString();
            txbBlue.Text = blueValue.ToString();
            setColor();

        }

        private void Send(object sender, RoutedEventArgs e)
        {
            String text;
            text = '#' + txbText.Text;
            arduino.Write(text);

            
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            if (cmbComPort.SelectedIndex != -1)
            {
                this.arduino.PortName = cmbComPort.Text;
                this.arduino.BaudRate = 9600;
                try
                {
                    this.arduino.Open();
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Port maybe already in use", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No port selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (arduino.IsOpen == true)
            {
                arduino.Write("#Welcome !");
                btnDisconnect.IsEnabled = true;
                btnConnect.IsEnabled = false;
                btnSend.IsEnabled = true;
                txbText.IsEnabled = true;
                cmbComPort.IsEnabled = false;
                gridColor.IsEnabled = true;
            }
        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {
            arduino.Write("+");
            btnConnect.IsEnabled = true;
            btnDisconnect.IsEnabled = false;
            btnSend.IsEnabled = false;
            txbText.IsEnabled = false;
            cmbComPort.IsEnabled = true;
            gridColor.IsEnabled = false;
            txbText.Clear();
            sldRed.Value = 0;
            sldGreen.Value = 0;
            sldBlue.Value = 0;
            this.arduino.Close();
        }


        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (arduino.IsOpen == true)
            {
                arduino.Write("+");
            }
            arduino.Close();
        }

        private void RedValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.redValue = (int)sldRed.Value;
            txbRed.Text = this.redValue.ToString();
            this.arduino.Write("*R" + this.redValue);
            setColor();
        }

        private void GreenValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.greenValue = (int)sldGreen.Value;
            txbGreen.Text = this.greenValue.ToString();
            this.arduino.Write("*G" + this.greenValue);
            setColor();
        }

        private void BlueValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.blueValue = (int)sldBlue.Value;
            txbBlue.Text = this.blueValue.ToString();
            this.arduino.Write("*B" + this.blueValue);
            setColor();
        }

        private void setColor()
        {
            solidColor.Color = Color.FromRgb((byte)this.redValue, (byte)this.greenValue, (byte)this.blueValue);
            ellipseColor.Fill = solidColor;
        }
    }
}
