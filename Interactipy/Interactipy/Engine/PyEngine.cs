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


        protected bool isBusy = false;
        public bool IsBusy { get { return isBusy; } }

        

        public abstract event EventHandler<ErrorOccuredEventArgs> ErrorOccured;
        public abstract event EventHandler WorkFinished;


        public abstract bool IsEnded { get; }


        public abstract bool Execute(string command);
        public abstract void Start(string startupDirectory);
        public abstract void Stop();

        

        
    }
}
