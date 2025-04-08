using Android.OS;
using Android.Runtime;
using Android.Speech;
using Java.Interop;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Models;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android
{
    public class AudioRecogitionListener : Java.Lang.Object, IRecognitionListener
    {                                
        private readonly WeakReference<IAudioRecorderConsumer>? Consumer;

        public JniManagedPeerStates JniManagedPeerState => throw new NotImplementedException();

        public AudioRecogitionListener(WeakReference<IAudioRecorderConsumer>? weakConsumer = null)
        {
            Consumer = weakConsumer;
        }

        public void Disposed()
        {
            throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            throw new NotImplementedException();
        }

        public void Finalized()
        {
            throw new NotImplementedException();
        }

        public void OnBeginningOfSpeech()
        {
            throw new NotImplementedException();
        }

        public void OnBufferReceived(byte[]? buffer)
        {
            
        }

        public void OnEndOfSpeech()
        {
            throw new NotImplementedException();
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            throw new NotImplementedException();
        }

        public void OnEvent(int eventType, Bundle? @params)
        {
            throw new NotImplementedException();
        }

        public void OnPartialResults(Bundle? partialResults)
        {
            throw new NotImplementedException();
        }

        public void OnReadyForSpeech(Bundle? @params)
        {
            throw new NotImplementedException();
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
#if ANDROID34_0_OR_GREATER
                language = results.GetString(SpeechRecognizer.DetectedLanguage);
#endif
                float confidence = results.GetFloat(SpeechRecognizer.ConfidenceScores);

                SubmitResults(new SpeechRecognitionResult(result, confidence, language));
            }
        }

        private void SubmitResults(SpeechRecognitionResult result)
        {
            if (Consumer?.TryGetTarget(out var target) == true)
            {
                if(target is IAudioSpeechRecorderConsumer speechConsumer)
                {
                    speechConsumer.SpeechRecognitionResultsReceived(result);
                }                
            }
        }

        public void OnRmsChanged(float rmsdB)
        {
            throw new NotImplementedException();
        }

        public void SetJniIdentityHashCode(int value)
        {
            throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            throw new NotImplementedException();
        }
    }
}
