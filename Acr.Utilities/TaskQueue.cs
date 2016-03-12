using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Acr.Utilities
{
    public class TaskQueue
    {
        public int MaxExecutions { get; set; } = 4;
        readonly Queue<Task> tasks = new Queue<Task>();


        public void Add(Task task)
        {
            this.tasks.Enqueue(task);
        }


        public void Start()
        {
            
        }


        public void Stop()
        {
            
        }


        public void CancelAll()
        {
            
        }
    }
}
