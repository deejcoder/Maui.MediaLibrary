using Maui.MediaLibrary.Core.Features.Recording.Models;
using System.ComponentModel;

namespace Maui.MediaLibrary.Core.Features.Recording.Interfaces;

public interface IAudioRecorderConsumer : INotifyPropertyChanged
{    
    bool IsRecording { get; set; }

    /// <summary>
    /// Called when an audio chunk is received.
    /// </summary>
    /// <param name="chunk">The audio chunk.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AudioChunkReceived(AudioChunk chunk);

    void RecordingStarted();

    /// <summary>
    /// Called when the recording is stopped.
    /// </summary>
    void RecordingStopped();
}
