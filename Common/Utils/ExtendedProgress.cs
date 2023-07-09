namespace Common.Utils;

public interface IExtendedProgress<T> : IProgress<T>
{
    public bool Throbber { get; }
    public T Progress { get; }
    public T MaxProgress { get; }
    public string SubOperation { get; }
    public void SetThrobber(bool value);
    public void SetProgress(T value);
    public void SetMaxProgress(T value);
    public void SetOperation(string value);
    public void SetSubOperation(string value);
}
public class ExtendedProgress<T> : IExtendedProgress<T>
{
    /// <summary>
    /// A throbber is a progressbar that doesn't have a set max value.
    /// It is used to inform users that an operation is ongoing, but it's progression is not known.
    /// </summary>
    public bool Throbber { get; private set; }
    private T initialProgress;
    public T Progress { get; private set; }
    public T MaxProgress { get; private set; }
    public string Operation { get; private set; }
    public string SubOperation { get; private set; }

    public event EventHandler<T>? ProgressChanged;

    public ExtendedProgress(T initialProgress, T maxProgress)
    {
        Operation = "";
        SubOperation = "";
        Throbber = false;
        this.initialProgress = initialProgress;
        Progress = initialProgress;
        MaxProgress = maxProgress;
    }

    void IExtendedProgress<T>.SetThrobber(bool value) {
        this.Progress = initialProgress;
        this.Throbber = value;
        (this as IProgress<T>).Report(this.Progress);
    }

    void IExtendedProgress<T>.SetProgress(T value) {
        this.Progress = value;
        (this as IProgress<T>).Report(this.Progress);
    }
    void IExtendedProgress<T>.SetMaxProgress(T value) {
        this.MaxProgress = value;
        (this as IProgress<T>).Report(this.Progress);
    }
    void IExtendedProgress<T>.SetOperation(string value) {
        this.Progress = initialProgress;
        this.Operation = value;
        this.SubOperation = "";
        (this as IProgress<T>).Report(this.Progress);
    }
    void IExtendedProgress<T>.SetSubOperation(string value) {
        this.SubOperation = value;
        (this as IProgress<T>).Report(this.Progress);
    }

    void IProgress<T>.Report(T value)
    {
        this.Progress = value;
        ProgressChanged?.Invoke(this, value);
    }
}