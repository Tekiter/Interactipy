using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactipy.Engine
{

    public class PyProcess
    {
        public static PyProcess Create(string pyexe, string args, string workingDirectory, IEnumerable<string> pythonPath)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = pyexe;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            proc.StartInfo.EnvironmentVariables["PYTHONPATH"] = PythonPathBuilder(pythonPath);
            proc.StartInfo.EnvironmentVariables["PYTHONIOENCODING"] = "UTF-8";
            proc.StartInfo.EnvironmentVariables["PYTHONUNBUFFERED"] = "1";

            PyProcess pyproc = new PyProcess();

            pyproc.RawProcess = proc;
            proc.Start();

            return pyproc;
        }

        private static string PythonPathBuilder(IEnumerable<string> pythonPath)
        {
            
            StringBuilder sb = new StringBuilder();

            bool isfirst = true;
            foreach (string i in pythonPath)
            {
                if (isfirst)
                {
                    isfirst = false;
                }
                else
                {
                    sb.Append(';');
                }
                sb.Append(i);
            }
            return sb.ToString();
            
        }
        public Process RawProcess { get; private set; }


        private PyProcess()
        {

        }
        

    }
}
