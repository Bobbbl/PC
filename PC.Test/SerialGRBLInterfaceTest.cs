using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PC.Test
{
    public class SerialGRBLInterfaceTest
    {
        public SerialGRBLInterface GRBLInterface { get; set; }


        [Fact]
        public void SendMessage_GotAnythingBack()
        {
            // Assamble
            GRBLProtokoll pr = new GRBLProtokoll();
            CNCMessage m = pr.GetCurrentFeedMessage();

            // Act
            GRBLInterface.SendMessage(m);


            // Assert
            // No Exception is proof enough

        }

        [Fact]
        public void SendMessage_ReceiveMessage()
        {
            // Assamble
            GRBLProtokoll pr = new GRBLProtokoll();
            CNCMessage m = pr.GetCurrentFeedMessage();
            CNCMessage start = new CNCMessage() { Message = "Grbl 1.1g ['$' for help]" };
            var an = GRBLInterface.ReceiveMessage(100, start, 2000);

            // Act
            GRBLInterface.SendMessage(m);
            //CNCMessage answer = GRBLInterface.ReceiveMessage(10);
            CNCMessage tmp = new CNCMessage() { Message = "F" };
            CNCMessage answer = GRBLInterface.ReceiveMessage(100, tmp, 3000);


            // Assert
            Assert.Contains("F", answer.Message);
        }

        public SerialGRBLInterfaceTest()
        {
            GRBLInterface = new SerialGRBLInterface(GeneralInformations.PortName, GeneralInformations.BaudRate);
        }
    }
}
