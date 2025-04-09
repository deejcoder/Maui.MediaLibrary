using Android.Media;
using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Models;


namespace Maui.MediaLibrary.Core.Features.Recording.Platforms.Android;

internal class AudioRecorder : IAudioRecorder
{
    private AudioRecord? AudioRecord { get; set; }
    private int BufferSize;
    private byte[] Buffer;
    private WeakReference<IAudioRecorderConsumer>? Consumer { get; set; }

    public bool IsRecording { get; private set; }

    public AudioRecorder(IAudioRecorderConsumer consumer)
    {
        this.Consumer = new WeakReference<IAudioRecorderConsumer>(consumer);

        BufferSize = AudioRecord.GetMinBufferSize(16000, ChannelIn.Mono, Encoding.Pcm16bit);
        Buffer = new byte[BufferSize];
    }

    public void StartRecording(CancellationToken? cancellationToken = null)
    {        
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

                // Calculate loudness
                double loudness = CalculateLoudness(chunk);

                // Calculate frequency
                double frequency = CalculateFrequency(chunk, 16000); // Assuming 16kHz sample rate

                SubmitAudioChunk(chunk, loudness, frequency);

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

    private void SubmitAudioChunk(byte[] chunk, double loudness, double freq)
    {
        if (Consumer != null && Consumer.TryGetTarget(out var consumer))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                consumer.AudioChunkReceived(new AudioChunk(chunk, loudness, freq));
            });            
        }
        else
        {
            // If the consumer is no longer available, stop the recording
            StopRecording();
        }
    }

    private double CalculateLoudness(byte[] audioChunk)
    {
        double sum = 0;
        for (int i = 0; i < audioChunk.Length; i += 2) // Assuming 16-bit PCM
        {
            short sample = BitConverter.ToInt16(audioChunk, i);
            sum += sample * sample;
        }
        double rms = Math.Sqrt(sum / (audioChunk.Length / 2));
        return 20 * Math.Log10(rms); // Convert to decibels (dB)
    }

    private double CalculateFrequency(byte[] audioChunk, int sampleRate)
    {
        int length = audioChunk.Length / 2; // Assuming 16-bit PCM
        double[] samples = new double[length];
        for (int i = 0; i < length; i++)
        {
            samples[i] = BitConverter.ToInt16(audioChunk, i * 2);
        }

        // convert samples to Complex32
        var complexSamples = new MathNet.Numerics.Complex32[samples.Length];

        for (int i = 0; i < samples.Length; i++)
        {
            complexSamples[i] = new MathNet.Numerics.Complex32((float)samples[i], 0);
        }

        // Perform FFT
        MathNet.Numerics.IntegralTransforms.Fourier.Forward(complexSamples, MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);

        // Find the index of the peak frequency
        int peakIndex = 0;
        double maxAmplitude = 0;
        for (int i = 0; i < samples.Length / 2; i++) // Only consider positive frequencies
        {
            double amplitude = Math.Sqrt(samples[i] * samples[i]);
            if (amplitude > maxAmplitude)
            {
                maxAmplitude = amplitude;
                peakIndex = i;
            }
        }

        // Calculate the frequency
        double frequency = (double)peakIndex * sampleRate / length;
        return frequency;
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

            await Task.Delay(1000);
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
