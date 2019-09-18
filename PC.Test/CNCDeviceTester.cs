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
        public async Task JogRelativeTest()
        {
            var tmp1 = await device.RelativeJogX(10, 100);
            var tmp2 = await device.RelativeJogY(10, 100);
            var tmp3 = await device.RelativeJogZ(10, 100);
            var tmp4 = await device.RelativeJogXYZ(10, 10, 10, 100);

            Assert.DoesNotContain("error", tmp1.Message);
            Assert.DoesNotContain("error", tmp2.Message);
            Assert.DoesNotContain("error", tmp3.Message);
            Assert.DoesNotContain("error", tmp4.Message);

            Assert.Contains("ok", tmp1.Message);
            Assert.Contains("ok", tmp2.Message);
            Assert.Contains("ok", tmp3.Message);
            Assert.Contains("ok", tmp4.Message);
        }

        [Fact]
        public async Task MoveByTest()
        {
            var tmp1 = await device.MoveByX(10, 100);
            var tmp2 = await device.MoveByY(10, 100);
            var tmp3 = await device.MoveByZ(10, 100);
            var tmp4 = await device.MoveByXYZ(10, 10, 10, 100);

            Assert.DoesNotContain("error", tmp1.Message);
            Assert.DoesNotContain("error", tmp2.Message);
            Assert.DoesNotContain("error", tmp3.Message);
            Assert.DoesNotContain("error", tmp4.Message);

            Assert.Contains("ok", tmp1.Message);
            Assert.Contains("ok", tmp2.Message);
            Assert.Contains("ok", tmp3.Message);
            Assert.Contains("ok", tmp4.Message);

        }

        [Fact]
        public async Task JogXTest()
        {
            var tmp = await device.JogX(10, 100);

            Assert.DoesNotContain("error", tmp.Message);
            Assert.Contains("ok", tmp.Message);
        }

        [Fact]
        public async Task JogYTest()
        {

            var tmp = await device.JogY(10, 100);

            Assert.DoesNotContain("error", tmp.Message);
            Assert.Contains("ok", tmp.Message);
        }

        [Fact]
        public async Task JogZTest()
        {
            var tmp = await device.JogZ(10, 100);

            Assert.DoesNotContain("error", tmp.Message);
            Assert.Contains("ok", tmp.Message);
        }


        [Fact]
        public async Task GetCurrentXYZTest()
        {
            var xyz = await device.GetCurrentXYZ();

            Assert.IsType(typeof(float), xyz[0]);
            Assert.IsType(typeof(float), xyz[1]);
            Assert.IsType(typeof(float), xyz[2]);
            Assert.Equal(3, xyz.Length);

        }



        [Fact]
        public async Task GetCurrentFeedTest()
        {
            var tmp = await device.GetCurrentFeed();

            Assert.IsType(typeof(float), tmp);
        }

        [Fact]
        public async Task GetCurrentXTest()
        {
            var tmp = await device.GetCurrentX();

            Assert.IsType(typeof(float), tmp);
            Assert.Equal(tmp, tmp);
        }

        [Fact]
        public async Task GetCurrentYTest()
        {
            var tmp = await device.GetCurrentY();

            Assert.IsType(typeof(float), tmp);
        }

        [Fact]
        public async Task GetCurrentZTest()
        {
            var tmp = await device.GetCurrentZ();

            Assert.IsType(typeof(float), tmp);
        }

        [Fact]
        public async Task SendSoftResetTest()
        {
            var tmp = await device.SendSoftReset();

            Assert.DoesNotContain("error", tmp.Message);
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
