using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using UIKit;

namespace Acr.iOS.Rx
{
    public static class UIDeviceObservables
    {

        public static IObservable<int> WhenBatteryPercentageChanged()
        {
            return Observable.Create<int>(ob =>
                UIDevice
                    .Notifications
                    .ObserveBatteryLevelDidChange((sender, args) =>
                    {
                        var percent = (int) (UIDevice.CurrentDevice.BatteryLevel*100F);
                        ob.OnNext(percent);
                    })
            );
        }


        public static IObservable<UIDeviceBatteryState> WhenBatteryStateChanged()
        {
            return Observable.Create<UIDeviceBatteryState>(ob =>
                UIDevice
                    .Notifications
                    .ObserveBatteryStateDidChange((sender, args) => ob.OnNext(UIDevice.CurrentDevice.BatteryState))
            );
        }
    }
}