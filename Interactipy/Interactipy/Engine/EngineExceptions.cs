﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactipy.Engine
{
    public class ExcutableNotRespondingException : Exception
    {
        public ExcutableNotRespondingException(string message) : base(message)
        {
        }
    }
}
