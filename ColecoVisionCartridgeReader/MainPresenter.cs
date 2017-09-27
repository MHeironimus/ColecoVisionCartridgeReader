using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Shell;

namespace ColecoVisionCartridgeReader
{
    class MainPresenter
    {
        #region Private Fields

        private const int CartridgeSize = 0x8000;
        private const int ChipSize = 0x2000;
        private const int CartridgeAddressStart = 0x8000;

        private readonly MainWindow _mainView;
        private ProgressDialog _progressView;

        private byte[] _cartridgeBuffer;
        private int _cartridgeSize;

        #endregion

        #region Constructor

        public MainPresenter()
        {
            _mainView = new MainWindow();

            _mainView.Exit += MainWindow_Exit;
            _mainView.FileOpen += MainView_FileOpen;
            _mainView.FileSaveAs += MainView_FileSaveAs;
            _mainView.ReadCartridge += MainView_ReadCartridge;
            _mainView.HelpAbout += MainView_HelpAbout;
        }

        #endregion

        #region Public Methods

        public void ShowMainView()
        {
            _mainView.Show();
        }

        #endregion

        #region MainWindow Event Handlers

        private void MainWindow_Exit(object sender, EventArgs e)
        {
            _mainView.Close();
        }

        private void MainView_FileOpen(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".rom",
                Filter = "ColecoVision Cartridge (*.rom)|*.rom|All Files (*.*)|*.*"
            };

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                ClearCartridgeData();

                // Open document 
                LoadFile(dlg.FileName);

