using System;

namespace Bill.Mutant.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ModuleAttribute : Attribute
    {
        public int Order { get; }

        public ModuleAttribute(int order = 0)
        {
            Order = order;
        }
    }
}
