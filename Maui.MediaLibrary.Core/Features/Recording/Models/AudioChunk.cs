namespace Maui.MediaLibrary.Core.Features.Recording.Models
{
    public class AudioChunk
    {
        public byte[] Bytes { get; } = [];
        public double Loudness { get; }
        public double Frequency { get; }

        internal AudioChunk(byte[] bytes, double loudness, double frequency)
        {
            Bytes = bytes;
            Loudness = loudness;
            Frequency = frequency;
        }
    }
}
