using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactipy.Engine
{
    public class PyExecutable
    {
        public string Path { get; set; }


        public PyExecutable(string path)
        {
            Path = path;
        }
    }
}
