﻿using Guybrush.SmartHome.Modules.Delegates;
using Guybrush.SmartHome.Modules.Interfaces;
using Guybrush.SmartHome.Station.Core.Base;
using System;

namespace Guybrush.SmartHome.Modules.Standard
{
    public class Blinds : Observable, ITurnOnOffModule
    {
        private bool _status;

        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool Status
        {
            get
            {
                return _status;
            }

            set
            {

                _status = value;
                if (ValueChanged != null)
                    ValueChanged(this, value);
                OnPropertyChanged();
            }
        }

        public event DeviceOnOffEventArgs ValueChanged;
    }
}
