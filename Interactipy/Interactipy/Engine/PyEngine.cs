using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactipy.Engine
{
    abstract class PyEngine
    {

        public StreamReader OutputStream { get; set; }
        public StreamReader InformationStream { get; set; }
        


        public abstract bool Execute(string command);
        public abstract void Start(string startupDirectory);
        public abstract void Stop();

        

        
    }
}
