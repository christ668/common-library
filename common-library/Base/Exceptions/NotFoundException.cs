using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string name, object key) : base($"{name} ({key} was not found)")
        {

        }
    }
}
