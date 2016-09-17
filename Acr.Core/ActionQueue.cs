﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Core
{
    public class ActionQueue : IDisposable
    {
        readonly Queue<Action> actions = new Queue<Action>();
        public bool IsRunning { get; private set; }
        public bool IsExecutingAction { get; private set; }
        public event EventHandler<Exception> Error;

        CancellationTokenSource cancelSrc;


        ~ActionQueue()
        {
            this.Dispose(false);
        }


        public void Add(params Action[] actionArray)
        {
            foreach (var action in actionArray)
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
            if (!this.IsRunning)
                return;

            this.IsRunning = false;
            this.cancelSrc?.Cancel();
        }


        public int Count => this.actions.Count;


        public void Clear()
        {
            this.actions.Clear();
        }


        void Run()
        {
            Task.Run(async () =>
            {
                while (!this.cancelSrc.IsCancellationRequested)
                {
                    if (this.actions.Count == 0)
                    {
                        Debug.WriteLine("No action items");
                        await Task.Delay(500); // spin
                    }
                    else
                    {
                        var action = this.actions.Dequeue();
                        try
                        {
                            Debug.WriteLine("Action executing");
                            this.IsExecutingAction = true;
                            action();
                            Debug.WriteLine("Action completed successfully");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error executing action - {ex}");
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


        public void Dispose()
        {
            this.Dispose(true);
        }


        protected virtual void Dispose(bool disposing)
        {
            this.actions.Clear();
            this.cancelSrc?.Cancel();
        }
    }
}