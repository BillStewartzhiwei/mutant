using System;
using System.Collections.Generic;

namespace Bill.Mutant.Core
{
    internal class ModuleInfo
    {
        public Type ModuleType { get; }
        public int Order { get; }
        public List<Type> Dependencies { get; }

        public ModuleInfo(Type type, int order, List<Type> deps)
        {
            ModuleType = type;
            Order = order;
            Dependencies = deps ?? new List<Type>();
        }
    }
}
