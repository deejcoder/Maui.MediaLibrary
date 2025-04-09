using Android.Content;
using Android.Speech;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android
{
    public class AudioSpeechRecognizer : IAudioRecorder
    {
        private WeakReference<IAudioSpeechRecorderConsumer>? WeakConsumer { get; set; }
        private SpeechRecognizer? Recognizer { get; set; }

        public AudioSpeechRecognizer(IAudioSpeechRecorderConsumer consumer)
        {
            this.WeakConsumer = new WeakReference<IAudioSpeechRecorderConsumer>(consumer);
        }

        public void StartRecording(CancellationToken? cancellationToken = null)
        {
            if (Platform.CurrentActivity == null)
            {
                throw new InvalidOperationException("Current activity is null.");
            }

            if (!SpeechRecognizer.IsRecognitionAvailable(Platform.CurrentActivity))
            {
                throw new InvalidOperationException("Speech recognition is not available on this device.");
            }
            
            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(Platform.CurrentActivity);
            if (Recognizer == null)
            {
                throw new InvalidOperationException("SpeechRecognizer is not available.");
            }

            Recognizer.SetRecognitionListener(new AudioRecogitionListener(WeakConsumer));

            var intent = new Intent(RecognizerIntent.ExtraLanguageModel);
            intent.PutExtra(RecognizerIntent.LanguageModelFreeForm, true);

            if (AudioRecognizerGuards.IsAtLeastAndroid34)
            {
                intent.PutExtra(RecognizerIntent.ExtraRequestWordConfidence, true);
            }

            Recognizer.StartListening(intent);

            cancellationToken?.Register(() => StopRecording());
        }

        public void StopRecording()
        {            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Recognizer != null)
                {
                    Recognizer.StopListening();
                    Recognizer.Destroy();
                    Recognizer.Dispose();
                }

                Recognizer = null;
            });
        }
    }
}
