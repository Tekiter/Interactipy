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

        

        public string GetVersionString(int timeout = 200)
        {
            Process proc = new Process();

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Path;
            info.Arguments = @"Python\vercheck.py";
            info.WorkingDirectory = Environment.CurrentDirectory;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

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
                if (DateTime.Now - stamp > new TimeSpan(0,0,0,0,timeout))
                {
                    throw new ExcutableNotRespondingException("not responding");
                }
            }
            return result;
        }

        public PyProcess Run(string args, string workingDirectory)
        {
            List<string> pythonPathes = new List<string>()
            {
                System.IO.Path.GetDirectoryName(Path)
            };

            PyProcess proc = PyProcess.Create(Path, args, workingDirectory, pythonPathes);

            return proc;
        }
    }
}
