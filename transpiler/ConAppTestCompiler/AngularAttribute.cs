using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConAppTestCompiler
{
    using System;
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class AngularAttribute : Attribute
    {
        public AngularAttribute()
        {
            
        }
        public string ModuleName { get; set; }
        public string DI { get; set; }
        public string ActionType { get; set; }
        public string ActionName { get; set; }
    }
}
