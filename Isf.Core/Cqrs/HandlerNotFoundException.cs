using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(Type dto)
            : base($"A handler for '{dto.FullName}' could not be found")
        {

        }
    }
}
