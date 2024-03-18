using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base.Exceptions
{
    public class NoContentException : System.Exception
    {
        public NoContentException(string? message) : base(message)
        {
        }
    }
}
