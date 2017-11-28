using System;

namespace DebugMod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BindableMethod : Attribute
    {
        public string name;
        public string category;
    }
}
