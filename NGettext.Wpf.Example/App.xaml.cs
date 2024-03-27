using System.Windows;

namespace NGettext.Wpf.Example
{
    /// <inheritdoc/>
    public partial class App : Application
    {
        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            CompositionRoot.Compose("Example");
            base.OnStartup(e);
        }
    }
}