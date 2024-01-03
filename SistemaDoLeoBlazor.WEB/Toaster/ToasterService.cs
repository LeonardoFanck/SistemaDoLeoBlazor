using System.Timers;
namespace SistemaDoLeoBlazor.WEB.Toaster;

public class ToasterService : IDisposable
{
    private readonly List<Toast> _toastList = new List<Toast>();
    private System.Timers.Timer _timer = new System.Timers.Timer();
    public event EventHandler? ToasterChanged;
    public event EventHandler? ToasterTimerElapsed;
    public bool HasToasts => _toastList.Count > 0;

    public ToasterService()
    {
        _timer.Interval = 1000;
        _timer.AutoReset = true;
        _timer.Elapsed += this.TimerElapsed;
        _timer.Start();
    }

    public List<Toast> GetToasts()
    {
        ClearBurntToast();
        return _toastList;
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        this.ClearBurntToast();
        this.ToasterTimerElapsed?.Invoke(this, EventArgs.Empty);
    }

    public void AddToast(Toast toast)
    {
        _toastList.Add(toast);
        // only raise the ToasterChanged event if it hasn't already been raised by ClearBurntToast
        if (!this.ClearBurntToast())
            this.ToasterChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearToast(Toast toast)
    {
        if (_toastList.Contains(toast))
        {
            _toastList.Remove(toast);
            // only raise the ToasterChanged event if it hasn't already been raised by ClearBurntToast
            if (!this.ClearBurntToast())
                this.ToasterChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool ClearBurntToast()
    {
        var toastsToDelete = _toastList.Where(item => item.IsBurnt).ToList();
        if (toastsToDelete is not null && toastsToDelete.Count > 0)
        {
            toastsToDelete.ForEach(toast => _toastList.Remove(toast));
            this.ToasterChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        if (_timer is not null)
        {
            _timer.Elapsed += this.TimerElapsed;
            _timer.Stop();
        }
    }
}
