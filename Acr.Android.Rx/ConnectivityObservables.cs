//using System;
//using Android.Net;


//namespace Acr.Android.Rx
//{
//    public static class ConnectivityObservables
//    {
//        public IObservable<NetworkReachability> WhenStatusChanged()
//        {
//            return AndroidObservables
//                .WhenIntentReceived(ConnectivityManager.ConnectivityAction)
//                .Select(intent => this.InternetReachability);
//        }
//    }
//}