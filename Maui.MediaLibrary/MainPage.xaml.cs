namespace Maui.MediaLibrary
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void RecordingClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("audioRecorderPage");
        }
    }

}
