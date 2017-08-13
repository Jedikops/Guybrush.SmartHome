﻿using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Station.Core.Base;
using System;
using Windows.Devices.Gpio;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Light : Observable, ITurnOnOffModule
    {
        GpioController GPIO;
        GpioPin pin;

        public Light()
        {
            try
            {
                GPIO = GpioController.GetDefault();
                pin = GPIO.OpenPin(6);
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
            catch { }
        }
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private bool _status;
        public bool Status
        {
            get
            {
                return _status;
            }

            set
            {
                if (pin == null)
                {
                    try
                    {
                        GPIO = GpioController.GetDefault();
                        pin = GPIO.OpenPin(6);
                        pin.SetDriveMode(GpioPinDriveMode.Output);
                    }
                    catch { }
                }

                if (value == true)
                    pin.Write(GpioPinValue.Low);
                else
                    pin.Write(GpioPinValue.High);

                _status = value;
                ValueChanged?.Invoke(this, value);
                OnPropertyChanged();

            }
        }

        public event DeviceOnOffEventArgs ValueChanged;
    }
}
