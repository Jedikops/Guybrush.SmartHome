using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guybrush.SmartHome.Client.Data.Managers
{
    public class ConditionManager
    {
        private IInterface conditionInterface;
        private string name;


        public void Configure(IInterface conditionInterface, string name)
        {
            this.conditionInterface = conditionInterface;
            this.name = name;
        }

        public async void GetConditions()
        {

            var method = conditionInterface.Methods.First(x => x.Name == "GetConditions");
            IList<ParameterInfo> outsig = (IList<ParameterInfo>)method.OutSignature;

            InvokeMethodResult result = await method.InvokeAsync(new List<object>());
            var values = result.Values;
            foreach (var obj in values)
            {

            }

        }
    }
    public class IntArrayOverBytes
    {
        private readonly byte[] bytes;
        public IntArrayOverBytes(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public int this[int index]
        {
            get { return BitConverter.ToInt32(bytes, index * 4); }
            set { Array.Copy(BitConverter.GetBytes(value), 0, bytes, index * 4, 4); }
        }
    }
}
