namespace Maui.MediaLibrary.Features.Recording.Interfaces
{
    public interface IAudioRecordingView
    {
        Task AudioChunkReceived(byte[] chunk);
    }
}
