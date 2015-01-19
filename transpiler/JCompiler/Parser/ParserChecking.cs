using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Tokens;
using JCompiler.Ast;
using System.Collections;

namespace JCompiler.Parser
{
    public partial class Parser
    {
        private string msg(string str, params object[] msglist) { return string.Format(str, msglist); }
        internal bool IsLembda(int index)
        {
            return tokens[index] is TStringLiteral && (tokens[index + 1] is TLembda);
        }
        internal bool IsClass(int index)
        {
            return tokens[index + 1] is TClass;
        }
        internal bool IsMethodCall(int index)
        {
            if (tokens[index] is TBeginParen) return true;
            else if (tokens[index] is TLT)
            {
                index = ResolveLTGTForMethodCall(index);
                return tokens[index + 1] is TBeginParen;
            }
            return false;
        }
        private int ResolveLTGTForMethodCall(int index)
        {
            Stack stack = new Stack();
            if (this.tokens[index] is TLT)
            {
                stack.Push('<');
            }
            else
            {
                throw new Exception("Internal error at method ResolveLTGT()");
            }
            index++;
            int temp=index ;
            try
            {
                while (stack.Count != 0)
                {
                    if ((this.tokens[index] is TLT)) stack.Push('<');
                    else if ((this.tokens[index] is TGT)) stack.Pop();
                    index++;
                    if (this.tokens[index] is TSemi) { stack.Clear(); }
                }
            }
            catch (Exception)
            {

                return temp;
            }
            index--;
            return index;
        }
        internal bool IsConstructor(int index)
        {
            return ((tokens[index + 1] is TStringLiteral) && (tokens[index + 2] is TBeginParen));
        }
        private string CURRENT_METHOD_TYPE = "";
        private int GetParamTypeToCheckMethod(int index)
        {
            try
            {
                string typeName = "";

                if (this.tokens[index] is TFunc) { index = ResolveLTGT(index + 1); CURRENT_METHOD_TYPE = "func"; }
                else if (this.tokens[index] is TList) { index = ResolveLTGT(index + 1); CURRENT_METHOD_TYPE = "List"; }
                else if (this.tokens[index] is TLT) { typeName = GetString(this.tokens[index - 1]); index = ResolveLTGT(index); }
                else if (this.tokens[index] is TStringLiteral && this.tokens[index + 1] is TLT) { typeName = GetString(this.tokens[index]); index++; index = ResolveLTGT(index); }
                else if (this.tokens[index] is TStringLiteral && this.tokens[index + 1] is TDot) { index += 2; index = GetParamTypeToCheckMethod(index); }
                else if (this.tokens[index] is TDictionary)
                {
                    index++; index = ResolveLTGT(index); CURRENT_METHOD_TYPE = "Dictionary";
                }
                else if (this.tokens[index] is TEnum) { return index; }
                else CURRENT_METHOD_TYPE = GetString(this.tokens[index]);

                return index;
            }
            catch (Exception ex)
            {

                throw new Exception("Internal error at method GetParamTypeToCheckMethod(). error: " + ex.Message);
            }
        }
        internal bool IsFunctionMember(int index)
        {
            try
            {
                if (tokens[index] is TVirtual || tokens[index] is TOverride) { index++; }
                index = GetParamTypeToCheckMethod(index);
                return ((tokens[index + 1] is TStringLiteral) && (tokens[index + 2] is TBeginParen));

            }
            catch (Exception ex)
            {

                throw new Exception("Internal error at method IsFunctionMember(). error: " + ex.Message);
            }
        }

        internal bool IsDataMember(int index)
        {
            if ((tokens[index] is TStringLiteral))
            {
                if (GetString(tokens[index]) == "this")
                {
                    return false;
                }

                index++;
                if (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual) { return true; }
                else if (tokens[index] is TLT)
                {
                    index = ResolveLTGT(index);
                    index++;
                    if (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual) { return true; }
                }
                else if (tokens[index] is TDot)
                {
                    index++;
                    return IsDataMember(index);
                }
                else if (tokens[index] is TBeginBraket)
                {
                    index += 2;
                    return (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual);
                }
                else if (tokens[index] is TFunc)
                {
                    index ++;
                    index = ResolveLTGT(index);
                    index++;
                    return (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual);
                }

            }
            else if (tokens[index] is TFunc)
            {
                index++;
                index = ResolveLTGT(index);
                index++;
                return (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual);
            }
            return false;
        }

