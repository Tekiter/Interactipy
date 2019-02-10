using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactipy.Engine
{
    class ErrorOccuredEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }

        public ErrorOccuredEventArgs()
        {
            ErrorMessage = "";
        }
        public ErrorOccuredEventArgs(string errormsg)
        {
            ErrorMessage = errormsg;
        }

    }
}
