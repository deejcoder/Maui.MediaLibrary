using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Platforms;

namespace Maui.MediaLibrary.Core.Controls;

public partial class AudioRecordingView : ContentView, IAudioRecordingView
{
    private readonly IAudioRecorder Recorder;
    private CancellationTokenSource? RecorderCancellationTokenSource { get; set; }

    public static readonly BindableProperty ConsumerProperty =
        BindableProperty.Create(nameof(Consumer), typeof(IAudioRecorderConsumer), typeof(AudioRecordingView), null);
    
    public IAudioRecorderConsumer Consumer
    {
        get => (IAudioRecorderConsumer)GetValue(ConsumerProperty);
        set => SetValue(ConsumerProperty, value);
    }
    
    public static readonly BindableProperty IsRecordingProperty =
        BindableProperty.Create(nameof(IsRecording), typeof(bool), typeof(AudioRecordingView), false, propertyChanged: IsRecordingPropertyChanged);
    
    public bool IsRecording
    {
        get => (bool)GetValue(IsRecordingProperty);
        set => SetValue(IsRecordingProperty, value);
    }

    private static void IsRecordingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (AudioRecordingView)bindable;

        if (oldValue == newValue) return;

        if(newValue is bool isRecording)
        {
            if (isRecording)
            {
                view.StartRecording();
            }
            else
            {
                view.StopRecording();
            }
        }
    }

    
    public static readonly BindableProperty TimeoutProperty =
        BindableProperty.Create(nameof(Timeout), typeof(TimeSpan), typeof(AudioRecordingView), TimeSpan.FromSeconds(30));

    public TimeSpan Timeout
    {
        get => (TimeSpan)GetValue(TimeoutProperty);
        set => SetValue(TimeoutProperty, value);
    }
    
    public AudioRecordingView()
	{
		InitializeComponent();

        this.Recorder = AudioRecorderFactory.Create();
	}

    private async Task StartRecording()
    {
        if (Consumer != null)
        {
            // TODO: deal with thread safety
            await RequestPermission();

            RecorderCancellationTokenSource = new(Timeout);
            Recorder.StartRecording(Consumer, RecorderCancellationTokenSource.Token);
            IsRecording = true;
        }
    }

    private async Task RequestPermission()
    {
        var status = await Permissions.RequestAsync<Permissions.Microphone>();

        // TODO...
        if (status == PermissionStatus.Granted)
        {
        }
        else if (status == PermissionStatus.Denied)
        {
        }
        else
        {
        }
    }

    private void StopRecording()
    {
        if (Consumer != null)
        {
            Recorder.StopRecording();
            IsRecording = false;
        }
    }
}