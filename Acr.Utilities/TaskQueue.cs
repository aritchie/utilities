using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Utilities
{
    public class TaskQueue : IDisposable
    {
        public int MaxExecutions { get; set; } = 4;
        public int CurrentRunningTasks { get; private set; }
        public bool IsRunning { get; private set; }

        readonly Queue<Func<CancellationToken, Task>> tasks = new Queue<Func<CancellationToken, Task>>();
        CancellationTokenSource cancelSrc;
        TaskCompletionSource<object> tcs;


        ~TaskQueue()
        {
            this.Dispose(false);
        }


        public Task AwaitAll()
        {
            if (this.tcs == null)
                return Task.FromResult<object>(null);

            return this.tcs.Task;
        }


        public void Add(params Func<CancellationToken, Task>[] taskFuncs)
        {
            foreach (var func in taskFuncs)
                this.tasks.Enqueue(func);
        }


        public void Start()
        {
            if (this.IsRunning)
                return;

            this.IsRunning = true;
            this.cancelSrc = new CancellationTokenSource();
            this.StartLoop();
        }


        public void Stop()
        {
            if (this.IsRunning)
                return;

            this.IsRunning = false;
            this.cancelSrc?.Cancel();
        }


        public int Count => this.tasks.Count;


        public void Clear()
        {
            this.tasks.Clear();
        }


        protected virtual void StartLoop()
        {
            Task.Run(async () =>
            {
                this.tcs = new TaskCompletionSource<object>();

                while (!this.cancelSrc.IsCancellationRequested)
                {
                    if (this.tasks.Count == 0 || this.CurrentRunningTasks >= this.MaxExecutions)
                    {
                        this.tcs.TrySetResult(null);
                        await Task.Delay(500); // spin
                    }
                    else
                    {
                        Debug.WriteLine("Starting a task");
                        var taskFunc = this.tasks.Dequeue();
                        var run = taskFunc(this.cancelSrc.Token);
                        this.CurrentRunningTasks++;
                        Debug.WriteLine($"Task {this.CurrentRunningTasks} Started");

                        run.ContinueWith(x =>
                        {
                            this.CurrentRunningTasks--;
                            Debug.WriteLine($"Task {this.CurrentRunningTasks} Finished");
                        });
                    }
                }
            });
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }


        protected virtual void Dispose(bool disposing)
        {
            this.Stop();
            this.tasks.Clear();
        }
    }
}
