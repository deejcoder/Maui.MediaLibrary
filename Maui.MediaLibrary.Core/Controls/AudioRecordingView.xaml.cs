using Maui.MediaLibrary.Core.Features.Recording.Interfaces;
using Maui.MediaLibrary.Core.Features.Recording.Platforms;

namespace Maui.MediaLibrary.Core.Controls;

public partial class AudioRecordingView : ContentView, IAudioRecordingView
{
    private IAudioRecorder? Recorder { get; set; }
    private CancellationTokenSource? RecorderCancellationTokenSource { get; set; }

    public static readonly BindableProperty ConsumerProperty =
        BindableProperty.Create(nameof(Consumer), typeof(IAudioRecorderConsumer), typeof(AudioRecordingView), null, propertyChanged: ConsumerPropertyChanged);
    
    public IAudioRecorderConsumer Consumer
    {
        get => (IAudioRecorderConsumer)GetValue(ConsumerProperty);
        set => SetValue(ConsumerProperty, value);
    }

    private static void ConsumerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (AudioRecordingView)bindable;
        if (oldValue == newValue) return;

        if (oldValue is IAudioRecorderConsumer oldConsumer)
        {
            view.TearDownRecorder(oldConsumer);
        }

        if (newValue is IAudioRecorderConsumer newConsumer)
        {
            view.SetupRecorder(newConsumer);
        }        
    }

    public static readonly BindableProperty IsRecordingProperty =
        BindableProperty.Create(nameof(IsRecording), typeof(bool), typeof(AudioRecordingView), false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: IsRecordingPropertyChanged);
    
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
                // TODO: handle this better
                view.Dispatcher.DispatchAsync(async () => await view.StartRecording());
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
	}

    private void SetupRecorder(IAudioRecorderConsumer consumer)
    {
        this.Recorder = AudioRecorderFactory.Create(consumer);
        this.Recorder.RecordingStopped += OnRecordingStopped;
        this.Recorder.RecordingStarted += OnRecordingStarted;
    }

    private void TearDownRecorder(IAudioRecorderConsumer consumer)
    {
        if (Recorder != null)
        {
            Recorder.StopRecording();
            this.Recorder.RecordingStopped -= OnRecordingStopped;
            this.Recorder.RecordingStarted -= OnRecordingStarted;
            Recorder = null;
        }
    }

    private void OnRecordingStopped(object? sender, EventArgs e)
    {
        IsRecording = false;
        RecorderCancellationTokenSource = null;
    }

    private void OnRecordingStarted(object? sender, EventArgs e)
    {
        IsRecording = true;
    }


    private async Task StartRecording()
    {
        if (Recorder != null)
        {
            // Request microphone permission
            await RequestPermission();

            // Set a timeout for when the recording should stop
            RecorderCancellationTokenSource = new(Timeout);
            
            Recorder.StartRecording(RecorderCancellationTokenSource.Token);
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
        if (Recorder != null)
        {
            Recorder.StopRecording();
            IsRecording = false;
        }
    }
}