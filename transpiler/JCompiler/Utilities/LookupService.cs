using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Ast;
namespace JCompiler.Utilities
{
    public interface LookupService
    {
        void AddClass(string className, int lineNo);
        void AddMethod(string className,Function function, int lineNo);
        void AddClassVariable(string className, DeclareVar declareVar, int lineNo);
        void AddMethodVariable(string className, Function functionName, DeclareVar declareVar, int lineNo);
        void ChequeAssignment(string className, string functionName, string var, int lineNo);
        string GetFunctionReturnType(string className, string functionName, int lineno);
        string GetFunctionParamJVMType(string className, string functionName);
    }
}
