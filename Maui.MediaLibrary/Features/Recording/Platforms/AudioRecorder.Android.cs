using Android.Media;
using Maui.MediaLibrary.Features.Recording.Interfaces;


namespace Maui.MediaLibrary.Features.Recording.Platforms
{
    internal class AudioRecorder : IAudioRecorder
    {
        private AudioRecord? AudioRecord { get; set; }
        private int BufferSize;
        private byte[] Buffer;
        private IAudioRecorderConsumer? Consumer { get; set; }

        public bool IsRecording { get; private set; }

        public AudioRecorder()
        {
            BufferSize = AudioRecord.GetMinBufferSize(16000, ChannelIn.Mono, Encoding.Pcm16bit);
            Buffer = new byte[BufferSize];

        }

        public void StartRecording(IAudioRecorderConsumer consumer, CancellationToken? cancellationToken = null)
        {
            this.Consumer = consumer;

            cancellationToken ??= CancellationToken.None;

            AudioRecord = new AudioRecord(
                AudioSource.Mic,
                16000,
                ChannelIn.Mono,
                Encoding.Pcm16bit,
                BufferSize);

            AudioRecord.StartRecording();
            IsRecording = true;

            // Start reading audio data asynchronously
            _ = Task.Run(async () => await Listen(cancellationToken.Value));
        }

        private async Task Listen(CancellationToken cancellationToken)
        {
            try
            {
                if (AudioRecord == null)
                {
                    throw new InvalidOperationException("AudioRecord is not initialized.");
                }

                await foreach (var chunk in ReadAudioDataAsync())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    this.Consumer?.AudioChunkReceived(chunk);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch(TaskCanceledException)
            {

            }
            catch(Exception ex)
            {

            }
        }

        private async IAsyncEnumerable<byte[]> ReadAudioDataAsync()
        {
            if (AudioRecord == null)
            {
                throw new InvalidOperationException("AudioRecord is not initialized.");
            }

            while (IsRecording)
            {
                int bytesRead = await AudioRecord.ReadAsync(Buffer, 0, BufferSize);
                if (bytesRead > 0)
                {
                    byte[] chunk = new byte[bytesRead];
                    Array.Copy(Buffer, chunk, bytesRead);
                    yield return chunk;
                }
            }
        }

        public void StopRecording()
        {
            if(!IsRecording)
            {
                return;
            }

            if(AudioRecord == null)
            {
                throw new InvalidOperationException("AudioRecord is not initialized.");
            }

            AudioRecord.Stop();
            AudioRecord.Release();
            IsRecording = false;
        }
    }
}
