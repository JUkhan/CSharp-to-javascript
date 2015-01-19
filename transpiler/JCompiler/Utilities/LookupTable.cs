using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Ast;

namespace JCompiler.Utilities
{
    public class LookupTable
    {
        public string ClassName;
        public List<LookupVariable> VariableList = new List<LookupVariable>();
        public List<LookupMethod> MethodList = new List<LookupMethod>();

    }
    public class LookupMethod
    {
        public Function Method;    
        public List<LookupVariable> VariableList = new List<LookupVariable>();
    }
    public class LookupVariable
    {
        public string ScopeName;
        public string VariableName;
        public string VariableTpe;
        public string varNo;
    }
}
