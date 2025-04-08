using Maui.MediaLibrary.Core.Features.Recording.Interfaces;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms;

public class AudioRecorderFactory
{
    public static IAudioRecorder Create()
    {
#if ANDROID
        return new AudioRecorder();
#else
        throw new NotSupportedException("Audio recording is not supported on this platform.");
#endif
    }
}
