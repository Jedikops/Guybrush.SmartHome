namespace Guybrush.SmartHome.Modules.Delegates
{
    public delegate void ReaderEventArgs(object sender, int value);
    public delegate void DeviceOnOffEventArgs(object sender, bool value);
    public delegate void DisplayEventArgs(object sender, string text);
}
