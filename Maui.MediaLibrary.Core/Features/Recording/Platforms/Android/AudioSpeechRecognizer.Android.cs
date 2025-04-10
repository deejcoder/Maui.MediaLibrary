using Android.Content;
using Android.Speech;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Platforms.Shared;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android
{
    public class AudioSpeechRecognizer : AudioRecorderBase, IAudioRecorder
    {        
        private SpeechRecognizer? Recognizer { get; set; }

        private WeakReference<IAudioSpeechRecorderConsumer> WeakSpeechConsumer { get; set; }

        public AudioSpeechRecognizer(IAudioSpeechRecorderConsumer consumer) : base(consumer)
        {
            WeakSpeechConsumer = new WeakReference<IAudioSpeechRecorderConsumer>(consumer);
        }

        public override void StartRecording(CancellationToken? cancellationToken = null)
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

            Recognizer.SetRecognitionListener(new AudioRecogitionListener(WeakSpeechConsumer));

            var intent = new Intent(RecognizerIntent.ExtraLanguageModel);
            intent.PutExtra(RecognizerIntent.LanguageModelFreeForm, true);

            if (AudioRecognizerGuards.IsAtLeastAndroid34)
            {
                intent.PutExtra(RecognizerIntent.ExtraRequestWordConfidence, true);
            }

            Recognizer.StartListening(intent);

            base.RaiseRecordingStarted();

            cancellationToken?.Register(() => StopRecording());
        }

        public override void StopRecording()
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

                base.RaiseRecordingStopped();
            });
        }
    }
}
