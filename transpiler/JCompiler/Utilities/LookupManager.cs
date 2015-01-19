using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Ast;

namespace JCompiler.Utilities
{
    public class LookupManager:LookupService
    {
        private Dictionary<string, LookupTable> lookupDictionary = new Dictionary<string, LookupTable>();
       
        string Qformat(string msg, params object[] args) { return String.Format(msg, args); }
        
        void LookupService.AddClass(string className, int lineNo)
        {
          
            if (lookupDictionary.ContainsKey(className))
            {
                throw new Exception(Qformat("class name '{0}' already defined. Line number {1}",className, lineNo));
            }
            lookupDictionary.Add(className, new LookupTable { ClassName = className });
        }

        void LookupService.AddMethod(string className, Ast.Function function, int lineNo)
        {
            LookupTable lt = lookupDictionary[className];

            var fx = (from u in lt.MethodList where u.Method.FunctionName == function.FunctionName select u).SingleOrDefault();
            if (fx != null)
            {
                foreach (var item in lt.MethodList)
                {
                    if (item.Method.ParamList.Count == function.ParamList.Count && item.Method.FunctionName==function.FunctionName)
                    {
                        if (hasAnyOverload(item.Method.ParamList, function.ParamList))
                        {
                            throw new Exception(Qformat("Function name '{0}' already defined.  Line number {1}", function.FunctionName, lineNo));
                        }
                    }
                }
            }
            lt.MethodList.Add(new LookupMethod { Method=function });
        }

        void LookupService.AddClassVariable(string className, Ast.DeclareVar declareVar, int lineNo)
        {
          
            LookupTable lt = lookupDictionary[className];
            var varcheque = (from u in lt.VariableList where u.VariableName == declareVar.Ident select u).SingleOrDefault();
            if (varcheque != null)
            {
                throw new Exception(Qformat("Already contains a defination for '{0}'.  Line number {1}", declareVar.Ident, lineNo));
            }
            lt.VariableList.Add(new LookupVariable { VariableName = declareVar.Ident, VariableTpe = declareVar.TypeName, ScopeName="class" });
        }
       
        void LookupService.AddMethodVariable(string className, Function func, Ast.DeclareVar declareVar, int lineNo)
        {
            LookupTable lt = lookupDictionary[className];
            var findInClass = (from u in lt.VariableList where u.VariableName == declareVar.Ident select u).SingleOrDefault();
            if (findInClass != null)
            {
                throw new Exception(Qformat("Class data member named '{0}' is already defined '.  Line number {1}", declareVar.Ident,lineNo));
            }
            var findInParamList=(from u in func.ParamList where u.ParamName==declareVar.Ident select u).SingleOrDefault();
            if (findInParamList != null)
            {
                throw new Exception(Qformat("A local variable named '{0}' cannot be declared in this scope because it would give a different meaning to '{0}',\n which is already used in 'paramiter list' scope to denote something else. Line number {1}", declareVar.Ident, lineNo));
            }
            var lookupMethod = (from u in lt.MethodList where u.Method.FunctionName == func.FunctionName select u).SingleOrDefault();
            if (lookupMethod != null)
            {
                var varcheque=(from u in lookupMethod.VariableList where u.VariableName==declareVar.Ident select u).SingleOrDefault();
                if(varcheque!=null)
                {
                    throw new Exception(Qformat("A local variable named '{0}' cannot be declared in this scope because it would give a different meaning to '{0}',\n which is already used in 'parent or current' scope to denote something else. Line number {1}", declareVar.Ident, lineNo));
                }
            }
            else
            {
                throw new Exception(Qformat("Unknown fatal error. variable declaration in method named '{0}' and class named '{1}'.  Line number {2}", func.FunctionName, className, lineNo));
            }
            lookupMethod.VariableList.Add(new LookupVariable { VariableName = declareVar.Ident, VariableTpe = declareVar.TypeName, ScopeName="method" });
        }

        void LookupService.ChequeAssignment(string className, string functionName, string var, int lineNo)
        {
            throw new NotImplementedException();
        }
        #region Helper
        bool hasAnyOverload(List<FunctionParam> p1, List<FunctionParam> p2)
        {            
            string p1Str = "";
            foreach (var item in p1)
            {
                p1Str += item.ParamType;
            }
            string p2Str = "";
            foreach (var item in p2)
            {
                p2Str += item.ParamType;
            }

            return p1Str == p2Str;
        }
        #endregion

        #region LookupService Members

        public string GetFunctionParamJVMType(string className, string functionName)
        {
            StringBuilder res = new StringBuilder();
            LookupTable lt = lookupDictionary[className];
            var method = (from u in lt.MethodList where u.Method.FunctionName == functionName select u).SingleOrDefault();
            if (method == null)
            {
                throw new Exception(Qformat("Undefined function name '{0}'", functionName));
            }
            foreach (var item in method.Method.ParamList)
            {
                res.Append(getJVMType(item.ParamType));
            }
            return res.ToString();
        }
        private string getJVMType(string type)
        {
            switch (type)
            {
                case "void": return "V";
                case "int": return "I";
                case "float": return "F";
                case "double": return "D";
                case "string": return "Ljava/lang/String;";
                default:
                    throw new Exception("Invalid type declaration.");

            }

        }
        public string GetFunctionReturnType(string className, string functionName, int lineno)
        {
            LookupTable lt = lookupDictionary[className];
            var method = (from u in lt.MethodList where u.Method.FunctionName == functionName select u).SingleOrDefault();
            if (method == null)
            {
                throw new Exception(Qformat("Undefined function name '{0}' at Lineno {1}", functionName, lineno));
            }
            return method.Method.ReturnType;
        }

        #endregion
    }
}
