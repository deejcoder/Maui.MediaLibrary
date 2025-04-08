using Maui.MediaLibrary.Features.Tests;

namespace Maui.MediaLibrary
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Routing.RegisterRoute("audioRecorderPage", typeof(AudioRecorderPage));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}