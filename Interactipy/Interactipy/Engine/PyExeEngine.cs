using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interactipy.Engine
{
    class PyExeEngine : PyEngine
    {
        public string InterpreterPath { get; set; }

        public override bool IsEnded { get { return mainproc.HasExited; } }

        public override event EventHandler WorkFinished;
        public override event EventHandler<ErrorOccuredEventArgs> ErrorOccured;

        private Process mainproc;
        private Thread informThread;


        public PyExeEngine(string interpreterPath)
        {
            InterpreterPath = interpreterPath;
        }

        public override void Start(string startupDirectory)
        {
            mainproc = new Process();

            ProcessStartInfo startinfo = new ProcessStartInfo(InterpreterPath, "-i -u -q");
            startinfo.CreateNoWindow = true;
            startinfo.RedirectStandardInput = true;
            startinfo.RedirectStandardOutput = true;
            startinfo.RedirectStandardError = true;
            startinfo.UseShellExecute = false;
            startinfo.WorkingDirectory = startupDirectory;

            mainproc.StartInfo = startinfo;

            

            mainproc.Start();

            OutputStream = mainproc.StandardOutput;
            InformationStream = mainproc.StandardError;

            informThread = new Thread(new ThreadStart(informThreadWorking));
            informThread.Start();
        }

        public override void Stop()
        {
            
            if (!mainproc.HasExited)
                mainproc.Kill();
        }

        public override bool Execute(string command)
        {
            isBusy = true;
            mainproc.StandardInput.WriteLine(command);

            return true;
        }

        private string informBlock = "";
        private int endCharCount = 0;
        private void informThreadWorking()
        {
            while (true)
            {
                char ch = (char)InformationStream.Read();
                

                if (ch == '>')
                {
                    endCharCount++;
                    if (endCharCount == 3)
                    {
                        if (!string.IsNullOrWhiteSpace(informBlock))
                        {
                            ErrorOccured?.Invoke(this, new ErrorOccuredEventArgs(informBlock));
                        }
                        WorkFinished?.Invoke(this, new EventArgs());
                        endCharCount = 0;
                        isBusy = false;
                        informBlock = "";
                    }
                    
                }
                else
                {
                    informBlock += ch;
                    endCharCount = 0;
                }

            }
        }
    }
}
