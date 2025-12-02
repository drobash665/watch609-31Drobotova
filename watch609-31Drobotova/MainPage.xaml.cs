using System.Timers;

namespace watch609_31Drobotova;

public partial class MainPage : ContentPage
{
    private System.Timers.Timer timer;
    private DateTime currentTime;
    private const double CircleCircumference = 1000;

    
    private readonly Color[] pinkGradient = new Color[]
    {
        Color.FromArgb("#FFFFFF"), 
        Color.FromArgb("#FFBCD9"), 
        Color.FromArgb("#EA899A"), 
        Color.FromArgb("#DE5D83"), 
        Color.FromArgb("#B3446C")  
    };

    public MainPage()
    {
        InitializeComponent();

        currentTime = DateTime.Now;
        UpdateTimeDisplay();

        timer = new System.Timers.Timer(50);
        timer.Elapsed += OnTimerElapsed;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            currentTime = DateTime.Now;
            UpdateTimeDisplay();
            UpdateProgressCircle();
        });
    }

    private void UpdateTimeDisplay()
    {
        TimeLabel.Text = currentTime.ToString("HH:mm:ss");

        int cycleSecond = currentTime.Second % 10;
        CycleLabel.Text = $"{cycleSecond}/9";
    }

    private void UpdateProgressCircle()
    {
        int currentSecond = currentTime.Second % 10;
        double milliseconds = currentTime.Millisecond;
        double progress = (currentSecond + milliseconds / 1000.0) / 10.0;

        double filledLength = progress * CircleCircumference;
        double gapLength = CircleCircumference - filledLength;

        ProgressCircle.StrokeDashArray = new double[] { filledLength, gapLength };
        ProgressCircle.Rotation = -90;

        UpdateProgressColor(progress);
    }

    private void UpdateProgressColor(double progress)
    {
        
        if (progress < 0.001)
        {
            
            ProgressCircle.Stroke = pinkGradient[0];
        }
        else if (progress < 0.25)
        {
            
            double localProgress = progress / 0.25;
            ProgressCircle.Stroke = InterpolateColor(pinkGradient[0], pinkGradient[1], localProgress);
        }
        else if (progress < 0.5)
        {
            
            double localProgress = (progress - 0.25) / 0.25;
            ProgressCircle.Stroke = InterpolateColor(pinkGradient[1], pinkGradient[2], localProgress);
        }
        else if (progress < 0.75)
        {
            
            double localProgress = (progress - 0.5) / 0.25;
            ProgressCircle.Stroke = InterpolateColor(pinkGradient[2], pinkGradient[3], localProgress);
        }
        else
        {
            
            double localProgress = (progress - 0.75) / 0.25;
            ProgressCircle.Stroke = InterpolateColor(pinkGradient[3], pinkGradient[4], localProgress);
        }
    }

    
    private Color InterpolateColor(Color color1, Color color2, double progress)
    {
        progress = Math.Max(0, Math.Min(1, progress));

        double r = color1.Red + (color2.Red - color1.Red) * progress;
        double g = color1.Green + (color2.Green - color1.Green) * progress;
        double b = color1.Blue + (color2.Blue - color1.Blue) * progress;

        return Color.FromRgb(r, g, b);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        timer?.Stop();
        timer?.Dispose();
    }
}