using Android.Speech;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android
{
    public class AudioSpeechRecognizer : IAudioRecorder
    {
        private WeakReference<IAudioRecorderConsumer>? Consumer { get; set; }

        public AudioSpeechRecognizer()
        {

        }

        public void StartRecording(IAudioRecorderConsumer consumer, CancellationToken? cancellationToken = null)
        {
            this.Consumer = new WeakReference<IAudioRecorderConsumer>(consumer);

            // TODO: finish this
            SpeechRecognizer? recognizer = SpeechRecognizer.CreateSpeechRecognizer(Platform.CurrentActivity);
            if(recognizer == null)
            {
                throw new InvalidOperationException("SpeechRecognizer is not available.");
            }

            recognizer.SetRecognitionListener(new AudioRecogitionListener(Consumer));
            
            // ...
        }

        public void StopRecording()
        {
            throw new NotImplementedException();
        }
    }
}
