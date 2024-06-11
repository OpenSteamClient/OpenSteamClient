using System.ComponentModel;

namespace OpenSteamworks.Client.Utils;

/// <summary>
/// Wraps multiple Progress<> elements to create an ExtendedProgress
/// </summary>
/// <typeparam name="T"></typeparam>
public class ExtendedProgressWrap<T> : IExtendedProgress<T>
{
    /// <inheritdoc/>
    public bool Throbber => _progress == null || _maxProgress == null;

    public T InitialProgress { get; private set; }
    public T Progress { get; private set; }
    public T MaxProgress { get; private set; }
    public string Operation { get; private set; }
    public string SubOperation { get; private set; }
    public event EventHandler<T>? ProgressChanged;

    private readonly Progress<T>? _progress;
    private readonly Progress<T>? _maxProgress;
    private readonly Progress<string>? _operation;
    private readonly Progress<string>? _subOperation;

    public ExtendedProgressWrap(Progress<string> operation, Progress<string> subOperation, T initialProgress, Progress<T>? progress, Progress<T>? maxProgress)
    {
        this.InitialProgress = initialProgress;
        this.Progress = initialProgress;
        this.MaxProgress = initialProgress;
        this.Operation = string.Empty;
        this.SubOperation = string.Empty;
        
        this._operation = operation;
        this._subOperation = subOperation;
        this._progress = progress;
        this._maxProgress = maxProgress;

        operation.ProgressChanged += (object? sender, string val) =>
        {
            this.Operation = val;
            ReportChanged();
        };

        subOperation.ProgressChanged += (object? sender, string val) =>
        {
            this.SubOperation = val;
            ReportChanged();
        };

        if (progress != null) {
            progress.ProgressChanged += (object? sender, T val) =>
            {
                this.Progress = val;
                ReportChanged();
            };
        }

        if (maxProgress != null) {
            maxProgress.ProgressChanged += (object? sender, T val) =>
            {
                this.MaxProgress = val;
                ReportChanged();
            };
        }
    }

    private void ReportChanged() {
        ProgressChanged?.Invoke(this, this.Progress);
    }

    void IProgress<T>.Report(T value)
    {
        throw new InvalidOperationException("Cannot report with the wrapper class!");
    }

    public void SetThrobber()
    {
        throw new InvalidOperationException("Cannot report with the wrapper class!");
    }

    public void SetProgress(T value)
    {
        throw new InvalidOperationException("Cannot report with the wrapper class!");
    }

    public void SetMaxProgress(T value)
    {
        throw new InvalidOperationException("Cannot report with the wrapper class!");
    }

    public void SetOperation(string value)
    {
        throw new InvalidOperationException("Cannot report with the wrapper class!");
    }

    public void SetSubOperation(string value)
    {
        throw new InvalidOperationException("Cannot report with the wrapper class!");
    }
}