using Maui.MediaLibrary.Core.Features.Recording.Models;

namespace Maui.MediaLibrary.Core.Features.Recording.Interfaces
{
    public interface IAudioSpeechRecorderConsumer : IAudioRecorderConsumer
    {
        /// <summary>
        /// Called when speech recognition results are received.
        /// </summary>
        /// <param name="results">The speech recognition results.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SpeechRecognitionResultsReceived(SpeechRecognitionResult results);        

        /// <summary>
        /// Called when speech recognition is ready.
        /// </summary>
        void SpeechRecognitionReady();
        /// <summary>
        /// Called when speech recognition encounters an error.
        /// </summary>
        void SpeechRecognitionError();
    }
}
