using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Ast;
using JCompiler.Tokens;
using JCompiler.Utilities;
using System.Collections;

namespace JCompiler.Parser
{
    public partial class Parser
    {
        #region Parse Class
        private void ParseClass(Stmt result)
        {
            mToken = this.tokens[this.index] as TClass;
            this.index++;

            result = ParseClass(GetString(this.tokens[this.index]));
            if (!(this.tokens[this.index] is TEndBrace))
            {
                throw new System.Exception("expected  end brace'}' to end a Class body, Line Number: " + mToken.LineNumber + "\nDo not forget to assaign a value when you declare a variable.");
            }
            this.index++;
            if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndBrace))
            {
                StmtSequence sequence = new StmtSequence();
                sequence.First = result;
                sequence.Second = this.ParseStmt();
                result = sequence;
            }


        }

        private Stmt ParseClass(string name)
        {
            currentClass = name;
            lookupTable.AddClass(name, mToken.LineNumber);
            Stmt result = null;
            AstClass ast_class = new AstClass();
            ast_class.Namespace = this.Namespace;
            ast_class.ClassName = name;
            int temp = this.index;
            temp++;
            if (this.tokens[temp] is TLT)
            {
                temp = ResolveLTGT(temp);
                temp++;
            }
            if (this.tokens[temp] is TClone)
            {
                temp++;
                ast_class.InheritedBy = "";
                while (!(this.tokens[temp] is TBeginBrace))
                {
                    if (this.tokens[temp] is TStringLiteral)
                    {
                        ast_class.InheritedBy += GetString(this.tokens[temp]);
                    }
                    else if (this.tokens[temp] is TDot)
                    {
                        ast_class.InheritedBy += ".";
                    }
                    else if (this.tokens[temp] is TLT)
                    {
                        temp = ResolveLTGT(temp);

                    }
                    temp++;
                }

                this.index = temp + 1;
            }
            else
            {
                ast_class.InheritedBy = "jsClass";
                this.index = temp + 1;
            }
            if (!(this.tokens[this.index] is TEndBrace))
                ast_class.Content = ParseStmt();
            result = ast_class;
            currentClass = string.Empty;
            return result;
        }
        #endregion

        #region Parse Function

        private Stmt ParseFunction(MethodInfo info)
        {

            // Stmt result = null;
            Function func = new Function();

            if (mToken != null)
                chequeClassName("Function declaration should be inside a classs. Line Number :" + mToken.LineNumber);

            func.ModifierName = info.ModifierName;
            func.ReturnType = info.ReturnType;
            func.FunctionName = info.MethodName;
            index = info.Index;
            currentMethod = func;
            while (!(this.tokens[this.index] is TEndParen))
            {

                if (this.tokens[this.index] is TComma)
                {
                    this.index++;
                }
                else if (this.tokens[this.index] is TEqual) {
                    this.index += 2;
                }
                else
                {
                    FunctionParam param = new FunctionParam();
                    param.ParamType = GetParamType();

                    if (this.tokens[this.index] is TFunc)
                    {
                        //do { this.index++; } while (!(this.tokens[this.index] is TGT));
                        index++;
                        index = ResolveLTGT(index);
                        this.index++;
                        param.ParamName = GetString(this.tokens[this.index]);
                        func.ParamList.Add(param);
                        this.index++;
                        param.ParamType = "object";
                    }
                    else
                    {
                        this.index++;
                        param.ParamName = GetString(this.tokens[this.index]);
                        func.ParamList.Add(param);
                        this.index++;
                    }
                }


            }
            lookupTable.AddMethod(currentClass, func, mToken.LineNumber);
            currentMethod = func;
            if (this.tokens[this.index + 1] is TClone)
            {
                this.index++;
                if (this.tokens[this.index + 1] is TBase)
                {
                    this.index += 2;
                    func.Base = ParseExpr();
                    this.index--;
                }
                else
                {
                    throw new Exception("Invalid constructor");
                }
            }
            this.index += 2;
            if (!(this.tokens[this.index] is TEndBrace))
                func.Content = ParseStmt();
            currentMethod = null;

            return func;
        }
        #endregion

        private string GetTypeName(Token token)
        {
            if (token is TList)
            {
                index += 3;
                return "List";
            }
            else if (token is TFunc)
            {
                do { this.index++; } while (!(this.tokens[this.index] is TEqual));
                index -= 2;
                return "func";
            }
            else if (token is TDictionary)
            {
                int temp = 0;
                if (tokens[index + 2] is TStringLiteral)
                {
                    if (tokens[index + 4] is TList)
                    {
                        temp = 8;
                    }
                    else
                    {
                        temp = 5;
                    }
                }
                else
                {
                    throw new System.Exception("Invalid Dictionary at line number:" + mToken.LineNumber);
                }
                index += temp;
                return "Dictionary";
            }
            else if (token is TStringLiteral)
            {
                return GetString(token);
            }
            throw new System.Exception("Invalid type declaration at line number:" + mToken.LineNumber);
        }
        private string GetTypeName2(Token token)
        {
            if (token is TList)
            {

                return "List";
            }
            else if (token is TFunc)
            {

                return "func";
            }
            else if (token is TDictionary)
            {

                return "Dictionary";
            }
            else if (token is TStringLiteral)
            {
                return GetString(token);
            }
            throw new System.Exception("Invalid type declaration at line number:" + mToken.LineNumber);
        }
        private bool FindVariableInfo(VariableInfo varInfo, int index)
        {
            if ((tokens[index] is TStringLiteral) || (tokens[index] is TFunc) || (tokens[index] is TList) || (tokens[index] is TDictionary))
            {
                index++;
                if (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual)
                {
                    varInfo.VarName = GetString(tokens[index]);
                    varInfo.IndexStatus = index + 1;

                    return true;
                }
                else if (tokens[index] is TLT)
                {
                    varInfo.ReturnType = GetTypeName2(tokens[index - 1]);

                    index = ResolveLTGT(index);
                    index++;
                    if (tokens[index] is TStringLiteral && tokens[index + 1] is TEqual)
                    {
                        varInfo.VarName = GetString(tokens[index]);
                        varInfo.IndexStatus = index + 1;
                        return true;
                    }
                }
                else if (tokens[index] is TDot)
                {
                    index++;
                    return FindVariableInfo(varInfo, index);
                }
                else if (tokens[index] is TBeginBraket)
                {
                    varInfo.ReturnType = GetTypeName2(tokens[index - 1]);
                    index += 2;
                    return FindVariableInfo(varInfo, index);
                }
                else if (tokens[index] is TEqual)
                {
                    varInfo.VarName = GetString(tokens[index - 1]);
                    varInfo.IndexStatus = index;
                    return true;
                }
            }


            return false;
        }
        private void VariableDiclarationForEnum(DeclareVar declareVar)
        {

            this.index++;
            declareVar.Ident = GetString(tokens[index]);
            if (tokens[index + 1] is TBeginBrace)
            {
                AstJsonObject obj = new AstJsonObject();
                index += 2;
                int val = 0;
                while (!(tokens[index] is TEndBrace))
                {

                    if (this.tokens[index] is TStringLiteral)
                    {
                        Assign ass = new Assign();
                        ass.Ident = GetString(tokens[index]);
                        IntLiteral intLiteral = new IntLiteral();
                        intLiteral.Value = val++;
                        ass.Expr = intLiteral;
                        obj.list.Add(ass);
                    }
                    index++;
                }
                index++;
                declareVar.Expr = obj;
            }
            else
            {
                throw new Exception("Incorrect enum declaration.");
            }
        }
        private void VariableDiclaration(DeclareVar declareVar)
        {

            VariableInfo varInfo = new VariableInfo();
            FindVariableInfo(varInfo, index);
            declareVar.TypeName = varInfo.ReturnType;
            declareVar.Ident = varInfo.VarName;
            this.index = varInfo.IndexStatus;

            if (this.index == this.tokens.Count || !(this.tokens[this.index] is TEqual))
            {
                throw new System.Exception("assignment missing(=) at line number:" + mToken.LineNumber);
            }

            this.index++;

            declareVar.Expr = this.ParseExpr();
            AddVariable(declareVar);
            if (parenthesisCounter != 0)
            {
                throw new System.Exception("Ending Parenthesis mismatch at line number:" + mToken.LineNumber);
            }
            if ((this.tokens[this.index] is TEndParen))
            {
                throw new System.Exception("Begining Parenthesis mismatch at line number:" + mToken.LineNumber);
            }
            if (this.index < this.tokens.Count && (this.tokens[this.index] is TComma))
            {
                do
                {
                    Assign ass = new Assign();
                    this.index++;
                    ass.Ident = GetString(this.tokens[this.index]);
                    if (!(this.tokens[this.index + 1] is TEqual))
                    {
                        throw new System.Exception("Do not forget to assaign a value when you declare a variable. Line no:" + mToken.LineNumber);
                    }
                    this.index += 2;
                    ass.Expr = ParseExpr();
                    declareVar.list.Add(ass);
                } while (this.index < this.tokens.Count && !(this.tokens[this.index] is TSemi));
            }
            if (this.index < this.tokens.Count && !(this.tokens[this.index] is TSemi))
            {
                throw new System.Exception("Semiclone missing at line number:" + mToken.LineNumber);
            }


        }

        #region Func Method Call/ Delegate / Lembda
        private Expr ParseDelegate()
        {
            mToken = this.tokens[this.index] as TDelegate;
            ASTDelegate result = new ASTDelegate();
            this.index++;
            if (!(this.tokens[this.index] is TBeginParen))
            {
                throw new System.Exception("Missing '(' at line number" + mToken.LineNumber);
            }

            this.index++;
            while (!(this.tokens[this.index] is TEndParen))
            {
                if (this.tokens[this.index] is TComma)
                {
                    this.index++;
                }
                else
                {
                    FunctionParam param = new FunctionParam();
                    param.ParamType = GetParamType();
                    this.index++;
                    {
                        param.ParamName = GetString(this.tokens[this.index]);
                        result.ParamNameList.Add(param.ParamName);
                        this.index++;
                    }
                }
            }
            this.index += 2;

            result.Body = ParseStmt();
            if (!(this.tokens[this.index] is TEndBrace))
            {
                throw new System.Exception("Missing '}' of func method defination at line number " + mToken.LineNumber);
            }
            this.index++;
            return result;
        }
        private Expr ParseLembda(bool isSingleParam = false)
        {
            ASTLembda result = new ASTLembda();
            if (!isSingleParam)
            {
                mToken = this.tokens[this.index] as TBeginParen;

                if (!(this.tokens[this.index] is TBeginParen))
                {
                    throw new System.Exception("Missing '(' at line number" + mToken.LineNumber);
                }
                this.index++;
                while (!(this.tokens[this.index] is TEndParen))
                {
                    if (this.tokens[this.index] is TComma)
                    {
                        this.index++;
                    }
                    else
                    {
                        FunctionParam param = new FunctionParam();
                        //param.ParamType = GetParamType();
                        //this.index++;
                        {
                            param.ParamName = GetString(this.tokens[this.index]);
                            result.ParamNameList.Add(param.ParamName);
                            this.index++;
                        }
                    }
                }
                this.index += 2;
            }
            else
            {
                mToken = this.tokens[this.index] as TLembda;
                FunctionParam param = new FunctionParam();
                param.ParamName = GetString(this.tokens[this.index - 1]);
                result.ParamNameList.Add(param.ParamName);
                this.index += 1;
            }
            if (this.tokens[this.index] is TBeginBrace)
            {
                this.index++;
                result.Body = ParseStmt();
                this.index++;
            }
            else
            {
                result.singleBody = ParseExpr();
            }

            return result;
        }
        private Expr FuncMethodCall()
        {
            mToken = this.tokens[this.index] as TFunc;
            Func result = new Func();
            this.index++;
            if (!(this.tokens[this.index] is TBeginParen))
            {
                throw new System.Exception("Missing '(' at line number" + mToken.LineNumber);
            }
            this.index++;
            while (!(this.tokens[this.index] is TEndParen))
            {
                if (this.tokens[this.index] is TComma)
                {
                    this.index++;
                }
                else
                {
                    if (!(this.tokens[this.index] is TStringLiteral))
                    {
                        throw new System.Exception("Syntex error of func method defination at line number " + mToken.LineNumber);
                    }
                    result.ParamNameList.Add(GetString(this.tokens[this.index]));
                    this.index++;
                }
            }
            this.index += 2;
            result.Body = ParseStmt();
            if (!(this.tokens[this.index] is TEndBrace))
            {
                throw new System.Exception("Missing '}' of func method defination at line number " + mToken.LineNumber);
            }
            this.index++;
            return result;
        }
        #endregion
        private Expr ParseLinqQuery()
        {
            AstLinq linq = new AstLinq();
            this.index++;
            linq.varName = GetString(this.tokens[this.index]);
            this.index += 2;
            linq.InExp = this.ParseExpr();
            if (this.tokens[this.index] is TJoin)
            {
                this.index++;
                linq.Join = new AstLinq();
                linq.Join.varName = GetString(this.tokens[this.index]);
                this.index += 2;
                linq.Join.InExp = this.ParseExpr();
                this.index++;
                linq.Join.WhereExp = this.ParseExpr();
            }
            if (this.tokens[this.index] is TWhere)
            {
                this.index++;
                linq.WhereExp = this.ParseExpr();
            }
            if (this.tokens[this.index] is TSelect)
            {
                this.index++;
                if (this.tokens[this.index] is TNew)
                    linq.SelectObject = this.ParseExpr() as AstJsonObject;
                else
                    linq.SelectVarName = this.ParseExpr();
            }
            return linq;
        }
        private Stmt parseStmtIncrementForLoop()
        {
            Stmt result = null;
            if (this.tokens[this.index] is TStringLiteral)
            {
                string ident = GetString(this.tokens[this.index]);
                index++;
                if (this.tokens[this.index] is TIncrement)
                {
                    PostIncrementOp inc = new PostIncrementOp();
                    inc.Ident = ident;
                    result = inc;
                }
                else if (this.tokens[this.index] is TDecrement)
                {
                    PostDecrementOp dec = new PostDecrementOp();
                    dec.Ident = ident;
                    result = dec;
                }
                else if (this.tokens[this.index] is TAddEqual)
                {
                    AddEqual dec = new AddEqual();
                    dec.Ident = ident;
                    this.index++;
                    dec.Expr = ParseExpr();
                    result = dec;
                }
                else if (this.tokens[this.index] is TSubEqual)
                {
                    SubEqual dec = new SubEqual();
                    dec.Ident = ident;
                    this.index++;
                    dec.Expr = ParseExpr();
                    result = dec;
                }
                else if (this.tokens[this.index] is TMulEqual)
                {
                    MulEqual dec = new MulEqual();
                    dec.Ident = ident;
                    this.index++;
                    dec.Expr = ParseExpr();
                    result = dec;
                }
                else if (this.tokens[this.index] is TDivEqual)
                {
                    DivEqual dec = new DivEqual();
                    dec.Ident = ident;
                    this.index++;
                    dec.Expr = ParseExpr();
                    result = dec;
                }

            }
            else if (this.tokens[this.index] is TIncrement)
            {
                PreIncrementOp inc = new PreIncrementOp();
                this.index++;
                inc.Ident = GetString(this.tokens[this.index]);
                result = inc;
            }
            else if (this.tokens[this.index] is TDecrement)
            {
                PreDecrementOp dec = new PreDecrementOp();
                this.index++;
                dec.Ident = GetString(this.tokens[this.index]);
                result = dec;

            }
            return result;
        }
        private Stmt parseStmtInitForLoop()
        {
            Stmt result = null;
            mToken = this.tokens[this.index] as TStringLiteral;
            if (IsDataMember(index))
            {
                DeclareVar declareVar = new DeclareVar();
                VariableDiclaration(declareVar);
                result = declareVar;
            }

            else if (tokens[index + 1] is TEqual)
            {

                Assign assign = new Assign();
                assign.Ident = GetString(mToken);
                this.index += 2;
                assign.Expr = this.ParseExpr();
                result = assign;
            }

            return result;
        }

        private object GetCaseValue(Token token)
        {
            if (token is TStringInQuotation)
            {
                return string.Format("'{0}'", ((TStringInQuotation)token).Value);
            }
            if (token is TIntegerValue)
            {
                return ((TIntegerValue)token).Value;
            }
            return "";
        }

    }


}
