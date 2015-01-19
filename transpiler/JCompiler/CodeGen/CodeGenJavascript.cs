using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Ast;
using JCompiler.Parser;
using JCompiler.Utilities;


namespace JCompiler.CodeGen
{
    public class CodeGenJavascript
    {
        private string NEW_LINE = Environment.NewLine;
        private Javascript SCRIPT = new Javascript();
        private readonly StringBuilder result = new StringBuilder();
        private readonly List<string> resultSet = new List<string>();
        private bool isBindWithThis = true;
        public CodeGenJavascript(Stmt stmt, bool isBindWithThis = true)
        {
            this.isBindWithThis = isBindWithThis;
            this.GenStmt(stmt);
            result.Clear();
            //result.Append(SCRIPT.ToString());
        }
        private string classScopeId = "";
        public String Result
        {
            get
            {
                string res = Environment.NewLine + "(function(window){" + Environment.NewLine;
                foreach (var item in resultSet)
                {
                    res += item;
                }
                res += Environment.NewLine + "})(window);";
                return res;
            }
        }
        private string currentMethod = null;
        private string currentModifier = null;
        private JSLookupTable lookup = new JSLookupTable();
        private Stack<string> scopeStack = new Stack<string>();
        private string getScopeName() { return Guid.NewGuid().ToString(); }
        private string getTypeName(Expr ex)
        {

            if (ex is IntLiteral) return "int";
            if (ex is FloatLiteral) return "float";
            if (ex is DoubleLiteral) return "double";
            //if (ex is StringLiteral) return "string";
            if (ex is AstJsonArrey) return "List";
            if (ex is AstEmptyList) return "List";
            if (ex is AstJsonObject) return "Dictionary";
            return "";
        }
        private string MethodObjectType = "";
        private bool isConstructorBase = false;
        private void GenStmt(Stmt stmt)
        {
            #region Sequence
            if (stmt is StmtSequence)
            {
                StmtSequence seq = (StmtSequence)stmt;
                this.GenStmt(seq.First);
                this.GenStmt(seq.Second);
            }
            else if (stmt is SModifier)
            {
                SModifier cl = (SModifier)stmt;
                currentModifier = cl.Name;

            }
            #endregion
            #region Complex StateMent
            if (stmt is ASTComplexStmt)
            {
                ASTComplexStmt seq = (ASTComplexStmt)stmt;
                this.GenExpr(seq.LeftExpression);
                if (seq.RightExpression != null)
                {
                    QAdd("=");
                    MethodObjectType = "";
                    this.GenExpr(seq.RightExpression);
                }
                Semi(seq);
                NewLine();
            }
            #endregion
            else if (stmt is SNamespace)
            {
                this.AstNamespace = stmt as SNamespace;

            }
            #region AstClass
            else if (stmt is AstClass)
            {
                scopeStack.Push((classScopeId = getScopeName()));
                lookup.ClassScopeID = classScopeId;
                AstClass cl = (AstClass)stmt;
                SCRIPT = new Javascript();
                if (!string.IsNullOrEmpty(AstNamespace.ActionType) && AstNamespace.ActionType == "controller")
                {
                    SCRIPT.IsAngular = true;
                }
                SCRIPT.AstNamespace = this.AstNamespace;
                SCRIPT.ClassName = cl.ClassName;
                SCRIPT.InhritedBy = cl.InheritedBy;
                SCRIPT.Namespace = cl.Namespace;
                GenStmt(cl.Content);
                resultSet.Add(SCRIPT.ToString());
                scopeStack.Pop();
            }
            #endregion
            #region Function
            else if (stmt is Function)
            {
                scopeStack.Push(getScopeName());
                Function f = (Function)stmt;
                f.FunctionName = f.FunctionName.Replace("_s_", "$");
                currentMethod = f.FunctionName;
                /*if (f.ModifierName == "public")
                {
                    QAddF("this.{0}=function(", f.FunctionName);
                }
                else
                {
                    QAddF("function {0}(", f.FunctionName);
                }*/
                if (f.ModifierName == "public" && f.FunctionName != "init")
                {
                    SCRIPT.InitMethodHelper.AppendFormat("this.scope.{0}={1}\n", f.FunctionName, "this." + f.FunctionName + ".bind(this);");
                }
                if (f.FunctionName == "ToString")
                {
                    f.FunctionName = "toString";
                }
                QAddF("{0}:function(", f.FunctionName);
                bool isFirst = true, flag2 = true;
                foreach (FunctionParam item in f.ParamList)
                {
                    item.ParamName = item.ParamName.Replace("_s_", "$");
                    if (item.ParamType == "func")
                    {
                        if (isFirst)
                        {
                            QAddF("{0} <", item.ParamType);
                            flag2 = true;
                            foreach (var item2 in item.FuncSignature)
                            {
                                if (flag2)
                                {
                                    QAdd(item2);
                                    flag2 = false;
                                }
                                else
                                {
                                    QAdd("," + item2);
                                }
                            }
                            QAdd("> " + item.ParamName);
                            isFirst = false;
                        }
                        else
                        {
                            QAddF(",{0} <", item.ParamType);
                            flag2 = true;
                            foreach (var item2 in item.FuncSignature)
                            {
                                if (flag2)
                                {
                                    QAdd(item2);
                                    flag2 = false;
                                }
                                else
                                {
                                    QAdd("," + item2);
                                }
                            }
                            QAdd("> " + item.ParamName);
                        }
                    }
                    else
                    {
                        if (isFirst)
                        {
                            // if (item.ParamName == "$scope" || item.ParamName == "scope") { SCRIPT.IsAngular = true; }
                            if (f.FunctionName == "init")
                            {
                                SCRIPT.InitMethodParams.Append(item.ParamName);
                            }
                            QAddF("{0}", item.ParamName);
                            isFirst = false;
                        }
                        else
                        {
                            // if (item.ParamName == "$scope" || item.ParamName == "scope") { SCRIPT.IsAngular = true; }
                            if (f.FunctionName == "init")
                            {
                                SCRIPT.InitMethodParams.AppendFormat(",{0}", item.ParamName);
                            }
                            QAddF(",{0}", item.ParamName);
                        }
                    }
                }
                QAdd(")");
                NewLine();
                QAdd("{");
                NewLine();
                if (f.Base != null)
                {
                    QAdd("this._super");
                    isConstructorBase = true;
                    GenExpr(f.Base);
                    isConstructorBase = false;
                    QAdd(";");
                    NewLine();
                }
                GenStmt(f.Content);
                NewLine();
                if (f.FunctionName == "init")
                {
                    // QAdd("var that=this;");
                    NewLine();
                    QAdd("$INIT$");
                }
                QAdd("},");
                //if (f.ModifierName == "public")
                //QAdd(";");

                NewLine();
                /* if (f.ModifierName == "public")
                     SCRIPT.PublicMemberFunction.Append(result);
                 else
                     SCRIPT.PrivateMemberFunction.Append(result);*/
                SCRIPT.PublicMemberFunction.Append(result);
                result.Clear();
                currentMethod = null;
                currentModifier = "private";
                scopeStack.Pop();
                MethodObjectType = "";
            }
            #endregion
            #region Switch
            else if (stmt is Switch)
            {
                Switch s = (Switch)stmt;
                NewLine();
                QAdd("switch(");
                GenExpr(s.Expr);
                QAdd(")");
                NewLine();
                QAdd("{");
                NewLine();
                GenStmt(s.Body);
                NewLine();
                QAdd("}");

            }
            #endregion
            #region Return
            else if (stmt is Return)
            {
                Return r = (Return)stmt;
                NewLine();
                QAdd("return ");
                GenExpr(r.Expr);
                Semi(r);
                NewLine();
            }
            #endregion
            #region Case
            else if (stmt is Case)
            {
                Case c = (Case)stmt;
                NewLine();
                QAddF("case {0}:", c.ident);
            }
            #endregion
            #region Default
            else if (stmt is Default)
            {
                Default c = (Default)stmt;
                NewLine();
                QAddF("default:");
            }
            #endregion
            //#region Print
            //else if (stmt is Print)
            //{
            //    Print p = (Print)stmt;
            //    NewLine();
            //    result.Append("print ");
            //    this.GenExpr(p.Expr);
            //    Semi(p);
            //}
            //#endregion
            #region DeclareVar
            else if (stmt is DeclareVar)
            {
                DeclareVar d = (DeclareVar)stmt;
                lookup.AddVariable(scopeStack.Peek(), d.Ident, d.TypeName /*getTypeName(d.Expr)*/);
                if (currentMethod != null)
                {
                    QAddF("var {0}=", d.Ident);
                }
                else
                {

                    SCRIPT.PublicMemberVariable.AppendFormat("{0}:", d.Ident);
                }
                GenExpr(d.Expr);
                if (currentMethod == null)
                {

                    SCRIPT.PublicMemberVariable.Append(result + ",");
                    SCRIPT.PublicMemberVariable.Append(NEW_LINE);
                    result.Clear();
                    foreach (var item in d.list)
                    {

                        SCRIPT.PublicMemberVariable.AppendFormat("{0}:", item.Ident);
                        GenExpr(item.Expr);
                        SCRIPT.PublicMemberVariable.Append(result + ",");
                        SCRIPT.PublicMemberVariable.Append(NEW_LINE);
                        result.Clear();

                    }
                }
                else
                {
                    foreach (var item in d.list)
                    {
                        if (currentMethod != null)
                        {
                            QAddF(",{0}=", item.Ident);
                            GenExpr(item.Expr);
                        }

                    }
                    Semi(d);
                }
                currentModifier = "private";
            }
            #endregion
            #region Assign
            else if (stmt is Assign)
            {
                Assign a = (Assign)stmt;
                QAdd(a.Ident + "=");
                GenExpr(a.Expr);

                Semi(a);

            }
            #endregion
            #region PreIncrementOp
            else if (stmt is PreIncrementOp)
            {
                PreIncrementOp p = (PreIncrementOp)stmt;
                NewLine();
                QAddF("++{0}", p.Ident);

                Semi(p);
            }
            #endregion
            #region PreDecrementOp
            else if (stmt is PreDecrementOp)
            {
                PreDecrementOp p = (PreDecrementOp)stmt;
                NewLine();
                QAddF("--{0}", p.Ident);

                Semi(p);

            }
            #endregion
            #region PostIncrementOp
            else if (stmt is PostIncrementOp)
            {
                PostIncrementOp p = (PostIncrementOp)stmt;
                NewLine();
                QAddF("{0}++", p.Ident);

                Semi(p);

            }
            #endregion
            #region PostDecrementOp
            else if (stmt is PostDecrementOp)
            {
                PostDecrementOp p = (PostDecrementOp)stmt;
                NewLine();
                QAddF("{0}--", p.Ident);

                Semi(p);

            }
            #endregion
            #region MethodCall
            else if (stmt is MethodCall)
            {
                MethodCall m = (MethodCall)stmt;
                string jsMethodName = lookup.GetJSMethodName(MethodObjectType, m.MethodName, m.LineNo);
                MethodObjectType = "";
                if (!String.IsNullOrEmpty(jsMethodName))
                    m.MethodName = jsMethodName;
                QAdd(m.MethodName);
                QAdd(m.BeginParen);
                bool isFirst = true;
                foreach (Expr item in m.paramList)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        GenExpr(item);
                    }
                    else
                    {
                        QAdd(",");
                        GenExpr(item);
                    }
                }
                QAdd(m.EndParen);

