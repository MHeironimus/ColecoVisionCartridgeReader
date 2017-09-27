namespace ColecoVisionCartridgeReader
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        #region Public Methods

        public void UpdateProgress(double value, string message)
        {
            MessageLabel.Content = message;
            ProgressBar.Value = value;
        }

        #endregion
    }
}
