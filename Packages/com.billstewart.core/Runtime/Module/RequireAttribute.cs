using System;

namespace Bill.Mutant.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireAttribute : Attribute
    {
        public Type ModuleType { get; }

        public RequireAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }
    }
}
