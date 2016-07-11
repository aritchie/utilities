﻿using System;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using Foundation;
using UIKit;


namespace Acr.iOS.Rx
{
    public static class UIAppObservables
    {

        public static IObservable<object> WhenEnteringForeground(this UIApplication app)
        {
            return Observable.Create<object>(ob =>
                UIApplication
                    .Notifications
                    .ObserveWillEnterForeground((sender, args) => ob.OnNext(null))
            );
        }


        public static IObservable<object> WhenEnteringBackground()
        {
            return Observable.Create<object>(ob =>
                 UIApplication
                    .Notifications
                    .ObserveDidEnterBackground((sender, args) => ob.OnNext(null))
            );
        }


        public static IObservable<CultureInfo> WhenCultureChanged(this UIApplication app)
        {
            return Observable.Create<CultureInfo>(ob =>
                NSLocale
                    .Notifications
                    .ObserveCurrentLocaleDidChange((sender, args) =>
                    {
                        var culture = GetSystemCultureInfo();
                        ob.OnNext(culture);
                    })
            );
        }


        // taken from https://developer.xamarin.com/guides/cross-platform/xamarin-forms/localization/ with modifications
        public static CultureInfo GetSystemCultureInfo()
        {
            try
            {
                var netLang = "en";
                var prefLang = "en";
                if (NSLocale.PreferredLanguages.Any())
                {
                    var pref = NSLocale.PreferredLanguages
                        .First()
                        .Substring(0, 2)
                        .ToLower();

                    if (prefLang == "pt")
                        pref = pref == "pt" ? "pt-BR" : "pt-PT";

                    netLang = pref.Replace("_", "0");
                    Console.WriteLine($"Preferred Language: {netLang}");
                }
                CultureInfo value;
                try
                {
                    Console.WriteLine($"Setting locale to {netLang}");
                    value = new CultureInfo(netLang);
                }
                catch
                {
                    Console.WriteLine($"Failed setting locale - moving to preferred langugage {prefLang}");
                    value = new CultureInfo(prefLang);
                }
                return value;
            }
            catch
            {
                return CultureInfo.CurrentUICulture;
            }
        }
    }
}
/*










     */