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

        public GRBLProtokollTest()
        {

        }
    }
}
