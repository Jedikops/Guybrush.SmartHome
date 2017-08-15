using Guybrush.SmartHome.Modules.Standard;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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
            _station.RegisterTurnOnOffDevice("Guybrush Inc", "Light", "1", light.Id.ToString(), "Guybrush Light", light);
            Assert.IsTrue(_station.Devices.Any(x => x.Equals(light)));
        }

        [TestMethod]
        public void RegisterReading()
        {
            var lightSens = new LightSensor();
            _station.RegisterReadingDevice("Guybrush Inc", "Light Intensity", "1", lightSens.Id.ToString(), "Guybrush light intensity sensor", lightSens);

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
