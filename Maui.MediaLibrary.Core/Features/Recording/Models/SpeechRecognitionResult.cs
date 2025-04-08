namespace Maui.MediaLibrary.Core.Features.Recording.Models
{
    public class SpeechRecognitionResult
    {
        /// <summary>
        /// Gets or sets the recognized text.
        /// </summary>
        public string RecognizedText { get; set; }
        /// <summary>
        /// Gets or sets the confidence level of the recognition.
        /// </summary>
        public float Confidence { get; set; }
        /// <summary>
        /// Gets or sets the language of the recognized speech.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// True when the transcription is partial and not final.
        /// </summary>
        public bool IsPartial { get; set; } = false;


        public SpeechRecognitionResult(string recognizedText, float confidence, string language)
        {
            RecognizedText = recognizedText;
            Confidence = confidence;
            Language = language;
        }
    }
}
