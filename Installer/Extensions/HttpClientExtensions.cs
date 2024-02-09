using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Installer.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<int> progress, long knownLength = 0, CancellationToken cancellationToken = default)
    {
        // Get the http headers first to examine the content length
        using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
        {
            long contentLength = 0;
            if (knownLength > 0) {
                contentLength = knownLength;
            } else if (response.Content.Headers.ContentLength != null) {
                contentLength = (long)response.Content.Headers.ContentLength;
            }

            using (var download = await response.Content.ReadAsStreamAsync())
            {
                // Ignore progress reporting when no progress reporter was 
                // passed or when the content length is unknown
                if (progress == null || contentLength == 0)
                {
                    await download.CopyToAsync(destination);
                    return;
                }

                bool cancelledDueToTimeout = false;

                using (CancellationTokenSource source = new CancellationTokenSource())
                {
                    System.Timers.Timer timer = new System.Timers.Timer(15000);
                    timer.AutoReset = false;
                    timer.Start();
                    timer.Elapsed += (object? sender, ElapsedEventArgs args) => {
                        cancelledDueToTimeout = true;
                        source.Cancel();
                    };

                    // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                    // Also reset the timer by setting it's interval
                    var relativeProgress = new Progress<long>((long totalBytes) => {
                        progress.Report((int)(((float)totalBytes / contentLength)*100));
                        timer.Interval = 20000;
                    });

                    CancellationToken token = source.Token;
                    cancellationToken.Register(() => {
                        cancelledDueToTimeout = false;
                        source.Cancel();
                    });

                    try
                    {
                        // Use extension method to report progress while downloading
                        await download.CopyToAsync(destination, 81920, relativeProgress, token);
                        // If download finished, stop the timer
                        timer.Stop();
                    } 
                    catch (OperationCanceledException)
                    {
                        if (!cancelledDueToTimeout) {
                            throw;
                        }
                        throw new Exception("Lost connection to server");
                    }
                }

                progress.Report(100);
            }
        }
    }
}