                Semi(m);

            }
            #endregion
            #region AddEqual
            else if (stmt is AddEqual)
            {
                AddEqual a = (AddEqual)stmt;
                NewLine();
                QAddF("{0} +=", a.Ident);
                GenExpr(a.Expr);
                Semi(a);

            }
            #endregion
            #region SubEqual
            else if (stmt is SubEqual)
            {
                SubEqual a = (SubEqual)stmt;
                NewLine();
                QAddF("{0} -=", a.Ident);
                GenExpr(a.Expr);
                Semi(a);

            }
            #endregion
            #region MulEqual
            else if (stmt is MulEqual)
            {
                MulEqual a = (MulEqual)stmt;
                NewLine();
                QAddF("{0} *=", a.Ident);
                GenExpr(a.Expr);
                Semi(a);

            }
            #endregion
            #region DivEqual
            else if (stmt is DivEqual)
            {
                DivEqual a = (DivEqual)stmt;
                NewLine();
                QAddF("{0} /=", a.Ident);
                GenExpr(a.Expr);
                Semi(a);

            }
            #endregion
            #region IF
            else if (stmt is IF)
            {
                scopeStack.Push(getScopeName());
                IF fi = (IF)stmt;
                NewLine();
                result.Append("if(");
                GenExpr(fi.Condition);
                result.Append("){");
                GenStmt(fi.Body);
                result.Append('}');
                NewLine();
                scopeStack.Pop();
            }
            #endregion
            #region ElseIf
            else if (stmt is ElseIF)
            {
                ElseIF fi = (ElseIF)stmt;
                NewLine();
                result.Append("else if(");
                GenExpr(fi.Condition);
                result.Append("){");
                GenStmt(fi.Body);
                result.Append('}');
                NewLine();
            }
            #endregion
            #region Else
            else if (stmt is ELSE)
            {
                ELSE fi = (ELSE)stmt;
                NewLine();
                result.Append("else");
                result.Append("{");
                GenStmt(fi.Body);
                result.Append('}');
                NewLine();
            }
            #endregion
            #region Parenthesis
            else if (stmt is Parenthesis)
            {
                Parenthesis v = (Parenthesis)stmt;
                if (v.content != null)
                {
                    //if (!(v.content is Variable))
                    //QAdd("(");
                    GenExpr(v.content);
                    //if (!(v.content is Variable))
                    // QAdd(")");
                }
            }
            #endregion
            #region New
            else if (stmt is New)
            {
                New v = (New)stmt;
                QAdd("new ");

            }
            #endregion
            #region WhileLoop
            else if (stmt is WhileLoop)
            {
                WhileLoop v = (WhileLoop)stmt;
                QAdd("while( ");
                GenExpr(v.Condition);
                QAdd(")");
                NewLine();
                QAdd("{");
                NewLine();
                GenStmt(v.Body);
                NewLine();
                QAdd("}");
            }
            #endregion
            #region ForLoop
            else if (stmt is ForLoop)
            {
                ForLoop f = (ForLoop)stmt;
                NewLine();
                QAdd("for(");
                GenStmt(f.Initialize);
                Semi();
                GenExpr(f.Condition);
                Semi();
                GenStmt(f.Increment);
                QAdd(")");
                NewLine();
                QAdd("{");
                NewLine();
                GenStmt(f.Body);
                NewLine();
                QAdd("}");
                NewLine();
            }
            #endregion
            #region Break
            else if (stmt is Break)
            {
                QAdd("break;\n");
            }
            #endregion
            #region Continue
            else if (stmt is Continue)
            {
                QAdd("continue;\n");
            }
            #endregion
            #region DotLeft
            else if (stmt is DOTLeft)
            {
                DOTLeft d = (DOTLeft)stmt;
                MethodObjectType = lookup.GetVarType(scopeStack.Peek(), d.Ident);
                QAdd(d.Ident);
            }
            #endregion
            #region Dot
            else if (stmt is DOT)
            {
                QAdd(".");
            }
            #endregion
            #region Foreach
            else if (stmt is ForEach)
            {
                string res = result.ToString();
                result.Clear();
                ForEach obj = stmt as ForEach;
                //res += "for (var __i = 0,__l=";
                GenExpr(obj.Source);
                string listName = result.ToString();
                result.Clear();
                //res += listName;
                //res += ".length; __i < __l; __i++) {";
                //res += String.Format("var {0}={1}[__i];", obj.varName, listName);
                res += listName + ".forEach(function(" + obj.varName + "){" + Environment.NewLine;
                if (obj.Body != null)
                {
                    GenStmt(obj.Body);
                    res += result.ToString();
                    result.Clear();

                }
                if (this.isBindWithThis)
                    res += "}.bind(this));" + Environment.NewLine;
                else
                    res += "});" + Environment.NewLine;
                result.Append(res);
            }
            #endregion
            #region ArrayIndexing
            else if (stmt is ASTArrayIndex)
            {
                ASTArrayIndex arr = stmt as ASTArrayIndex;
                string res = result.ToString();
                result.Clear();
                QAddF("{0}[", arr.Ident);
                GenExpr(arr.index);
                res += result.ToString() + "]";
                result.Clear();
                if (arr.isEqual)
                {
                    res += "=";
                    GenExpr(arr.Expr);
                    Semi(arr);
                    res += result.ToString();
                    result.Clear();
                    QAdd(res);
                }
                else
                {
                    QAdd(res);

                }
            }
            #endregion
            #region MethodCall
            else if (stmt is AstBase)
            {
                AstBase astBase = (AstBase)stmt;
                MethodCall m = astBase.Method;
                string jsMethodName = lookup.GetJSMethodName(MethodObjectType, m.MethodName, m.LineNo);
                MethodObjectType = "";
                if (!String.IsNullOrEmpty(jsMethodName))
                    m.MethodName = jsMethodName;
                QAdd("this." + m.MethodName);
                QAdd(m.BeginParen);
                bool isFirst = true;
                foreach (Expr item in m.paramList)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        GenExpr(item);
                    }
                    else
                    {
                        QAdd(",");
                        GenExpr(item);
                    }
                }
                QAdd(m.EndParen);

                Semi(astBase);

            }
            #endregion
            #region JScript
            else if (stmt is AstJS)
            {
                AstJS js = (AstJS)stmt;
                QAdd(js.JScript);

            }
            #endregion
        }
        private void GenExpr(Expr expr)
        {
            #region Sequence2
            if (expr is ExprSequence)
            {
                ExprSequence seq = (ExprSequence)expr;
                this.GenExpr(seq.First);
                this.GenExpr(seq.Second);
            }
            #endregion
            #region Casting By AS
            if (expr is ASTAs)
            {
                ASTAs exp = (ASTAs)expr;
                this.GenExpr(exp.Left);
            }
            #endregion
            #region ExprDot
            if (expr is ExprDot)
            {
                ExprDot seq = (ExprDot)expr;
                this.GenExpr(seq.First);
                if (seq.First is Variable)
                    MethodObjectType = lookup.GetVarType(scopeStack.Peek(), ((Variable)seq.First).Ident);
                else if (seq.First is StringQuotation)
                    MethodObjectType = "String";
                QAdd(".");
                this.GenExpr(seq.Second);
            }
            #endregion
            #region StringLiteral
            else if (expr is StringQuotation)
            {

                StringQuotation sliteral = (StringQuotation)expr;
                result.AppendFormat("\"{0}\"", sliteral.Value);

            }
            #endregion
            #region IntLiteral
            else if (expr is IntLiteral)
            {
                IntLiteral iliteral = (IntLiteral)expr;
                result.AppendFormat("{0}", iliteral.Value);

            }
            #endregion
            #region DoubleLiteral
            else if (expr is DoubleLiteral)
            {
                DoubleLiteral iliteral = (DoubleLiteral)expr;
                result.AppendFormat("{0}", iliteral.Value);

            }
            #endregion
            #region BinExpr
            else if (expr is BinExpr)
            {
                BinExpr be = (BinExpr)expr;
                GenExpr(be.Left);
                result.Append(GetOPName(be.Op));
                GenExpr(be.Right);
            }
            #endregion
            #region MethodCall2
            else if (expr is MethodCall2)
            {
                MethodCall2 m = (MethodCall2)expr;
                m.MethodName = m.MethodName.Replace("_s_", "$");
                string jsMethodName = lookup.GetJSMethodName(MethodObjectType, m.MethodName, m.LineNo);
                MethodObjectType = "";
                if (!String.IsNullOrEmpty(jsMethodName))
                    m.MethodName = jsMethodName;

                if (m.MethodName == "ToString")
                {
                    m.MethodName = "toString";
                }
                else if (m.MethodName == "isx")
                {
                    m.MethodName = "is";
                }
                else if (m.MethodName == "eventx")
                {
                    m.MethodName = "event";
                }
                else if (m.MethodName == "finallyx")
                {
                    m.MethodName = "finally";
                }
                bool addParenthesis = true;
                if (m.MethodName == "jQuery" && m.paramList.Count == 0)
                {
                    addParenthesis = false;
                }
                QAdd(m.MethodName);
                if (addParenthesis)
                {
                    QAdd(m.BeginParen);
                }
                bool isFirst = true;
                foreach (Expr item in m.paramList)
                {

                    if (isFirst)
                    {
                        isFirst = false;
                        GenExpr(item);
                    }
                    else
                    {
                        QAdd(",");
                        GenExpr(item);
                    }

                }
                if (addParenthesis)
                {
                    QAdd(m.EndParen);
                }

                if (m.paramListForAnnonimysMethod != null)
                {
                    isFirst = true;
                    QAdd("(");
                    foreach (Expr item in m.paramListForAnnonimysMethod)
                    {

                        if (isFirst)
                        {
                            isFirst = false;
                            GenExpr(item);
                        }
                        else
                        {
                            QAdd(",");
                            GenExpr(item);
                        }

                    }
                    QAdd(")");
                }
                MethodObjectType = "";
            }
            #endregion
            #region Object Constructor
            else if (expr is AstObjectConstructor)
            {
                AstObjectConstructor m = (AstObjectConstructor)expr;
                if (m.ObjectName == "Dictionary")
                {
                    if (m.Init.Count > 0)
                    {
                        QAdd("(function(){");
                        QAdd("var _x=new Dictionary();");
                        foreach (var dicMamma in m.Init)
                        {
                            QAdd("_x[");
                            GenExpr(dicMamma.Key);
                            QAdd("]=");
                            GenExpr(dicMamma.Value);
                            QAdd(";");
                        }
                        QAdd("return _x;})()");
                    }
                    else
                    {
                        QAdd("new Dictionary()");
                    }
                }
                else
                {
                    QAdd("new ");
                    QAdd(m.ObjectName);
                    QAdd("(");
                    bool isFirst = true;
                    foreach (Expr item in m.ParamList)
                    {

                        if (isFirst)
                        {
                            isFirst = false;
                            GenExpr(item);
                        }
                        else
                        {
                            QAdd(",");
                            GenExpr(item);
                        }

                    }
                    QAdd(")");
                }

            }
            #endregion
            #region Func
            else if (expr is Func)
            {
                Func f = (Func)expr;
                QAdd("func(");
                bool isFirst = true;
                foreach (var item in f.ParamNameList)
                {

                    if (isFirst)
                    {
                        isFirst = false;
                        QAdd(item);
                    }
                    else
                    {
                        QAdd(",");
                        QAdd(item);
                    }

                }
                QAdd(")");
                QAdd("{");
                GenStmt(f.Body);
                QAdd("}");
            }
            #endregion
            #region Delegate
            else if (expr is ASTDelegate)
            {
                ASTDelegate f = (ASTDelegate)expr;
                QAdd("function(");
                bool isFirst = true;
                foreach (var item in f.ParamNameList)
                {

                    if (isFirst)
                    {
                        isFirst = false;
                        QAdd(item.Replace("_s_", "$"));
                    }
                    else
                    {
                        QAdd(",");
                        QAdd(item.Replace("_s_", "$"));
                    }

                }
                QAdd(")");
                QAdd("{");
                GenStmt(f.Body);
                if (this.isBindWithThis)
                    QAdd("}.bind(this)");
                else
                    QAdd("}");
                if (f.HasSemi) QAdd(";");
            }
            #endregion
            #region DotLeft2
            else if (expr is DOTLeft2)
            {
                DOTLeft2 dot = (DOTLeft2)expr;
                MethodObjectType = lookup.GetVarType(scopeStack.Peek(), dot.Ident);
                QAdd(dot.Ident);
            }
            #endregion
            #region DOT2
            else if (expr is DOT2)
            {

                QAddF(".");
            }
            #endregion
            #region Variable
            else if (expr is Variable)
            {
                Variable v = (Variable)expr;
                if (MethodObjectType == "String" && v.Ident == "Length")
                {
                    QAdd("length");
                }
                else if (MethodObjectType == "List" && v.Ident == "Count")
                {
                    QAdd("length");
                }
                else if (v.Ident == "classx")
                {
                    QAdd("class");
                }
                else if (v.Ident == "eventx")
                {

                    QAdd("event");
                }
                else if (v.Ident == "paramsx")
                {

                    QAdd("params");
                }
                else if (v.Ident == "controllerx")
                {

                    QAdd("controller");
                }
                else if (v.Ident == "templateUrlx")
                {

                    QAdd("templateUrl");
                }
                else if (v.Ident == "Checked")
                {

                    QAdd("checked");
                }
                else
                {
                    QAdd(v.Ident.Replace("_s_", "$"));
                }
            }
            #endregion
            #region Parenthesis2
            else if (expr is Parenthesis2)
            {
                Parenthesis2 v = (Parenthesis2)expr;
                if (v.content != null||v.paramList!=null)
                {
                    if (isConstructorBase)
                    {

                        QAdd("(");
                        if (v.content != null)
                        {
                            GenExpr(v.content);
                        }
                        else if (v.paramList != null)
                        {
                            bool isFirst = true;
                            foreach (var item in v.paramList)
                            {
                                if (!isFirst) { QAdd(", "); }
                                GenExpr(item);
                                isFirst = false;
                            }

                        }

                        QAdd(")");
                    }
                    else
                    {
                        if (!(v.content is Variable))
                            QAdd("(");
                        if (v.content != null)
                        {
                            GenExpr(v.content);
                        }
                        else if (v.paramList != null)
                        {
                            bool isFirst = true;
                            foreach (var item in v.paramList)
                            {
                                if (!isFirst) { QAdd(", "); }
                                GenExpr(item);
                                isFirst = false;
                            }

                        }
                        if (!(v.content is Variable))
                            QAdd(")");
                        if (v.HasSemi) { Semi(); }
                    }
                }

            }
            #endregion
            #region NotExpr
            else if (expr is NotExpr)
            {
                NotExpr v = (NotExpr)expr;
                QAdd("!");
                GenExpr(v.Operand);
            }
            #endregion
            #region New2
            else if (expr is New2)
            {
                New2 v = (New2)expr;
                QAdd("new ");

            }
            #endregion
            #region JsonObject
            else if (expr is AstJsonObject)
            {
                AstJsonObject obj = expr as AstJsonObject;
                QAdd("{");
                bool isFirst = true;
                foreach (var item in obj.list)
                {
                    if (item.Ident == "controllerx")
                    {
                        item.Ident = "controller";
                    }
                    else if (item.Ident == "paramsx")
                    {
                        item.Ident = "params";
                    }
                    else if (item.Ident == "templateUrlx")
                    {
                        item.Ident = "templateUrl";

                    }
                    else if (item.Ident == "abstractx")
                    {
                        item.Ident = "abstract";
                    }
                    if (isFirst)
                    {
                        QAddF("{0}:", item.Ident);
                        isFirst = false;
                    }
                    else
                        QAddF(",{0}:", item.Ident);
                    GenExpr(item.Expr);
                }
                QAdd("}");

            }
            #endregion
            #region EmptyList
            else if (expr is AstEmptyList)
            {
                QAdd("[]");

            }
            #endregion
            #region JsonArrayList
            else if (expr is AstJsonArrey)
            {
                AstJsonArrey obj = expr as AstJsonArrey;
                QAdd("[");
                bool isFirst = true;
                foreach (var item in obj.list)
                {
                    if (isFirst)
                    {
                        GenExpr(item);
                        isFirst = false;
                    }
                    else
                    {
                        QAdd(",");
                        GenExpr(item);

                    }
                }
                isFirst = true;
                if (obj.ExprList != null)
                {
                    foreach (var item in obj.ExprList)
                    {
                        if (isFirst)
                        {
                            GenExpr(item);
                            isFirst = false;
                        }
                        else
                        {
                            QAdd(",");
                            GenExpr(item);

                        }
                    }
                }
                QAdd("]");

            }
            #endregion
            #region Linq
            else if (expr is AstLinq)
            {
                string res = result.ToString();
                result.Clear();
                AstLinq obj = expr as AstLinq;
                GenExpr(obj.InExp);
                string listName = result.ToString();
                result.Clear();
                if (obj.Join != null)
                {
                    GenExpr(obj.Join.InExp);
                    res += listName + ".selectWithJoin(" + result.ToString() + ", ";
                    result.Clear();
                    res += "function(" + obj.varName + ", " + obj.Join.varName + "){ ";
                    GenExpr(obj.Join.WhereExp);
                    res += "return " + result.ToString() + "; },";
                    result.Clear();
                    if (obj.WhereExp == null)
                    {
                        res += "function(){return !0;},";
                    }
                    else
                    {
                        res += "function(" + obj.varName + ", " + obj.Join.varName + "){return (";
                        GenExpr(obj.WhereExp);
                        res += result.ToString() + ");},";
                        result.Clear();
                    }
                    if (obj.SelectVarName != null)
                    {
                        GenExpr(obj.SelectVarName);
                        string select = result.ToString();
                        result.Clear();
                        res += "function(" + obj.varName + ", " + obj.Join.varName + "){return " + select + "; }";
                    }
                    else if (obj.SelectObject != null)
                    {
                        res += "function(" + obj.varName + ", " + obj.Join.varName + "){ return ";
                        GenExpr(obj.SelectObject);
                        res += result.ToString();
                        result.Clear();
                        res += "; }";

                    }
                }
                else
                {
                    res += listName + ".select(";
                    if (obj.WhereExp == null)
                    {
                        res += "function(){return !0;},";
                    }
                    else
                    {
                        res += "function(" + obj.varName + "){return (";
                        GenExpr(obj.WhereExp);
                        res += result.ToString() + ");},";
                        result.Clear();
                    }
                    if (obj.SelectVarName != null)
                    {
                        GenExpr(obj.SelectVarName);
                        string select = result.ToString();
                        result.Clear();
                        res += "function(" + obj.varName + "){return " + select + "; }";
                    }
                    else if (obj.SelectObject != null)
                    {
                        res += "function(" + obj.varName + "){ return ";
                        GenExpr(obj.SelectObject);
                        res += result.ToString();
                        result.Clear();
                        res += "; }";

                    }
                }
                res += ")";

                result.Append(res);
            }
            #endregion
            #region ArrayIndexing
            else if (expr is ASTArrayIndex2)
            {
                /* ASTArrayIndex2 arr = expr as ASTArrayIndex2;
                 string res = result.ToString();
                 result.Clear();
                 QAddF("{0}[", arr.Ident);
                 GenExpr(arr.index);
                 res += result.ToString() + "]";
                 result.Clear();
                 if (arr.isEqual)
                 {
                     res += "=";
                     GenExpr(arr.Expr);

                     res += result.ToString();
                     result.Clear();
                     QAdd(res);
                 }
                 else
                 {
                     QAdd(res);

                 }*/
                ASTArrayIndex2 arr = expr as ASTArrayIndex2;
                GenExpr(arr.LeftExpr);
                QAdd("[");
                GenExpr(arr.index);
                QAdd("]");
                if (arr.RightExpr != null)
                    GenExpr(arr.RightExpr);

            }
            #endregion
            #region What
            else if (expr is ASTWhat)
            {
                ASTWhat what = expr as ASTWhat;
                GenExpr(what.Left);
                QAdd(" ? ");
                GenExpr(what.Expr1);
                QAdd(" : ");
                GenExpr(what.Expr2);

            }
            #endregion
            #region Lembda
            else if (expr is ASTLembda)
            {
                ASTLembda f = expr as ASTLembda;
                QAdd("function(");
                bool isFirst = true;
                foreach (var item in f.ParamNameList)
                {

                    if (isFirst)
                    {
                        isFirst = false;
                        QAdd(item.Replace("_s_", "$"));
                    }
                    else
                    {
                        QAdd(",");
                        QAdd(item.Replace("_s_", "$"));
                    }

                }
                QAdd(")");
                QAdd("{");
                if (f.singleBody != null)
                {
                    QAdd("return ");
                    this.GenExpr(f.singleBody);
                    QAdd(";");
                }
                else
                {
                    GenStmt(f.Body);
                }
                if (this.isBindWithThis)
                    QAdd("}.bind(this)");
                else
                    QAdd("}");
                if (f.HasSemi) QAdd(";");
            }
            #endregion

            else if (expr is AstJSExpr)
            {
                AstJSExpr f = expr as AstJSExpr;
                GenExpr(f.LeftExpr);
                QAdd(f.JScript);
            }
            else if (expr is PostIncrementOpExpr)
            {
                PostIncrementOpExpr f = expr as PostIncrementOpExpr;
                QAdd(f.Ident);
                QAdd("++");
            }
            else if (expr is PostDecrementOpExpr)
            {
                PostDecrementOpExpr f = expr as PostDecrementOpExpr;
                QAdd(f.Ident);
                QAdd("--");
            }
            else if (expr is PreIncrementOpExpr)
            {
                PreIncrementOpExpr f = expr as PreIncrementOpExpr;
                QAdd("++");
                GenExpr(f.expr);

            }
            else if (expr is PreDecrementOpExpr)
            {
                PreDecrementOpExpr f = expr as PreDecrementOpExpr;
                QAdd("--");
                GenExpr(f.expr);

            }
        }

        #region Helper
        private void Semi() { this.result.Append(";"); this.result.Append(NEW_LINE); }
        private void Semi(Stmt stmt) { if (stmt.HasSemi) this.result.Append(";"); MethodObjectType = ""; this.result.Append(NEW_LINE); }
        private void QAdd(string str) { this.result.Append(str); }
        private void NewLine() { this.result.Append(NEW_LINE); }
        private void QAddF(string str, params object[] args) { this.result.AppendFormat(str, args); }
        private string GetOPName(BinOp op)
        {
            if (op == BinOp.Add) return "+";
            if (op == BinOp.Div) return "/";
            if (op == BinOp.GT) return ">";
            if (op == BinOp.GTE) return ">=";
            if (op == BinOp.LAND) return "&&";
            if (op == BinOp.LE) return "==";
            if (op == BinOp.LOR) return "||";
            if (op == BinOp.LT) return "<";
            if (op == BinOp.LTE) return "<=";
            if (op == BinOp.Mul) return "*";
            if (op == BinOp.NE) return "!=";
            if (op == BinOp.Sub) return "-";
            if (op == BinOp.MOD) return "%";
            if (op == BinOp.JoinEquals) return "==";

            if (op == BinOp.ADD_EQUAL) return "+=";
            if (op == BinOp.SUB_EQUAL) return "-=";
            if (op == BinOp.MUL_EQUAL) return "*=";
            if (op == BinOp.DIV_EQUAL) return "/=";

            if (op == BinOp.MOD_EQUAL) return "%=";
            if (op == BinOp.AND_EQUAL) return "&=";
            if (op == BinOp.OR_EQUAL) return "|=";
            return "";
        }
        #endregion

        public SNamespace AstNamespace { get; set; }
    }

    public class Javascript
    {
        private bool isFirst = true;
        public Javascript()
        {
            ClassName = "";
            PrivateMemberVariable = new StringBuilder();
            PublicMemberVariable = new StringBuilder();
            PrivateMemberFunction = new StringBuilder();
            PublicMemberFunction = new StringBuilder();
            InitMethodHelper = new StringBuilder();
            InitMethodParams = new StringBuilder();
        }
        public SNamespace AstNamespace { get; set; }
        public bool IsAngular { get; set; }
        public String InhritedBy { get; set; }
        public String Namespace { get; set; }
        public String ClassName { get; set; }
        public StringBuilder InitMethodHelper { get; set; }
        public StringBuilder PrivateMemberVariable { get; set; }
        public StringBuilder PublicMemberVariable { get; set; }
        public StringBuilder PrivateMemberFunction { get; set; }
        public StringBuilder PublicMemberFunction { get; set; }
        public StringBuilder InitMethodParams { get; set; }
        public override string ToString()
        {
            StringBuilder js = new StringBuilder();
            js.AppendLine();
            if (InhritedBy == "BaseDirective") {
                InhritedBy = "jsClass";
            }
            if (InhritedBy == "jsClass" || InhritedBy.IndexOf(".") >= 0)
                js.AppendFormat("var {0}={1}.extend(", ClassName, InhritedBy);
            else
                js.AppendFormat("var {0}={1}.extend(", ClassName, Namespace + "." + InhritedBy);


            js.Append('{' + Environment.NewLine);
            string properties = PublicMemberVariable.ToString();
            //js.Append(PublicMemberVariable);
            string content = PublicMemberFunction.ToString();

            content = content.Replace("$INIT$", IsAngular ? InitMethodHelper.ToString() : "");
            if (content.Length > 3)
            {
                content = content.Remove(content.Length - 3);
            }
            else if (properties.Length > 3)
            {

                properties = properties.Substring(0, properties.Length - 3);
            }
            js.Append(properties);
            js.Append(content);


            js.Append("});");
            js.AppendLine();
            if (AstNamespace.ActionType != "directive")
            {
                js.AppendFormat("namespace('{0}.{1}',{1});", Namespace, ClassName);
            }
            string fxParams = this.InitMethodParams.ToString();
            js.AppendLine();
            //js.AppendFormat("namespace('{0}.{1}_ng',{2});", Namespace, ClassName, "function(" + fxParams + "){return new " + ClassName + "(" + fxParams + ");}");
            //angular.module('app', [])
            if (!String.IsNullOrEmpty(AstNamespace.ModuleName))
            {
                if (!String.IsNullOrEmpty(AstNamespace.ActionName) && !String.IsNullOrEmpty(AstNamespace.ActionType))
                {
                    switch (AstNamespace.ActionType)
                    {
                        case "run":
                        case "config":
                            js.AppendLine();
                            js.AppendFormat("angular.module('{0}')", AstNamespace.ModuleName);
                            if (!String.IsNullOrEmpty(AstNamespace.DI))
                            {
                                js.AppendFormat(".{0}({1});", AstNamespace.ActionType, "function(" + AstNamespace.DI + "){ new " + ClassName + "(" + AstNamespace.DI + ");}");
                            }
                            else
                            {
                                js.AppendFormat(".{0}({1});", AstNamespace.ActionType, "function(" + fxParams + "){ new " + ClassName + "(" + fxParams + ");}");
                            }


                            break;
                        case "controller":
                        case "service":
                            js.AppendLine();
                            js.AppendFormat("angular.module('{0}')", AstNamespace.ModuleName);
                            //js.AppendLine();
                            if (String.IsNullOrEmpty(AstNamespace.DI))
                            {
                                js.AppendFormat(".{0}('{1}',[{2} {3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(fxParams), ClassName);
                                // js.AppendFormat(".{0}('{1}',{2});", AstNamespace.ActionType, AstNamespace.ActionName, ClassName);
                            }
                            else
                            {

                                js.AppendFormat(".{0}('{1}',[{2} {3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(AstNamespace.DI), ClassName);
                            }
                            break;
                        case "directive":
                            /* StringBuilder dir = new StringBuilder();
                             var fxes = "var $$$006={" + properties + content + "};";

                             if (!string.IsNullOrEmpty(fxParams))
                             {
                                 string[] arr = fxParams.Split(new char[] { ',' });
                                 for (int i = 0; i < arr.Length; i++)
                                 {
                                     if (!string.IsNullOrEmpty(arr[i]))
                                     {
                                         fxes = fxes.Replace("this." + arr[i], arr[i]);
                                     }
                                 }
                             }
                             fxes = fxes.Replace("this.", "$$$006.");
                             fxes += Environment.NewLine + "return $$$006;";
                             dir.AppendLine();
                             dir.AppendFormat("angular.module('{0}')", AstNamespace.ModuleName);
                             //js.AppendLine();
                             if (String.IsNullOrEmpty(AstNamespace.DI))
                             {
                                 dir.AppendFormat(".{0}('{1}',[{2}{3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(fxParams), "function(" + fxParams + "){ " + fxes + "}");
                             }
                             else
                             {
                                 dir.AppendFormat(".{0}('{1}',[{2}{3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(AstNamespace.DI), "function(" + fxParams + "){ " + fxes + "}");
                             }
                             return dir.ToString();
                             */
                            StringBuilder dir = new StringBuilder();
                            dir.AppendLine();
                            dir.AppendFormat("angular.module('{0}')", AstNamespace.ModuleName);
                            if (String.IsNullOrEmpty(AstNamespace.DI))
                            {
                                dir.AppendFormat(".{0}('{1}',[{2}{3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(fxParams), "function(" + fxParams + "){ " + js.ToString() + Environment.NewLine + "return new " + ClassName + "(" + fxParams + ").getDirective();}");
                            }
                            else
                            {
                                dir.AppendFormat(".{0}('{1}',[{2}{3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(AstNamespace.DI), "function(" + fxParams + "){ " + js.ToString() + Environment.NewLine + "return new " + ClassName + "(" + fxParams + ").getDirective();}");
                            }
                            return dir.ToString();
                        case "factory":
                            js.AppendLine();
                            js.AppendFormat("angular.module('{0}')", AstNamespace.ModuleName);
                            //js.AppendLine();
                            if (String.IsNullOrEmpty(AstNamespace.DI))
                            {
                                js.AppendFormat(".{0}('{1}',[{2}{3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(fxParams), "function(" + fxParams + "){ return new " + ClassName + "(" + fxParams + ");}");
                            }
                            else
                            {
                                js.AppendFormat(".{0}('{1}',[{2}{3}]);", AstNamespace.ActionType, AstNamespace.ActionName, ModifyDI(AstNamespace.DI), "function(" + fxParams + "){ return new " + ClassName + "(" + fxParams + ");}");
                            }
                            break;
                        default:
                            throw new Exception("Angular attribute encountered invalid param's value");

                    }
                }
            }
            return js.ToString();
        }
        private string ModifyDI(string di)
        {
            if (string.IsNullOrEmpty(di)) return "";
            string[] arr = di.Split(new char[] { ',' });
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                sb.AppendFormat("'{0}',", arr[i]);
            }
            return sb.ToString();
        }
        public void AddVariable(string var)
        {
            if (isFirst)
            {
                PrivateMemberVariable.Append(var);
                isFirst = false;
            }
            else
            {
                PrivateMemberVariable.Append("," + var);
            }
        }

    }
}
