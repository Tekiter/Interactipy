using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Interactipy.Engine
{
    public class EnvironmentSelector
    {
        public List<string> SearchPath { get; } = new List<string>()
        {
            Path.GetPathRoot(Environment.SystemDirectory),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs", @"Python\"),
            
        };

        public List<PyExecutable> Environments { get; private set; }

        public EnvironmentSelector()
        {
            Environments = new List<PyExecutable>();
        }


        public void Refresh()
        {
            foreach (var path in SearchPath)
            {
                if (!Directory.Exists(path))
                {
                    continue;
                }

                if (File.Exists(Path.Combine(path, "python.exe")))
                {
                    AddEnvironment(path);

                }
                else
                {
                    foreach (var npath in Directory.GetDirectories(path, "?ython*"))
                    {
                        if (File.Exists(Path.Combine(npath, "python.exe")))
                        {
                            AddEnvironment(Path.Combine(npath, "python.exe"));
                        }
                    }
                }
                
            }
        }

        public bool AddEnvironment(string pypath)
        {
            PyExecutable exe = new PyExecutable(pypath);
            if (exe.IsValid)
            {
                Environments.Add(exe);
                return true;
            }
            return false;
        }
    }
}
