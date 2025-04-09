namespace Maui.MediaLibrary.Core.Features.Recording.Interfaces;

public interface IAudioRecorder
{
    void StartRecording(CancellationToken? cancellationToken = null);
    void StopRecording();
}
