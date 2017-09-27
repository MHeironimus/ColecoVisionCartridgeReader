using System.Collections;
using System.Globalization;

namespace ColecoVisionCartridgeReader
{
    /// <summary>
    /// Interaction logic for ReadCartridgeDialog.xaml
    /// </summary>
    public partial class ReadCartridgeDialog
    {
        public ReadCartridgeDialog()
        {
            InitializeComponent();
        }

        #region Public Properties

        public IEnumerable SerialPorts
        {
            get
            {
                return ArduinoPort.ItemsSource;
            }
            set
            {
                ArduinoPort.ItemsSource = value;
            }
        }

        public IEnumerable BaudRates
        {
            get
            {
                return BaudRateComboBox.ItemsSource;
            }
            set
            {
                BaudRateComboBox.ItemsSource = value;
            }
        }

        public string SerialPort
        {
            get
            {
                return ArduinoPort.Text;
            }
            set
            {
                ArduinoPort.Text = value;
            }
        }

        public int BaudRate
        {
            get
            {
                int result;

                if (int.TryParse(BaudRateComboBox.Text, out result) == false)
                {
                    result = 57600;
                }

                return result;
            }
            set
            {
                BaudRateComboBox.Text = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Event Handlers

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ArduinoPort.Focus();
        }

        #endregion
    }
}
