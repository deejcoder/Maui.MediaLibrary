using CommunityToolkit.Mvvm.ComponentModel;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;

namespace Maui.MediaLibrary.Core.Features.Recording.Models
{
    public abstract partial class AudioRecorderConsumer : ObservableObject, IAudioRecorderConsumer
    {
        [ObservableProperty]        
        public partial bool IsRecording { get; set; } = false;

        public abstract void RecordingStarted();
        public abstract void RecordingStopped();
        public abstract Task AudioChunkReceived(AudioChunk chunk);
    }
}
