using System.Runtime.Versioning;

namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android
{
    internal static class AudioRecognizerGuards
    {
        [SupportedOSPlatformGuard("android34.0")]
        internal static readonly bool IsAtLeastAndroid34 = OperatingSystem.IsAndroidVersionAtLeast(34);
    }
}
