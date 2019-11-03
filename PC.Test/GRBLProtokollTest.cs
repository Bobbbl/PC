using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PC.Test
{
    public class GRBLProtokollTest
    {

        [Fact]
        public void GetCurrentXYZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetCurrentXYZMessage();

            // Assert
            Assert.Equal("?", output.Message);
        }

        [Fact]
        public void GetCurrentFeedMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetCurrentFeedMessage();

            // Assert
            Assert.Equal("$G", output.Message);

        }

        [Fact]
        public void GetCurrentXMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetCurrentXMessage();

            // Assert
            Assert.Equal("?", output.Message);
        }

        [Fact]
        public void GetCurrentYMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetCurrentYMessage();

            // Assert
            Assert.Equal("?", output.Message);
        }

        [Fact]
        public void GetCurrentZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetCurrentZMessage();

            // Assert
            Assert.Equal("?", output.Message);
        }

        [Fact]
        public void GetMoveByXMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetMoveByXMessage(12, 100);

            // Assert
            Assert.Equal("X12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetMoveByYMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetMoveByYMessage(12, 100);

            // Assert
            Assert.Equal("Y12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetMoveByZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetMoveByZMessage(12, 100);

            // Assert
            Assert.Equal("Z12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetMoveByXYZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetMoveByXYZMessage(12, 12, 12, 100);

            // Assert
            Assert.Equal("X12 Y12 Z12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetJogByXMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetJogByXMessage(12, 100);

            // Assert
            Assert.Equal("$J=X12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetJogByYMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetJogByYMessage(12, 100);

            // Assert
            Assert.Equal("$J=Y12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetJogByZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetJogByZMessage(12, 100);

            // Assert
            Assert.Equal("$J=Z12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetJogByXYZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetJogByXYZMessage(12, 12, 12, 100);

            // Assert
            Assert.Equal("$J=X12 Y12 Z12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetRelativeJogByXMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetRelativeJogByXMessage(12, 100);

            // Assert
            Assert.Equal("$J=G21 G91 X12 F100", output.Message.Trim());
        }

        [Fact]
        public void GetRelativeJogByYMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetRelativeJogByYMessage(12, 100);

            // Assert
            Assert.Equal("$J=G21 G91 Y12 F100", output.Message.Trim());

        }

        [Fact]
        public void GetRelativeJogByZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetRelativeJogByZMessage(12, 100);

            // Assert
            Assert.Equal("$J=G21 G91 Z12 F100", output.Message.Trim());

        }

        [Fact]
        public void GetKillAlarmMessage_IsEqual()
        {
            GRBLProtokoll protokoll = new GRBLProtokoll();

            CNCMessage cNCMessage = protokoll.GetKillAlarmMessage();

            Assert.Equal("$X", cNCMessage.Message.Trim());
        }

        [Fact]
        public void GetSpindelSetRPMMessage_IsEqual()
        {
            GRBLProtokoll gRBLProtokoll = new GRBLProtokoll();

            CNCMessage cNCMessage = gRBLProtokoll.GetSpindelSetRPMMessage(1000, "clockwise");


            CNCMessage nNCMessagecounter = gRBLProtokoll.GetSpindelSetRPMMessage(1000, "counterclockwise");

            Assert.Equal("M3 S1000", cNCMessage.Message.Trim());
            Assert.Equal("M4 S1000", nNCMessagecounter.Message.Trim());


        }

        [Fact]
        public void GetSetZeroMessage_IsEqual()
        {
            GRBLProtokoll protokoll = new GRBLProtokoll();

            CNCMessage output = protokoll.GetSetZeroMessage();

            Assert.Equal("G10 P1 L20 X0 Y0 Z0", output.Message);
        }

        [Fact]
        public void GetHomingMessage_IsEqual()
        {
            GRBLProtokoll protokoll = new GRBLProtokoll();
            CNCMessage cNCMessage = protokoll.GetHomingMessage();

            Assert.Equal("$H", cNCMessage.Message);
        }

        [Fact]
        public void GetRelativeJogByXYZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetRelativeJogByXYZMessage(12, 12, 12, 100);

            // Assert
            Assert.Equal("$J=G21 G91 X12 Y12 Z12 F100", output.Message.Trim());

        }


    }
}
