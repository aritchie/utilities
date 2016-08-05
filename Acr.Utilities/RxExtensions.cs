//using System;
//using System.Collections.Generic;
//using System.Reactive.Linq;


//namespace Acr.Utilities
//{
//    public static class RxExtensions
//    {
//        public static IObservable<IList<T>> SlidingBuffer<T>(this IObservable<T> obs, TimeSpan span, int max)
//        {
//            return Observable.Create<IList<T>>(ob =>
//            {
//                var acc = new List<T>();
//                return obs
//                    .Buffer(span)
//                    .Subscribe(next =>
//                    {
//                        if (next.Count == 0) //no activity in time span
//                        {
//                            ob.OnNext(acc);
//                            acc.Clear();
//                        }
//                        else
//                        {
//                            acc.AddRange(next);
//                            if (acc.Count >= max) //max items collected
//                            {
//                                ob.OnNext(acc);
//                                acc.Clear();
//                            }
//                        }
//                    },
//                    ob.OnError,
//                    () =>
//                    {
//                        ob.OnNext(acc);
//                        ob.OnCompleted();
//                    }
//                );
//            });
//        }
//    }
//}
