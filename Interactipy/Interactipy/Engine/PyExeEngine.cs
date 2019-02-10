using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactipy.Engine
{
    class PyExeEngine : PyEngine
    {
        public string InterpreterPath { get; set; }

        

        private Process mainproc;

        public PyExeEngine(string interpreterPath)
        {
            InterpreterPath = interpreterPath;
        }

        public override void Start(string startupDirectory)
        {
            mainproc = new Process();

            ProcessStartInfo startinfo = new ProcessStartInfo(InterpreterPath, "-i -u");
            startinfo.CreateNoWindow = true;
            startinfo.RedirectStandardInput = true;
            startinfo.RedirectStandardOutput = true;
            startinfo.RedirectStandardError = true;
            startinfo.UseShellExecute = false;

            mainproc.StartInfo = startinfo;

            

            mainproc.Start();

            OutputStream = mainproc.StandardOutput;
            InformationStream = mainproc.StandardError;
        }

        public override void Stop()
        {
            if (!mainproc.HasExited)
                mainproc.Kill();
        }

        public override bool Execute(string command)
        {
            mainproc.StandardInput.WriteLine(command);

            return true;
        }
        
    }
}
