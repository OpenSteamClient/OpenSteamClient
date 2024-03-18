using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Profiler;

public class CProfiler
{
    public static CProfiler? CurrentProfiler { get; set; } = new("Startup") { PrintStatsOnScopeEnd = true };

    public string Name { get; init; }
    public bool PrintStatsOnScopeEnd { get; set; } = false;

    private Node RootNode { get; set; }
    private Node? currentNode;
    private int validThread = 0;
    
    private class ProcessedNode {
        public bool IsProfiler { get; init; }
        public string Name { get; init; }
        public long LastElapsed { get; init; }
        public long TotalElapsed => Calls.Sum();
        public double Average {
            get
            {
                if (Calls.Count == 0) {
                    return 0;
                }

                return Calls.Average();
            }
        }

        public List<long> Calls { get; init; } = new();
        public List<ProcessedNode> Children { get; init; } = new();

        public ProcessedNode(string name, long lastElapsed, bool isProfiler) {
            this.IsProfiler = isProfiler;
            this.Name = name;
            this.LastElapsed = lastElapsed;
            this.Calls.Add(lastElapsed);
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            if (IsProfiler) {
                builder.AppendLine($"Profiler-'{this.Name}': Total: {TotalElapsed}ms, Children: {this.Children.Count}");
            } else {
                builder.AppendLine($"Scope-'{this.Name}': Total: {TotalElapsed}ms, Last: {LastElapsed}ms, Average: {Average.ToString("0.00")}ms, Calls: {this.Calls.Count}, Children: {this.Children.Count}");
            }
            
            foreach (var child in this.Children)
            {
                foreach (var line in child.ToString().TrimEnd().Split('\n'))
                    if (!string.IsNullOrWhiteSpace(line))
                        builder.AppendLine($"\t{line}");
            }

            return builder.ToString();
        }

        public void AddChild(Node node) {
            ProcessedNode? targetChild = Children.Find(c => c.Name == node.Name);
            if (targetChild == null) {
                // Add a child to the unique list
                targetChild = node.ToProcessed();
                Children.Add(targetChild);
            } else {
                targetChild.Calls.Add(node.ElapsedMilliseconds);
            }
        }
    }

    public interface INodeLifetime : IDisposable { }

    public sealed class NodeLifetime : INodeLifetime {
        private readonly CProfiler profiler;
        internal NodeLifetime(CProfiler profiler) {
            this.profiler = profiler;
        }
        
        public void Dispose()
        {
            profiler.ExitScope();
        }
    }
    
    public sealed class DummyNodeLifetime : INodeLifetime {        
        public void Dispose()
        {
            
        }
    }

    private sealed class Node {
        public bool IsProfiler { get; internal set; } = false;
        public string Name { get; set; }
        public long ElapsedMilliseconds => stopwatch.ElapsedMilliseconds;
        public Stopwatch stopwatch;
        public List<Node> Children { get; init; } = new();
        public Node? parent;

        public Node(Node? parent, string name) {
            this.parent = parent;
            this.Name = name;
            stopwatch = new();
            stopwatch.Start();
        }

        public Node EnterChild(string name) {
            var node = new Node(this, name);
            Children.Add(node);
            return node;
        }

        public Node? Leave() {
            stopwatch.Stop();
            return parent;
        }

        public ProcessedNode ToProcessed() {
            ProcessedNode node = new(this.Name, this.ElapsedMilliseconds, IsProfiler);
            foreach (var item in this.Children)
            {
                node.AddChild(item);
            }

            return node;
        }
    }


    /// <summary>
    /// Marks the start of an operation's execution. Remember to dispose the resulting object once the operation is finished
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public INodeLifetime EnterScope(string name) {
        if (Environment.CurrentManagedThreadId != validThread) {
            return new DummyNodeLifetime();
        }

        if (currentNode == null) {
            currentNode = RootNode.EnterChild(name);
        } else {
            currentNode = currentNode.EnterChild(name);
        }

        return nodeLifetime;
    }

    /// <summary>
    /// Mark the end of an operation's execution, you can also just call dispose with the object returned from EnterScope
    /// </summary>
    public void ExitScope() {
        if (Environment.CurrentManagedThreadId != validThread) {
            return;
        }

        currentNode = currentNode?.Leave();
    }

    private readonly NodeLifetime nodeLifetime;
    public CProfiler(string profilerName) {
        this.nodeLifetime = new NodeLifetime(this);
        this.Name = profilerName;
        this.RootNode = new Node(null, profilerName) { IsProfiler = true };
        this.validThread = Environment.CurrentManagedThreadId;
    }

    public void Reset() {
        this.validThread = Environment.CurrentManagedThreadId;
        this.RootNode = new Node(null, Name) { IsProfiler = true };
    }

    public void Stop() {
        this.RootNode.Leave();
    }

    public void PrintStats() {
        var processed = this.RootNode.ToProcessed();
        Console.Write(processed.ToString());
    }
}
