using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;


namespace Acr.Utilities
{
    public static class NpcExtensions
    {
         public static IObservable<TRet> WhenAnyValue<TSender, TRet>(this TSender This, Expression<Func<TSender, TRet>> expression) where TSender : INotifyPropertyChanged
        {
            var p = This.GetPropertyInfo(expression);
            return Observable
                .FromEventPattern<PropertyChangedEventArgs>(This, "PropertyChanged")
                .Where(x => x.EventArgs.PropertyName == p.Name)
                .Select(x =>
                {
                    var value = (TRet)p.GetValue(This);
                    return value;
                });
        }


        public static void ApplyMaxLengthConstraint<T>(this T npc, Expression<Func<T, string>> expression, int maxLength) where T : INotifyPropertyChanged
        {
            var property = npc.GetPropertyInfo(expression);

            if (property.PropertyType != typeof(string))
                throw new ArgumentException($"You can only use maxlength constraints on string based properties - {npc.GetType()}.{property.Name}");

            if (!property.CanWrite)
                throw new ArgumentException($"You can only apply maxlength constraints to public setter properties - {npc.GetType()}.{property.Name}");

            npc
                .WhenAnyValue(expression)
                .Where(x => x != null && x.Length > maxLength)
                .Subscribe(x =>
                {
                    var value = x.Substring(0, maxLength);
                    property.SetValue(npc, value);
                });
        }



    }
}
