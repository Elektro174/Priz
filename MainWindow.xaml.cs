using System;
using System.IO.Ports;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace govno
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string data;

        SerialPort sp = new SerialPort();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            string returnData = "";
            SerialPort sp = (SerialPort)sender;
            int count = sp.BytesToRead;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    char bt = (char)sp.ReadByte();
                    returnData = returnData + bt.ToString();
                }

                Console.WriteLine(returnData.ToString());
                data = returnData.ToString();

                Dispatcher.Invoke(delegate
                {
                    temperature.Text = "Температура" + data;
                });

                returnData = "";
                sp.DiscardInBuffer();
            }

        }

        private void Button_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            ComboBoxPorts.Text = "";
            ComboBoxPorts.Items.Clear();
            if (ports.Length != 0)
            {
                foreach (var item in ports)
                {
                    ComboBoxPorts.Items.Add(item);
                    ComboBoxPorts.SelectedIndex = 0;
                }
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {

            if ((string)ButtonConnect.Content == "Connect")
            {
                try
                {

                    sp.PortName = ComboBoxPorts.Text;
                    sp.Open();
                    ButtonConnect.Content = "Disconnect";
                    ComboBoxPorts.IsEnabled = false;

                }
                catch (Exception)
                {
                    MessageBox.Show("Connection error");
                }
            }
            else if ((string)ButtonConnect.Content == "Disconnect")
            {
                sp.Close();
                ComboBoxPorts.IsEnabled = true;
                ButtonConnect.Content = "Connect";
            }
        }

        private void Temperature_Click(object sender, RoutedEventArgs e)
        {
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            
        }
    }
}
