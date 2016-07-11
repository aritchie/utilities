using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Acr.Android.Rx
{
    class ConnectivityObservables
    {
    }
}
/*
        public IObservable<NetworkReachability> WhenStatusChanged()
        {
            return AndroidObservables
                .WhenIntentReceived(ConnectivityManager.ConnectivityAction)
                .Select(intent => this.InternetReachability);
        }
     */