using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Utilities
{
    public class TaskQueue
    {
        public int MaxExecutions { get; set; } = 4;
        public int CurrentRunningTasks { get; private set; }
        public bool IsRunning { get; private set; }

        readonly Queue<Func<CancellationToken, Task>> tasks = new Queue<Func<CancellationToken, Task>>();
        CancellationTokenSource cancelSrc;


        public void Add(Func<CancellationToken, Task> task)
        {
            this.tasks.Enqueue(task);
        }


        public void Start()
        {
            if (this.IsRunning)
                return;

            this.IsRunning = true;
            this.cancelSrc = new CancellationTokenSource();
            this.Run();
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


        Task Run()
        {
            return Task.Run(async () =>
            {
                while (!this.cancelSrc.IsCancellationRequested)
                {
                    if (this.tasks.Count == 0 || this.CurrentRunningTasks >= this.MaxExecutions)
                        await Task.Delay(500); // spin
                    else
                    {
                        var taskFunc = this.tasks.Dequeue();
                        var run = taskFunc(this.cancelSrc.Token);
                        this.CurrentRunningTasks++;
                        run.ContinueWith(x => this.CurrentRunningTasks--);
                    }
                }
            });
        }
    }
}