                ShowCartridgeData();
            }
        }

        private void MainView_FileSaveAs(object sender, RoutedEventArgs e)
        {
            if (_cartridgeBuffer == null)
            {
                MessageBox.Show(_mainView, Properties.Resources.NoCartridgeLoadedMessage,
                    Properties.Resources.NoCartridgeLoadedTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Configure open file dialog box
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".rom",
                Filter = "ColecoVision Cartridge (*.rom)|*.rom|All Files (*.*)|*.*",
                OverwritePrompt = true
            };

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                SaveFile(dlg.FileName);
            }
        }

        private void MainView_ReadCartridge(object sender, RoutedEventArgs e)
        {
            var serialPorts = SerialPort.GetPortNames();

            if (serialPorts.Length < 1)
            {
                MessageBox.Show(_mainView, Properties.Resources.NoSerialPortsMessage,
                    Properties.Resources.NoSerialPortsTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var readCartridgeView = new ReadCartridgeDialog
                {
                    Owner = _mainView, 
                    SerialPorts = serialPorts,
                    SerialPort = serialPorts.Last(),
                    BaudRates = new[] { "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "28800", "38400", "57600", "115200" },
                    BaudRate = 57600
                };

                bool? result = readCartridgeView.ShowDialog();

                if (result == true)
                {
                    ClearCartridgeData();
                    ReadCartridge(new ArduinoSettings
                    {
                        SerialPort = readCartridgeView.SerialPort,
                        BaudRate = readCartridgeView.BaudRate
                    });
                }
            }
        }

        private void MainView_HelpAbout(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog
            {
                Owner = _mainView
            };

            aboutDialog.ShowDialog();
        }

        #endregion

        #region Private Properties

        private static string ApplicationTitle
        {
            get
            {
                Assembly currentAssembly = Assembly.GetExecutingAssembly();

                var title = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(
                    currentAssembly, typeof(AssemblyTitleAttribute));

                return title.Title;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clear Previous Cartridge Data
        /// </summary>
        private void ClearCartridgeData()
        {
            _cartridgeBuffer = null;
            _cartridgeSize = 0;
            _mainView.CartridgeData = string.Empty;
            _mainView.CartridgeLoaded = false;
            _mainView.Title = ApplicationTitle;
        }

        private void LoadFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            _mainView.Title = ApplicationTitle + " [" + fileName + "]";

            long fileSize = (new FileInfo(filePath)).Length;
            if (fileSize > 0x8000)
            {
                MessageBox.Show(_mainView, 
                    Properties.Resources.FileTooLargeMessage, 
                    Properties.Resources.FileTooLargeTitle, 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _cartridgeSize = (int)fileSize;

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using (var reader = new BinaryReader(fileStream))
                {
                    fileStream = null;
                    _cartridgeBuffer = reader.ReadBytes(_cartridgeSize);
                } // reader
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
                throw;
            }
        }

        private void ShowCartridgeData()
        {
            _mainView.CartridgeData = BuildCartridgeData();
            _mainView.CartridgeLoaded = true;
        }

        private string BuildCartridgeData()
        {
            var result = new StringBuilder();

            if (_cartridgeBuffer == null)
            {
                throw new InvalidOperationException(Properties.Resources.NoCartridgeFileLoaded);
            }

            for (int index = 0; index < _cartridgeSize; index += 16)
            {
                result.Append(BuildLine(CartridgeAddressStart + index, _cartridgeBuffer.Skip(index).Take(16).ToArray()));
            }

            return result.ToString();
        }

        private static string BuildLine(int address, byte[] data)
        {
            var result = new StringBuilder(80);
            var asciiVersion = new StringBuilder(16);

            result.AppendFormat("${0:X4} : ", address);

            foreach (byte t in data)
            {
                result.Append(t.ToString("X2"));
                result.Append(' ');

                if ((t >= 32) && (t <= 126))
                {
                    asciiVersion.Append(Convert.ToChar(t));
                }
                else
                {
                    asciiVersion.Append('∙');
                }
            }

            if (data.Length < 16)
            {
                for (int missingByte = 0; missingByte < (16 - data.Length); missingByte++)
                {
                    result.Append("   ");
                    asciiVersion.Append(" ");
                }
            }

            result.Append('|');
            result.Append(asciiVersion);
            result.Append('|');
            result.Append(Environment.NewLine);

            return result.ToString();
        }

        private void SaveFile(string filePath)
        {
            FileStream fileStream = null;

            string fileName = Path.GetFileName(filePath);
            _mainView.Title = ApplicationTitle + " [" + fileName + "]";
            
            try
            {
                fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                using (var writer = new BinaryWriter(fileStream))
                {
                    fileStream = null;
                    writer.Write(_cartridgeBuffer, 0, _cartridgeSize);
                }
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
                throw;
            }
        }

        private void ReadCartridge(ArduinoSettings arduinoSettings)
        {
            var cartridgeReaderBackground = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            cartridgeReaderBackground.DoWork += CartridgeReaderBackground_DoWork;
            cartridgeReaderBackground.ProgressChanged += CartridgeReaderBackground_ProgressChanged;
            cartridgeReaderBackground.RunWorkerCompleted += CartridgeReaderBackground_RunWorkerCompleted;

            _progressView = new ProgressDialog
            {
                Owner = _mainView
            };
            _progressView.UpdateProgress(0, Properties.Resources.ConnectingMessage);

            _mainView.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            cartridgeReaderBackground.RunWorkerAsync(arduinoSettings);

            var progressDialogResult = _progressView.ShowDialog();

            if (progressDialogResult == false)
            {
                cartridgeReaderBackground.CancelAsync();
            }
        }

        private void CreateEmptyCartridge(int cartridgeSize)
        {
            _cartridgeSize = cartridgeSize;
            _cartridgeBuffer = new byte[cartridgeSize];
        }

        private static byte ParseByte(string currentLine)
        {
            byte currentByte;

            if (!byte.TryParse(currentLine, System.Globalization.NumberStyles.AllowHexSpecifier, null, out currentByte))
            {
                throw new InvalidOperationException(
                    string.Format(Properties.Resources.ArduinoUnexpectedValueMessage,
                        "a hexadecimal value", currentLine));
            }

            return currentByte;
        }

        /// <summary>
        /// Removes any blank 8k segments from the end of the cartridge.
        /// </summary>
        private void TruncateCartridge()
        {
            for (int currentChip = 3; currentChip >= 0; currentChip--)
            {
                if (IsChipEmpty(currentChip))
                {
                    _cartridgeSize -= ChipSize;
                }
                else
                {
                    break;
                }
            }

            if (_cartridgeSize <= 0)
            {
                throw new InvalidOperationException(Properties.Resources.BlankCartridge);
            }
        }

        /// <summary>
        /// Is the indicated 8k cartridge blank?
        /// </summary>
        /// <param name="chipIndex">
        /// 0 - 3 : Index of the 8k cartridge (0 = 0x8000 chip, 1 = 0xA000 chip, etc.)
        /// </param>
        /// <returns>
        /// true if indicated 8k cartridge is blank, false if it is not blank.
        /// </returns>
        private bool IsChipEmpty(int chipIndex)
        {
            for (int currentByte = 0; currentByte < ChipSize; currentByte++)
            {
                int currentChipAddress = (ChipSize*chipIndex) + currentByte;
                if (_cartridgeBuffer[currentChipAddress] != 0xFF)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region CartridgeReaderBackground Events

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization",
            "CA1303:Do not pass literals as localized parameters",
            MessageId = "System.IO.Ports.SerialPort.WriteLine(System.String)",
            Justification = "This is a command sent to the Arduiono, not a string read by a user.")]
        private void CartridgeReaderBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var arduinoSettings = e.Argument as ArduinoSettings;

            if (worker == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (arduinoSettings == null)
            {
                throw new InvalidOperationException("Arduino settings were not specified.");
            }

            const string cReadCommand = "READ ALL";
            const string cStartMessage = "START:";
            const string cEndMessage = ":END";
            const int cUpdateProgressEvery = 0x0250;

            using (var serialPort = new SerialPort(arduinoSettings.SerialPort, arduinoSettings.BaudRate))
            {
                // Set the read/write timeouts
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;

                serialPort.Open();
                serialPort.DiscardInBuffer();

                // Tell the Arduino to read all of the cartridge.
                serialPort.WriteLine(cReadCommand);

                // Verify the Arduino returns the START: message.
                var readLine = serialPort.ReadLine().Trim();
                if (!cStartMessage.Equals(readLine, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new InvalidOperationException(
                        string.Format(Properties.Resources.ArduinoUnexpectedValueMessage,
                        cStartMessage, readLine));
                }

                CreateEmptyCartridge(CartridgeSize);

                int currentAddress = 0;
                string currentLine = serialPort.ReadLine().Trim();
                while (!cEndMessage.Equals(currentLine, StringComparison.InvariantCultureIgnoreCase)
                    && (currentAddress < CartridgeSize))
                {
                    _cartridgeBuffer[currentAddress++] = ParseByte(currentLine);

                    if ((currentAddress % cUpdateProgressEvery) == 0)
                    {
                        // Check for cancel
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            e.Result = false;
                            return;
                        }

                        // Update Progress Window
                        worker.ReportProgress((int)((currentAddress / (float)CartridgeSize) * 90));
                    }

                    currentLine = serialPort.ReadLine().Trim();

                } // while there is still data

                if (!cEndMessage.Equals(currentLine, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new InvalidOperationException(
                        string.Format(Properties.Resources.ArduinoUnexpectedValueMessage,
                        cEndMessage, currentLine));
                }

                if (currentAddress != CartridgeSize)
                {
                    throw new InvalidOperationException(
                        string.Format(Properties.Resources.UnexpectedCartridgeSize,
                        currentAddress, CartridgeSize));
                }

                // Update Progress Window
                worker.ReportProgress(95);

                TruncateCartridge();

                // Update Progress Window
                worker.ReportProgress(100);

            } // using serialPort

            e.Result = true;
        }

        private void CartridgeReaderBackground_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_progressView != null)
            {
                _progressView.UpdateProgress(e.ProgressPercentage,
                    string.Format(Properties.Resources.ProgressMessage, e.ProgressPercentage));
            }
            _mainView.TaskbarItemInfo.ProgressValue = e.ProgressPercentage / 100d;
        }

        private void CartridgeReaderBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _mainView.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
                MessageBox.Show(_mainView, e.Error.Message, Properties.Resources.CartridgeReadErrorTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (e.Cancelled == false)
            {
                ShowCartridgeData();
            }
            _mainView.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;

            // Close Progress Window, if it is still open.
            if ((_progressView != null) && _progressView.IsVisible)
            {
                _progressView.DialogResult = true;
                _progressView.Close();
            }
        }

        #endregion
    }
}
