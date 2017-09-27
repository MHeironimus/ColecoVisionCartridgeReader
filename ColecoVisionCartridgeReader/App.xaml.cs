using System.Windows;
using System.Windows.Threading;

namespace ColecoVisionCartridgeReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var presenter = new MainPresenter();

            presenter.ShowMainView();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, 
                ColecoVisionCartridgeReader.Properties.Resources.UnhandledExceptionTitle, 
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
