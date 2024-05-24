namespace OpenSteamworks.Client.Utils;

public class StreamPiper
{
    public Stream Source { get; private set; }
    public Stream Destination { get; private set; }
    private readonly Thread worker;

    bool shouldRun = true;
    bool stopped = false;
    bool hasWritten = false;

    public event EventHandler? StreamPositionChanged;

    public StreamPiper(Stream source, Stream destination)
    {
        Source = source;
        Destination = destination;
        worker = new Thread(this.ThreadMain)
        {
            Name = $"StreamPiper Worker ({this.Source.GetType().Name} -> {this.Destination.GetType().Name})"
        };
    }

    public static StreamPiper CreateAndStartPiping(Stream source, Stream destination) {
        StreamPiper piper = new(source, destination);
        piper.StartPiping();
        return piper;
    }
    
    public void StartPiping()
    {
        worker.Start();
    }

    private async void ThreadMain() {
        byte[] buffer = new byte[4096];
        while (this.shouldRun)
        {
            if (this.Destination.CanWrite && this.Source.CanRead)
            {
                var count = await this.Source.ReadAsync(buffer, 0, 4096);
                this.StreamPositionChanged?.Invoke(this, EventArgs.Empty);

                if (count > 0)
                {
                    try {
                        await this.Destination.WriteAsync(buffer, 0, count);
                        await this.Destination.FlushAsync();
                    } catch (IOException) {
                        this.shouldRun = false;
                    }

                    this.hasWritten = true;
                }
            } else if (this.hasWritten) {
                // If the pipe closes afterwards, stop piping.
                Console.WriteLine("Closed");
                this.StopPiping();
            }
        }
        this.stopped = true;
    }

    public void StopPiping()
    {
        shouldRun = false;
        do
        {
            Thread.Sleep(10);
        } while (!this.stopped);
    }
}