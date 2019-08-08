using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Interactipy.Engine
{
    public class PyExecutable
    {
        public string Path { get; set; }


        private string versioncache = "";
        public string Version
        {
            get
            {
                if (versioncache != "")
                {
                    versioncache = GetVersionString();
                }
                return versioncache;
            }

        }

        public bool IsValid
        {
            get
            {
                string verstr;
                if (!File.Exists(Path))
                {
                    return false;
                }
                try
                {
                    verstr = GetVersionString();
                    
                    if (!verstr.StartsWith("Python"))
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
                versioncache = verstr;

                return true;
            }
        }


        public PyExecutable(string path)
        {
            Path = path;
        }

        

        public string GetVersionString(int timeout = 500)
        {
            Process proc = new Process();

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Path;
            info.Arguments = "-V";
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;

            proc.StartInfo = info;

            proc.Start();

            DateTime stamp = DateTime.Now;
            string result = "";
            while (true)
            {
                if (proc.StandardOutput.Peek() > -1)
                {
                    char ch = (char)proc.StandardOutput.Read();
                    if (ch == '\r' || ch == '\n') {
                        break;
                    }
                    result += ch;
                }
                if (DateTime.Now - stamp > new TimeSpan(0,0,0,timeout))
                {
                    throw new Exception("not responding");
                }
            }
            return result;
        }

        public Process Run(string args, string workingDirectory)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Path;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            proc.StartInfo.EnvironmentVariables["PYTHONPATH"] = System.IO.Path.GetDirectoryName(Path);
            proc.StartInfo.EnvironmentVariables["PYTHONIOENCODING"] = "UTF-8";
            proc.StartInfo.EnvironmentVariables["PYTHONUNBUFFERED"] = "1";
            
            proc.Start();

            return proc;
        }
    }
}
