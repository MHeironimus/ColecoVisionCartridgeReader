using System.Windows;

namespace ColecoVisionCartridgeReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Public Events

        public event RoutedEventHandler Exit;
        public event RoutedEventHandler FileOpen;
        public event RoutedEventHandler FileSaveAs;
        public event RoutedEventHandler ReadCartridge;
        public event RoutedEventHandler HelpAbout;

        #endregion

        #region Public Properties

        public bool CartridgeLoaded
        {
            get
            {
                return _cartridgeLoaded;
            }
            set
            {
                _cartridgeLoaded = value;
                FileSaveAsMenuItem.IsEnabled = _cartridgeLoaded;
            }
        }

        public string CartridgeData
        {
            get
            {
                return CartridgeDataTextBox.Text;
            }
            set
            {
                CartridgeDataTextBox.Text = value;
                CartridgeDataTextBox.Select(0, 0);
            }
        }

        #endregion

        #region Private Fields

        bool _cartridgeLoaded;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            CartridgeLoaded = false;
        }

        #region Event Handlers

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            if (Exit != null)
            {
                Exit(sender, e);
            }
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            if (FileOpen != null)
            {
                FileOpen(sender, e);
            }
        }

        private void FileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (FileSaveAs != null)
            {
                FileSaveAs(sender, e);
            }
        }

        private void FileReadCartridge_Click(object sender, RoutedEventArgs e)
        {
            if (ReadCartridge != null)
            {
                ReadCartridge(sender, e);
            }
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            if (HelpAbout != null)
            {
                HelpAbout(sender, e);
            }
        }

        #endregion

    }
}
