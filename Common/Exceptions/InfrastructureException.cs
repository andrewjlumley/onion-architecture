using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public sealed class InfrastructureException : Exception
    {
        public InfrastructureException(string message, Exception? innerMessage)
            : base(message, innerMessage)
        {
        }
    }
}
