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
            CNCMessage output = protokoll.GetMoveByXMessage();

            // Assert

        }

        [Fact]
        public void GetMoveByYMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetMoveByYMessage();
        }

        [Fact]
        public void GetMoveByZMessage_IsEqual()
        {
            // Arrange
            GRBLProtokoll protokoll = new GRBLProtokoll();

            // Act
            CNCMessage output = protokoll.GetMoveByZMessage();
        }

        [Fact]
        public void GetJogByXMessage_IsEqual()
        {

        }

        [Fact]
        public void GetJogByYMessage_IsEqual()
        {

        }

        [Fact]
        public void GetJogByZMessage_IsEqual()
        {

        }
    }
}
