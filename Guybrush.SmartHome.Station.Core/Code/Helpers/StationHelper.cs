using System;

namespace Guybrush.SmartHome.Station.Core.Helpers
{
    internal static class StationHelper
    {
        internal static Guid GetDeviceID()
        {
            if (!Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("DSBDeviceId"))
            {
                Guid deviceId = Guid.NewGuid();
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["DSBDeviceId"] = deviceId;
                return deviceId;
            }
            return (Guid)Windows.Storage.ApplicationData.Current.LocalSettings.Values["DSBDeviceId"];
        }
    }
}
