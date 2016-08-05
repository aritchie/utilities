using System;
using System.Reactive.Linq;
using Acr.Utilities;
using GeoCoordinatePortable;
using Plugin.Geolocator.Abstractions;


namespace Xamarin.Plugins.Geolocator.Extensions
{
    public static class Extensions
    {
        public static IObservable<Distance> DerivedSpeed(this IGeolocator locator)
        {
            return Observable.Create<Distance>(ob =>
            {
                ob.OnNext(Distance.Empty);
                GeoCoordinate lastPosition = null;

                Observable
                    .Interval(TimeSpan.FromSeconds(1))
                    .Subscribe(x =>
                    {
                        // broadcast zero if new coordinate hasn't come in?
                        //if (lastPosition == null)
                    });
                var changed = Observable
                    .FromEventPattern<PositionEventArgs>(locator, "PositionChanged")
                    .Throttle(TimeSpan.FromSeconds(1))
                    .Select(x => x.EventArgs.Position)
                    .Subscribe(x =>
                    {
                        var current = new GeoCoordinate(x.Latitude, x.Longitude);
                        if (lastPosition == null)
                        {
                            lastPosition = current;
                            return;
                        }
                        var distKm = Convert.ToDouble(lastPosition.GetDistanceTo(current));
                        var dist = Distance.FromKilometers(distKm); // distance over the last second
                        lastPosition = current;

                        ob.OnNext(dist);
                    });

                locator.DesiredAccuracy = 200;
                if (!locator.IsListening)
                    locator.StartListeningAsync(1000, 200, false);

                return changed;
            });
        }


        public static IObservable<Distance> DerivedOdometer(this IGeolocator locator, int startingOdo = 0)
        {
            return Observable.Create<Distance>(ob =>
            {
                locator.DesiredAccuracy = 200;
                if (!locator.IsListening)
                    locator.StartListeningAsync(1000, 200, false);

                return () => { };
            });
        }
    }
}
