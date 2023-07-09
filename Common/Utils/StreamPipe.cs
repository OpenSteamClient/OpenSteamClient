namespace Common.Utils;

public class StreamPiper
{
    public Stream Source { get; protected set; }
    public Stream Destination { get; protected set; }
    private Task? worker;

    bool shouldRun = true;
    bool stopped = false;
    bool hasWritten = false;

    private StreamPiper(Stream source, Stream destination)
    {
        Source = source;
        Destination = destination;
    }

    public static StreamPiper StartPiping(Stream source, Stream destination)
    {
        StreamPiper pipe = new StreamPiper(source, destination);
        pipe.worker = Task.Run(async () =>
        {
            byte[] buffer = new byte[4096];
            while (pipe.shouldRun)
            {
                if (pipe.Destination.CanWrite && pipe.Source.CanRead)
                {
                    var count = await pipe.Source.ReadAsync(buffer, 0, 4096);
                    if (count > 0)
                    {
                        await pipe.Destination.WriteAsync(buffer, 0, count);
                        await pipe.Destination.FlushAsync();

                        pipe.hasWritten = true;

                        if (!pipe.shouldRun)
                        {
                            pipe.stopped = true;
                        }
                    }
                } else if (pipe.hasWritten) {
                    // If the pipe closes afterwards, stop piping.
                    Console.WriteLine("Closed");
                    pipe.StopPiping();
                }
            }
        });
        return pipe;
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