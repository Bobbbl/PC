using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PC.Test
{
    public class CNCDeviceTester
    {
        [Fact]
        public async Task SendSoftReset()
        {
            var tmp = await device.SendSoftReset();

            Assert.DoesNotContain("error", tmp.Message);
        }

        [Fact]
        public async Task GetCurrentZTest()
        {
            var tmp = await device.GetCurrentZ();

            Assert.IsType(typeof(float), tmp);
        }

        [Fact]
        public async Task GetKillAlarmTest()
        {
            var tmp = await device.SendKillAlarm();

            Assert.Contains("ok", tmp.Message);

        }


        public CNC_Device device { get; set; }
        public CNCDeviceTester()
        {
            CNCInterface iface = new SerialGRBLInterface(GeneralTestInformations.PortName, GeneralTestInformations.BaudRate);
            CNCProtokoll potokoll = new GRBLProtokoll();
            device = new CNC_Device(iface, potokoll);
        }


    }
}
