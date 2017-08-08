using Guybrush.SmartHome.Station.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station.Tests
{
    [TestClass]
    public class ComponentTest
    {
        private Station _station;

        [TestInitialize]
        public void Initialize()
        {
            _station = new Station();
            var task = Task.Run(async () =>
            {

                await _station.Initialize();

            });
            task.Wait();
        }

        [TestMethod]
        public void RegisterDevice()
        {
            var light = new Light();
            _station.RegisterTurnOnOffDevice("Light", "Guybrush Inc", "Light", "1", Guid.NewGuid().ToString(), "Guybrush Light", light);
            Assert.IsTrue(_station.Devices.Any(x => x.Equals(light)));
        }

        [TestMethod]
        public void RegisterReading()
        {
            var lightSens = new LightSensor();
            _station.RegisterReadingDevice("Light Intensity", "Lux", "Light intensity reading", "Current light intensity value in Lux", lightSens);

            Assert.IsTrue(_station.Readers.Any(x => x.Equals(lightSens)));

        }

        [TestCleanup]
        public void CleanUp()
        {
            var task = Task.Run(async () =>
            {

                await _station.Shutdown();
            });
            task.Wait();
        }
    }
}
