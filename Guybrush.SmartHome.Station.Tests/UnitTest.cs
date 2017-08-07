using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Station.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private Station _station;

        [TestInitialize]
        public void Initialize()
        {
            _station = new Station();
            var task = Task.Run(async () =>
            {

                await _station.Initialize();
                _station.Start();
            });
            task.Wait();


        }
        [TestMethod]
        public void IsActive()
        {
            Assert.IsTrue(_station.Status == Core.Enums.StationStatus.Active);
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
