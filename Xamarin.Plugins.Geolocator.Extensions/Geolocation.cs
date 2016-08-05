using System;
using GeoCoordinatePortable;


namespace Xamarin.Plugins.Geolocator.Extensions
{
    public class Geolocation
    {
        public Geolocation(double latitude, double longitude)
        {
            this.Coordinate = new GeoCoordinate(latitude, longitude);
        }


        public DateTime Timestamp { get; } = DateTime.Now;
        public GeoCoordinate Coordinate { get; }
    }
}
