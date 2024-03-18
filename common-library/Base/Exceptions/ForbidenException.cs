﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string? message) : base(message)
        {
        }
    }
}
