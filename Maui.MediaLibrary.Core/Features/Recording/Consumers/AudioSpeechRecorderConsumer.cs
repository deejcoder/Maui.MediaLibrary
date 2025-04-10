using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Models;

namespace Maui.MediaLibrary.Core.Features.Recording.Consumers
{
    public abstract class AudioSpeechRecorderConsumer : AudioRecorderConsumer, IAudioSpeechRecorderConsumer
    {
        public abstract void SpeechRecognitionError();
        public abstract void SpeechRecognitionReady();
        public abstract Task SpeechRecognitionResultsReceived(SpeechRecognitionResult results);
    }
}
