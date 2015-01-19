using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JCompiler.Utilities
{
    public class JSLookupTable
    {
        public string ClassScopeID { get; set; }
        private Dictionary<string, Dictionary<string, LookupVariable>> scopeVariable = new Dictionary<string, Dictionary<string, LookupVariable>>();
        public void AddVariable(string scopeid, string varName, string varType)
        {
            if (varName == "scope") varType = "$scope";
            else if (varName == "rootScope") varType = "$rootScope";

            LookupVariable var = new LookupVariable();
            var.VariableName = varName;
            var.VariableTpe = varType;
            if (scopeVariable.ContainsKey(scopeid))
            {
                var temp = scopeVariable[scopeid];
                if (temp.ContainsKey(varName))
                {
                    throw new Exception(String.Format("'{0}' variable is invalid", varName));
                }
                else
                {
                    temp[varName] = var;
                }
            }
            else
            {
                var temp = new Dictionary<string, LookupVariable>();
                temp[varName] = var;
                scopeVariable[scopeid] = temp;
            }
        }
        public string GetVarType(string scopeid, string varName)
        {
            try
            {
                if (!scopeVariable.ContainsKey(scopeid))
                {
                    if (!scopeVariable.ContainsKey(ClassScopeID))
                    {
                        return "";
                    }
                    scopeid = ClassScopeID;
                }
                var temp = scopeVariable[scopeid];
                if (temp.ContainsKey(varName))
                    return temp[varName].VariableTpe;
                else
                {
                    temp = scopeVariable[ClassScopeID];
                    if (temp.ContainsKey(varName))
                        return temp[varName].VariableTpe;
                }
            }
            catch (Exception)
            {

                return "";
            }
            return "";
        }
        private Dictionary<string, List<MethodName>> Methods = new Dictionary<string, List<MethodName>>();
        public void SetMethods(Dictionary<string, List<MethodName>> methods)
        {
            this.Methods = methods;
        }
        private void SetDefaultMethods()
        {
            List<MethodName> list = new List<MethodName> { 
                new MethodName{ CSMethodName="Add", JSMethodName="push"},
                  new MethodName{CSMethodName="Insert", JSMethodName="Insert"},
                new MethodName{CSMethodName="ForEach", JSMethodName="ForEach"},
                 new MethodName{CSMethodName="remove", JSMethodName="remove"},
                  new MethodName{CSMethodName="Where", JSMethodName="Where"},
                   new MethodName{CSMethodName="Find", JSMethodName="Find"},
                    new MethodName{CSMethodName="First", JSMethodName="First"},
                     new MethodName{CSMethodName="Last", JSMethodName="Last"},
                      new MethodName{CSMethodName="FindLast", JSMethodName="FindLast"},
                    new MethodName{CSMethodName="select", JSMethodName="select"},
                     new MethodName{CSMethodName="Single", JSMethodName="Single"},
                      new MethodName{CSMethodName="selectWithJoin", JSMethodName="selectWithJoin"},
                       new MethodName{CSMethodName="groupBy", JSMethodName="groupBy"},
                       new MethodName{CSMethodName="paging", JSMethodName="paging"},
                        new MethodName{CSMethodName="Join", JSMethodName="join"},
                         new MethodName{CSMethodName="join", JSMethodName="join"},
                        new MethodName{CSMethodName="push", JSMethodName="push"},
                        new MethodName{CSMethodName="pop", JSMethodName="pop"},
                        new MethodName{CSMethodName="slice", JSMethodName="slice"},
                        new MethodName{CSMethodName="sort", JSMethodName="sort"},
                        new MethodName{CSMethodName="shift", JSMethodName="shift"},
                        new MethodName{CSMethodName="unshift", JSMethodName="unshift"},
                        new MethodName{CSMethodName="reverse", JSMethodName="reverse"},
                        new MethodName{CSMethodName="getCount", JSMethodName="getCount"}
            };

            Methods["List"] = list;
            //Methods["$scope"] = new List<MethodName> { 
            //    new MethodName{CSMethodName="apply", JSMethodName="$apply" },
            //    new MethodName{CSMethodName="watch", JSMethodName="$watch" },
            //    new MethodName{CSMethodName="on", JSMethodName="$on" },
            //    new MethodName{CSMethodName="broadcast", JSMethodName="$broadcast" }
            //};
            //Methods["$rootScope"] = new List<MethodName> { 
            //    new MethodName{CSMethodName="apply", JSMethodName="$apply" },
            //    new MethodName{CSMethodName="watch", JSMethodName="$watch" },
            //    new MethodName{CSMethodName="on", JSMethodName="$on" },
            //    new MethodName{CSMethodName="broadcast", JSMethodName="$broadcast" }
            //};
            
            Methods["Dictionary"] = new List<MethodName> { 
                new MethodName{CSMethodName="Add", JSMethodName="Add" },
                new MethodName{ CSMethodName="ContainsKey", JSMethodName="ContainsKey"},
                new MethodName{ CSMethodName="ContainsValue", JSMethodName="ContainsValue"},
                 new MethodName{ CSMethodName="Where", JSMethodName="Where"},
                  new MethodName{ CSMethodName="Remove", JSMethodName="Remove"},
                 new MethodName{ CSMethodName="Clear", JSMethodName="Clear"},
                  new MethodName{ CSMethodName="ForEach", JSMethodName="ForEach"},
                   new MethodName{ CSMethodName="ToList", JSMethodName="ToList"},
                   new MethodName{ CSMethodName="ToJsonObject",JSMethodName ="ToJsonObject"},
                    new MethodName{ CSMethodName="getKeys", JSMethodName="getKeys"},
                   new MethodName{ CSMethodName="getValues",JSMethodName ="getValues"},
                    new MethodName{ CSMethodName="getCount",JSMethodName ="getCount"}
            };


            List<MethodName> forString = new List<MethodName> { 
                new MethodName{CSMethodName="format", JSMethodName="format" },              
                new MethodName{CSMethodName="ToLower", JSMethodName="toLowerCase" },
                new MethodName{CSMethodName="ToUpper", JSMethodName="toUpperCase" },
                 new MethodName{CSMethodName="Trim", JSMethodName="trim" },
                  new MethodName{CSMethodName="substr", JSMethodName="substr" },
                   new MethodName{CSMethodName="Split", JSMethodName="split" },
                   new MethodName{CSMethodName="Replace", JSMethodName="replace" },
                    new MethodName{CSMethodName="replace", JSMethodName="replace" },
                    new MethodName{CSMethodName="IndexOf", JSMethodName="indexOf" },
                      new MethodName{CSMethodName="LastIndexOf", JSMethodName="lastIndexOf" },
                     new MethodName{CSMethodName="ToCharArray", JSMethodName="ToCharArray" },
                      new MethodName{CSMethodName="StartsWith", JSMethodName="StartsWith" },
                       new MethodName{CSMethodName="match", JSMethodName="match" },
                       new MethodName{CSMethodName="charAt", JSMethodName="charAt" },
                       new MethodName{CSMethodName="charCodeAt", JSMethodName="charCodeAt" },
                       new MethodName{CSMethodName="substring", JSMethodName="substring" },
                        new MethodName{CSMethodName="ToString", JSMethodName="toString" },
                        new MethodName{CSMethodName="Substring", JSMethodName="substring" },
                         new MethodName{CSMethodName="getLength", JSMethodName="getLength"}
                
            };
            Methods["String"] = forString;
            Methods["string"] = forString;
        }
        public JSLookupTable()
        {
            SetDefaultMethods();
        }

        public string GetJSMethodName(string csObjecttype, string csMethodName, int lineNo)
        {

            if (csObjecttype == null) return "";
            if (Methods.ContainsKey(csObjecttype))
            {
                var list = Methods[csObjecttype];
                var x = (from u in list where u.CSMethodName == csMethodName select u).SingleOrDefault();
                if (x == null)
                {
                    throw new Exception(String.Format("For CS Object type '{0}', Method Name '{1}' is not supported for js at line no {2}.", csObjecttype, csMethodName, lineNo));
                }
                return x.JSMethodName;
            }
            return "";
        }
    }

    public class MethodName
    {
        public string CSMethodName { get; set; }
        public string JSMethodName { get; set; }
    }
}