        internal bool IsList(int index)
        {
            return (tokens[index + 1] is TList);
        }
        internal bool IsDictionary(int index)
        {
            return (tokens[index + 1] is TDictionary);
        }
        internal void chequeClassName(string str)
        {
            if (string.IsNullOrEmpty(currentClass))
            {
                throw new System.Exception(str);
            }
        }
        internal void AddVariable(DeclareVar declareVar)
        {
            //if (currentMethod == null)
            //{
            //    lookupTable.AddClassVariable(currentClass, declareVar, mToken.LineNumber);
            //}
            //else
            //{
            //    lookupTable.AddMethodVariable(currentClass, currentMethod, declareVar, mToken.LineNumber);
            //}
        }
        internal void chequeMethod(string msg) { if (currentMethod == null) { throw new System.Exception(msg); } }

        private string GetString(Token token)
        {
            if (token is TStringLiteral)
            {
                return ((TStringLiteral)token).Value;
            }
            else
            {
                mToken = token;
                throw new System.Exception(String.Format("{0} error at line number {1}. info:{2}", "TStringLiteral", mToken.LineNumber, mToken.ToString()));
            }
        }

        private int ResolveLTGT(int index)
        {
            Stack stack = new Stack();
            if (this.tokens[index] is TLT)
            {
                stack.Push('<');
            }
            else
            {
                throw new Exception("Internal error at method ResolveLTGT()");
            }
            index++;
            while (stack.Count != 0)
            {
                if ((this.tokens[index] is TLT)) stack.Push('<');
                else if ((this.tokens[index] is TGT)) stack.Pop();
                index++;
            }
            index--;
            return index;
        }

        private string GetParamType()
        {
            try
            {
                string typeName = "";

                if (this.tokens[this.index] is TFunc) return "func";
                else if (this.tokens[this.index] is TList) { this.index = ResolveLTGT(this.index + 1); typeName = "List"; }
                else if (this.tokens[this.index] is TLT) { typeName = GetString(this.tokens[this.index - 1]); this.index = ResolveLTGT(this.index); }
                else if (this.tokens[this.index] is TStringLiteral && this.tokens[this.index + 1] is TLT) { typeName = GetString(this.tokens[this.index]); this.index++; this.index = ResolveLTGT(this.index); }
                else if (this.tokens[this.index] is TStringLiteral && this.tokens[this.index + 1] is TDot) { this.index += 2; typeName = GetParamType(); }
                else if (this.tokens[this.index] is TDictionary)
                {
                    this.index++; this.index = ResolveLTGT(this.index); typeName = "Dictionary";
                }
                else typeName = GetString(this.tokens[this.index]);
                return typeName;
            }
            catch (Exception ex)
            {

                throw new Exception("Internal error at method GetParamType(). error: " + ex.Message);
            }
        }
        private MethodInfo GetMethodInfo()
        {
            MethodInfo info = new MethodInfo();

            TModifier modifier = tokens[this.index] as TModifier;
            info.ModifierName = modifier.Name;
            this.index++;
            if (tokens[index] is TVirtual || tokens[index] is TOverride) { this.index++; }
            this.index = GetParamTypeToCheckMethod(this.index);
            info.ReturnType = CURRENT_METHOD_TYPE;
            this.index++;
            info.MethodName = GetString(tokens[this.index]);
            if (tokens[this.index + 1] is TLT)
            {
                this.index++;
                this.index = ResolveLTGT(this.index);
            }
            this.index += 2;
            info.Index = this.index;
            return info;
            
        }
        private void ListHasData(ref int temp, ref bool hasData)
        {
            if (this.tokens[index + 1] is TLT)
            {
                temp = index;
                temp++;
                temp = ResolveLTGT(temp);
               
            }
            if (tokens[temp + 1] is TBeginParen && tokens[temp + 2] is TEndParen)
            {
                if (tokens[temp + 3] is TBeginBrace)
                {
                    if (!(tokens[temp + 4] is TEndBrace))
                    {
                        hasData = true;
                        temp +=4;
                    }
                    else
                    {
                        temp += 4;
                    }
                }
                else
                {
                    temp += 2;
                }
            }
            else if (tokens[temp + 1] is TBeginBrace)
            {
                if (!(tokens[index + 2] is TEndBrace))
                {
                    hasData = true;
                    temp += 2;
                }
                else
                {
                    temp += 2;
                }
            }
        }
       
    }

    public class MethodInfo
    {
        public int Index { get; set; }
        public string MethodName { get; set; }
        public string ModifierName { get; set; }
        public string ReturnType { get; set; }
        public string Base { get; set; }
    }
}
