namespace OpenSteamworks.Client.Utils;

public class Backoff {
    public event EventHandler? OnFailedPermanently;
    private int errorsInOneMinute = 0;
    private int maxErrorsInOneMinute = 0;
    private DateTime? startDate;
    public Backoff(int maxErrorsInOneMinute) {
        this.maxErrorsInOneMinute = maxErrorsInOneMinute;
    }

    public void OnError() {
        if (!startDate.HasValue) {
            startDate = DateTime.UtcNow;
        }

        if (startDate.Value.AddMinutes(1) > DateTime.UtcNow) {
            errorsInOneMinute++;
        } else {
            startDate = DateTime.UtcNow;
            errorsInOneMinute = 0;
        }

        if (errorsInOneMinute > maxErrorsInOneMinute) {
            OnFailedPermanently?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnSuccess() {
        startDate = null;
        errorsInOneMinute = 0;
    }
    
}