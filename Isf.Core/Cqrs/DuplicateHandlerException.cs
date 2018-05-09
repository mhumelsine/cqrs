using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class DuplicateHandlerException : Exception
    {
        public DuplicateHandlerException(Type dto, Type existingHandler, Type duplicateHandler)
            :base($"A handler for '{dto.FullName}' already exists.  Existing handler '{existingHandler.FullName}', attempted to add '{duplicateHandler.FullName}'")
        {

        }
    }
}
