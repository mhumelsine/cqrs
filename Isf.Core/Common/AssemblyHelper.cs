using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isf.Core.Common
{
    public class AssemblyHelper
    {
        private readonly string[] assemblies;

        private IEnumerable<Type> allTypes;
        public IEnumerable<Type> AllTypes
        {
            get
            {
                if (allTypes == null)
                {
                    allTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => assemblies.Any(asm => x.FullName.StartsWith(asm)))
                        .SelectMany(assembly => assembly.GetTypes());
                }

                return allTypes;
            }
        }

        public IEnumerable<Type> AllConcreteTypes
        {
            get
            {
                return AllTypes.Where(x => !x.IsAbstract && !x.IsInterface);
            }
        }

        public AssemblyHelper(params string[] assembliesToScan)
        {
            assemblies = assembliesToScan;
        }

        public IEnumerable<Type> GetAllGenericInterfaceImplementations(Type genericInterfaceType)
        {
            return AllTypes
                .Where(x => x.IsClass)
                .Where(x => x.GetInterfaces()
                    .Where(iface => iface.IsGenericType)
                        .Any(iface => genericInterfaceType == iface.GetGenericTypeDefinition()));
        }

        public IEnumerable<Type> GetAllConcreteDerivedTypes(Type baseType)
        {
            return AllConcreteTypes
                .Where(type => baseType.IsAssignableFrom(type));
        }
    }
}
