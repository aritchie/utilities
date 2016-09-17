using System;


namespace Acr.Core
{
    public class Weight
    {
        public Weight(double value, WeightUnit unit)
        {
            this.Value = value;
            this.Unit = unit;
        }


        public double Value { get; }
        public WeightUnit Unit { get; }


        public Weight ConvertTo(WeightUnit unit, int roundDecimals = 2)
        {
            return null;
        }
    }
}
