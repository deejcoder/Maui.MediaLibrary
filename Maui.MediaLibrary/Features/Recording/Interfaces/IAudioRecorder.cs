namespace Maui.MediaLibrary.Features.Recording.Interfaces
{
    public interface IAudioRecorder
    {
        void StartRecording(IAudioRecorderConsumer consumer, CancellationToken? cancellationToken = null);
        void StopRecording();
    }
}
