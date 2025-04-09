using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
#if ANDROID
using Maui.MediaLibrary.Core.Features.Recording.Platforms.Android;
#endif

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms;

public class AudioRecorderFactory
{
    public static IAudioRecorder Create(IAudioRecorderConsumer consumer)
    {
#if ANDROID
        switch(consumer)
        {
            case IAudioSpeechRecorderConsumer speechConsumer:
                return new AudioSpeechRecognizer(speechConsumer);                            
            default:
                return new AudioRecorder(consumer);
        }        
#else
        throw new NotSupportedException("Audio recording is not supported on this platform.");
#endif
    }
}
