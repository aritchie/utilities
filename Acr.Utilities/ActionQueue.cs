using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Utilities
{
    public class ActionQueue
    {
        readonly Queue<Action> actions = new Queue<Action>();
        public bool IsRunning { get; private set; }
        public bool IsExecutingAction { get; private set; }
        public event EventHandler<Exception> Error;

        CancellationTokenSource cancelSrc;


        public void Add(Action action)
        {
            this.actions.Enqueue(action);
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
        }


        public int Count => this.actions.Count;


        public void Clear()
        {
            this.actions.Clear();
        }


        Task Run()
        {
            return Task.Run(async () =>
            {
                while (!this.cancelSrc.IsCancellationRequested)
                {
                    if (this.actions.Count == 0)
                        await Task.Delay(500); // spin
                    else
                    {
                        var action = this.actions.Dequeue();
                        try
                        {
                            this.IsExecutingAction = true;
                            action();
                        }
                        catch (Exception ex)
                        {
                            this.Error?.Invoke(this, ex);
                        }
                        finally
                        {
                            this.IsExecutingAction = false;
                        }
                    }
                }
            });
        }
    }
}
