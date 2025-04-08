using Maui.MediaLibrary.Core.Features.Recording.Models;

namespace Maui.MediaLibrary.Core.Features.Recording.Interfaces;

public interface IAudioRecorderConsumer
{
    /// <summary>
    /// Called when an audio chunk is received.
    /// </summary>
    /// <param name="chunk">The audio chunk.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AudioChunkReceived(AudioChunk chunk);

    /// <summary>
    /// Called when the recording is stopped.
    /// </summary>
    void RecordingStopped();
}
