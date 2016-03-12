using System;
using System.Collections.Generic;


namespace Acr.Utilities
{
    public class ActionQueue
    {
        readonly Queue<Action> actions = new Queue<Action>(); 
    }
}
