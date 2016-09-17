using System;


namespace Acr.Core
{
    public class Distance
    {
        public Distance(double value, DistancetUnit unit)
        {
            this.Value = value;
            this.Unit = unit;
        }


        public double Value { get; }
        public DistancetUnit Unit { get; }


public static Distance(double value, DistanceUnit from, DistanceUnit to) {}
    }
}
