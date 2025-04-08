namespace Maui.MediaLibrary.Features.Tests;

public partial class AudioRecorderPage : ContentPage
{
    private AudioRecorderViewModel ViewModel => (AudioRecorderViewModel)BindingContext;

    public AudioRecorderPage(AudioRecorderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }
}