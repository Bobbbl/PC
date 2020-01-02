using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PC.Test
{
    public class PythonTester
    {
        [Fact]
        public void TestGeneralPythonScript()
        {
            string StartPythonExe = @"C:\Users\draxinger\AppData\Local\Programs\Python\Python36\python.exe";
            string StartScriptPath = @"..\..\TestGeneralPythonScript.py";

            Assert.True(File.Exists(StartScriptPath));
        
            string output = string.Empty;

            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(StartPythonExe)
                {
                    Arguments = StartScriptPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                process.Start();
                output = process.StandardOutput.ReadToEnd();
            }


        }

        [Fact]
        public void TestArguments()
        {
            string StartPythonExe = @"C:\Users\draxinger\AppData\Local\Programs\Python\Python36\python.exe";
            string StartScriptPath = @"..\..\TestArguments.py";
            string ScriptArguments = " Argument1 Argument2 Argument3";

            Assert.True(File.Exists(StartScriptPath));

            string[] output = null;

            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(StartPythonExe)
                {
                    Arguments = StartScriptPath + ScriptArguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                process.Start();
                output = process.StandardOutput.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }

            Assert.Contains("4", output[1]);
            Assert.Equal(6, output.Length);
            Assert.Contains(@"..\..\TestArguments.py", output[2]);
            Assert.Contains("Argument1", output[3]);
            Assert.Contains("Argument2", output[4]);
            Assert.Contains("Argument3", output[5]);
        }
    }
}
