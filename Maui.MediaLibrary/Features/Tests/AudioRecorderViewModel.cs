using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Models;

namespace Maui.MediaLibrary.Features.Tests
{
    public partial class AudioRecorderViewModel : ObservableObject, IAudioRecorderConsumer
    {
        [ObservableProperty]
        public partial bool IsRecording { get; set; }

        [ObservableProperty]
        public partial string RecordingButtonText { get; set; } = "Start Recording";

        [RelayCommand]
        private void StartStopRecording()
        {
            IsRecording = !IsRecording;
            RecordingButtonText = IsRecording ? "Stop Recording" : "Start Recording";
        }

        public Task AudioChunkReceived(AudioChunk chunk)
        {
            return Task.CompletedTask;
        }

        public void RecordingStopped()
        {
            throw new NotImplementedException();
        }
    }
}
