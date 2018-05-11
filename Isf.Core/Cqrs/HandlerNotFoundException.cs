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

    public class DomainObjectEventHandlerNotFoundException : Exception
    {
        public DomainObjectEventHandlerNotFoundException(string methodName, Type domainObjectType)
            : base($"Expected to find the a method '{methodName}' on the type '{domainObjectType.FullName}'")
        {

        }
    }
}
