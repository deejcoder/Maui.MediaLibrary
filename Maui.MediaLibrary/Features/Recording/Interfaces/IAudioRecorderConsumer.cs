namespace Maui.MediaLibrary.Features.Recording.Interfaces
{
    public interface IAudioRecorderConsumer
    {
        /// <summary>
        /// Called when an audio chunk is received.
        /// </summary>
        /// <param name="chunk">The audio chunk.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AudioChunkReceived(byte[] chunk);
        /// <summary>
        /// Called when the recording is stopped.
        /// </summary>
        void RecordingStopped();
    }
}
