﻿using System;
using System.Linq;
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
                Distance currentSpeed = null;

                // need to be calculating speed roughly every second, if
                // values aren't coming in, assume zero
                var timer = Observable
                    .Interval(TimeSpan.FromSeconds(2))
                    .Subscribe(x =>
                    {
                        if (currentSpeed != null)
                        {
                            ob.OnNext(currentSpeed);
                            currentSpeed = null;
                            return;
                        }
                        ob.OnNext(Distance.Empty);
                    });

                var gps = Observable
                    .FromEventPattern<PositionEventArgs>(locator, "PositionChanged")
                    .Select(x => new Geolocation(x.EventArgs.Position.Latitude, x.EventArgs.Position.Longitude))
                    .Buffer(2)
                    .Subscribe(coords =>
                    {
                        var first = coords.First();
                        var second = coords.Last();
                        var ts = second.Timestamp.Subtract(first.Timestamp);
                        var distKm = first.Coordinate.GetDistanceTo(second.Coordinate);
                        var speedKm = distKm / ts.TotalSeconds;
                        currentSpeed = Distance.FromKilometers(speedKm);
                    });

                locator.DesiredAccuracy = 200;
                if (!locator.IsListening)
                    locator.StartListeningAsync(1000, 200, false);

                return () =>
                {
                    timer.Dispose();
                    gps.Dispose();
                };
            });
        }


        public static IObservable<Distance> DerivedOdometer(this IGeolocator locator, Distance startingOdo = null)
        {
            return Observable.Create<Distance>(ob =>
            {
                var odometerKm = startingOdo.TotalKilometers;
                GeoCoordinate startTrip = null;

                var gps = Observable
                    .FromEventPattern<PositionEventArgs>(locator, "PositionChanged")
                    .Select(x => new GeoCoordinate(x.EventArgs.Position.Latitude, x.EventArgs.Position.Longitude))
                    .Subscribe(coords =>
                    {
                        if (startTrip == null)
                            startTrip = coords;
                        else
                        {
                            var distKm = startTrip.GetDistanceTo(coords);
                            var currentOdoKm = odometerKm + distKm; // don't accumulate
                            var dist = Distance.FromKilometers(currentOdoKm);
                            ob.OnNext(dist);
                        }
                    });

                locator.DesiredAccuracy = 200;
                if (!locator.IsListening)
                    locator.StartListeningAsync(1000, 200, false);

                return gps;
            });
        }
    }
}
