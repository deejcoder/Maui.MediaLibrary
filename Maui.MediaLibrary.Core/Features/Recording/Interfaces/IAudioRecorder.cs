namespace Maui.MediaLibrary.Core.Features.Recording.Interfaces;

public interface IAudioRecorder
{
    event EventHandler? RecordingStarted;
    event EventHandler? RecordingStopped;

    void StartRecording(CancellationToken? cancellationToken = null);
    void StopRecording();
}
