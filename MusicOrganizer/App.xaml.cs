using System.Windows;

namespace MusicOrganizer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs args) {
            base.OnStartup(args);

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata {
                DefaultValue = FindResource(typeof(Window))
            });
        }
    }
}
