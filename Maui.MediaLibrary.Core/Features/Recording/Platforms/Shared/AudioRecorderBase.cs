using Maui.MediaLibrary.Core.Features.Recording.Interfaces;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Shared
{
    public abstract class AudioRecorderBase : IAudioRecorder
    {
        public event EventHandler? RecordingStarted;
        public event EventHandler? RecordingStopped;

        protected WeakReference<IAudioRecorderConsumer>? WeakConsumer { get; set; }

        public AudioRecorderBase(IAudioRecorderConsumer consumer)
        {
            WeakConsumer = new(consumer);
        }

        public abstract void StartRecording(CancellationToken? cancellationToken = null);
        public abstract void StopRecording();

        protected void RaiseRecordingStarted()
        {
            RecordingStarted?.Invoke(this, EventArgs.Empty);

            if (WeakConsumer?.TryGetTarget(out IAudioRecorderConsumer? consumer) == true)
            {
                consumer.RecordingStarted();
            }
        }

        protected void RaiseRecordingStopped()
        {
            RecordingStopped?.Invoke(this, EventArgs.Empty);

            if (WeakConsumer?.TryGetTarget(out IAudioRecorderConsumer? consumer) == true)
            {
                consumer.RecordingStopped();
            }
        }
    }
}
