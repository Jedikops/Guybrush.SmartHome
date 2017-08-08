namespace Guybrush.SmartHome.Modules.Delegates
{
    public delegate void ReaderValueEventArgs(object sender, int value);
    public delegate void ReaderUnitEventArgs(object sender, string value);

    public delegate void DeviceOnOffEventArgs(object sender, bool value);
    public delegate void DisplayEventArgs(object sender, string text);
}
