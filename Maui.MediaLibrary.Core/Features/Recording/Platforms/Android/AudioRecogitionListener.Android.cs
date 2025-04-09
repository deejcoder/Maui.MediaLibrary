using Android.OS;
using Android.Runtime;
using Android.Speech;
using Java.Interop;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Models;
using System.Runtime.Versioning;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android
{
    public class AudioRecogitionListener : Java.Lang.Object, IRecognitionListener
    {
        private readonly WeakReference<IAudioSpeechRecorderConsumer>? WeakConsumer;

        public JniManagedPeerStates JniManagedPeerState => throw new NotImplementedException();

        public AudioRecogitionListener(WeakReference<IAudioSpeechRecorderConsumer>? weakConsumer = null)
        {
            WeakConsumer = weakConsumer;
        }

        public void Disposed()
        {            
        }

        public void DisposeUnlessReferenced()
        {            
        }

        public void Finalized()
        {            
        }

        public void OnBeginningOfSpeech()
        {            
        }

        public void OnBufferReceived(byte[]? buffer)
        {

        }

        public void OnEndOfSpeech()
        {            
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {            
        }

        public void OnEvent(int eventType, Bundle? @params)
        {            
        }

        public void OnPartialResults(Bundle? partialResults)
        {            
        }

        public void OnReadyForSpeech(Bundle? @params)
        {
            if(WeakConsumer?.TryGetTarget(out IAudioSpeechRecorderConsumer? consumer) == true)
            {
                consumer.SpeechRecognitionReady();
            }
        }

        public void OnResults(Bundle? results)
        {
            if (results == null)
            {
                // Potentially should do something here
                return;
            }

            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches != null && matches.Count > 0)
            {
                var result = matches[0];

                // also get the language and confidence
                string language = string.Empty;

                if (AudioRecognizerGuards.IsAtLeastAndroid34)
                {
                    language = results.GetString(SpeechRecognizer.DetectedLanguage) ?? string.Empty;
                }

                float confidence = results.GetFloat(SpeechRecognizer.ConfidenceScores);

                SubmitResults(new SpeechRecognitionResult(result, confidence, language));
            }
        }

        private void SubmitResults(SpeechRecognitionResult result)
        {
            if (WeakConsumer?.TryGetTarget(out IAudioSpeechRecorderConsumer? consumer) == true)
            {
                consumer.SpeechRecognitionReady();
            }
        }

        public void OnRmsChanged(float rmsdB)
        {            
        }

        public void SetJniIdentityHashCode(int value)
        {            
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {            
        }

        public void SetPeerReference(JniObjectReference reference)
        {            
        }
    }
}
