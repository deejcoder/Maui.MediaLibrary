

using Maui.MediaLibrary.Features.Recording.Interfaces;

namespace Maui.MediaLibrary.Controls.Recording;

public partial class AudioRecordingView : ContentView, IAudioRecordingView
{
	public AudioRecordingView()
	{
		InitializeComponent();
	}

    public Task AudioChunkReceived(byte[] chunk)
    {
        // TODO
    }
}