using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station.Tests
{
    [TestClass]
    public class StationTest
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
        public void IsActive()
        {
            Assert.IsTrue(_station.Status == Core.Enums.StationStatus.Running);
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
