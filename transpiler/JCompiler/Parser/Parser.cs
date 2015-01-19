using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Ast;
using JCompiler.Tokens;
using JCompiler.Utilities;

namespace JCompiler.Parser
{
    public partial class Parser
    {
        private LookupService lookupTable = new LookupManager();
        private int index = 0;
        private IList<Token> tokens = null;
        private readonly Stmt astResult;
        private Token mToken;
        private int tokenCount = 0;

        private string currentClass = "r2";
        private int parenthesisCounter = 0;
        private Function currentMethod;
        private string Namespace = "";



        public Stmt Result
        {
            get { return astResult; }
        }
        public Parser(IList<Token> tokens)
        {
            this.tokens = tokens;
            this.index = 0;
            this.tokenCount = tokens.Count;
            this.astResult = this.ParseStmt();

            if (this.index != this.tokens.Count)
            {
                if (mToken != null)
                    throw new System.Exception("expected EOF at line number :" + mToken.LineNumber);
            }
        }

        private Stmt ParseStmt()
        {
            Stmt result = null;
            if (tokens[index] is TStringLiteral)
            {
                mToken = tokens[index] as TStringLiteral;
                if (IsDataMember(index))
                {
                    DeclareVar declareVar = new DeclareVar();
                    VariableDiclaration(declareVar);
                    result = declareVar;
                }
                else if (IsFunctionMember(index))
                {

                    throw new System.Exception("Function Member Declaration must start with a Modifier(public, private, etc). at Line No:" + mToken.LineNumber);
                }
                else if (tokens[index + 1] is TEqual)
                {
                    Assign assign = new Assign();
                    assign.Ident = GetString(mToken);
                    this.index += 2;
                    assign.Expr = this.ParseExpr();
                    result = assign;
                }
                #region AddEqual
                else if (this.tokens[this.index + 1] is TAddEqual)
                {
                    AddEqual plusEqual = new AddEqual();
                    plusEqual.Ident = GetString(mToken);
                    this.index += 2;
                    plusEqual.Expr = ParseExpr();
                    result = plusEqual;
                    if (!(this.tokens[this.index] is TSemi))
                    {
                        throw new System.Exception("Incremental Statement " + plusEqual.Ident + " +=.. missing ';'. Line Number:" + mToken.LineNumber);
                    }

                }
                #endregion
                #region SubEqual
                else if (this.tokens[this.index + 1] is TSubEqual)
                {
                    SubEqual plusEqual = new SubEqual();
                    plusEqual.Ident = GetString(mToken);
                    this.index += 2;
                    plusEqual.Expr = ParseExpr();
                    result = plusEqual;
                    if (!(this.tokens[this.index] is TSemi))
                    {
                        throw new System.Exception("Incremental Statement " + plusEqual.Ident + " +=.. missing ';'. Line Number:" + mToken.LineNumber);
                    }

                }
                #endregion
                #region MulEqual
                else if (this.tokens[this.index + 1] is TMulEqual)
                {
                    MulEqual plusEqual = new MulEqual();
                    plusEqual.Ident = GetString(mToken);
                    this.index += 2;
                    plusEqual.Expr = ParseExpr();
                    result = plusEqual;
                    if (!(this.tokens[this.index] is TSemi))
                    {
                        throw new System.Exception("Incremental Statement " + plusEqual.Ident + " +=.. missing ';'. Line Number:" + mToken.LineNumber);
                    }

                }
                #endregion
                #region DivEqual
                else if (this.tokens[this.index + 1] is TDivEqual)
                {
                    DivEqual plusEqual = new DivEqual();
                    plusEqual.Ident = GetString(mToken);
                    this.index += 2;
                    plusEqual.Expr = ParseExpr();
                    result = plusEqual;
                    if (!(this.tokens[this.index] is TSemi))
                    {
                        throw new System.Exception("Incremental Statement " + plusEqual.Ident + " +=.. missing ';'. Line Number:" + mToken.LineNumber);
                    }

                }
                #endregion

                #region Increment
                else if (this.tokens[this.index + 1] is TIncrement)
                {
                    PostIncrementOp inc = new PostIncrementOp();
                    inc.Ident = GetString(mToken);
                    result = inc;
                    this.index += 2;
                }
                #endregion
                #region DEcrement
                else if (this.tokens[this.index + 1] is TDecrement)
                {
                    PostDecrementOp dec = new PostDecrementOp();
                    dec.Ident = GetString(mToken);
                    result = dec;
                    this.index += 2;

                }
                #endregion
                else
                {
                    ASTComplexStmt com = new ASTComplexStmt();
                    com.LeftExpression = ParseExpr();
                    if (tokens[index] is TEqual)
                    {
                        index++;
                        com.RightExpression = ParseExpr();
                    }
                    result = com;
                }
            }
            else if (tokens[index] is TList)
            {
                DeclareVar declareVar = new DeclareVar();
                VariableDiclaration(declareVar);
                result = declareVar;
            }
            else if (tokens[index] is TDictionary)
            {
                DeclareVar declareVar = new DeclareVar();
                VariableDiclaration(declareVar);
                result = declareVar;
            }
            else if (tokens[index] is TFunc)
            {
                DeclareVar declareVar = new DeclareVar();
                VariableDiclaration(declareVar);
                result = declareVar;
            }
            #region Increment
            else if (this.tokens[this.index] is TIncrement)
            {
                ASTComplexStmt com = new ASTComplexStmt();

                com.LeftExpression = ParseExpr();
                if (tokens[index] is TEqual)
                {
                    index++;
                    com.RightExpression = ParseExpr();
                }
                result = com;
            }
            #endregion
            #region DEcrement
            else if (this.tokens[this.index] is TDecrement)
            {
                ASTComplexStmt com = new ASTComplexStmt();
                com.LeftExpression = ParseExpr();
                if (tokens[index] is TEqual)
                {
                    index++;
                    com.RightExpression = ParseExpr();
                }
                result = com;
            }
            #endregion
            #region Return
            else if (this.tokens[this.index] is TReturn)
            {
                mToken = this.tokens[this.index] as TReturn;
                //chequeClassName("Invalid token 'return' outside class. Line number  " + mToken.LineNumber);
                //chequeMethod("Invalid token 'return' in class. Line number  " + mToken.LineNumber);
                Return retn = new Return();
                this.index++;
                retn.Expr = ParseExpr();
                result = retn;

                if (!(this.tokens[this.index] is TSemi))
                {
                    throw new System.Exception("Missing ';' at Line Number:" + mToken.LineNumber);
                }
            }
            #endregion

            #region MODIFIER PUBLIC PROTECTED PRIVATE

            else if (tokens[index] is TModifier)
            {
                mToken = tokens[index] as TModifier;
                #region IsClass
                if (IsClass(index))
                {
                    index++;
                    mToken = this.tokens[this.index] as TClass;
                    this.index++;

                    result = ParseClass(GetString(this.tokens[this.index]));
                    if (!(this.tokens[this.index] is TEndBrace))
                    {
                        throw new System.Exception("expected end brace '}' to end a Class body, Line Number: " + mToken.LineNumber + "\nDo not forget to assaign a value when you declare a variable.");
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
                #endregion
                else if (IsConstructor(index))
                {
                    parenthesisCounter = 0;
                    mToken = this.tokens[this.index] as TModifier;
                    MethodInfo methodInfo = new MethodInfo();
                    methodInfo.MethodName = "init";
                    methodInfo.ModifierName = "public";
                    methodInfo.Index = index + 3;
                    result = ParseFunction(methodInfo);
                    if (!(this.tokens[this.index] is TEndBrace))
                    {
                        throw new System.Exception("expected '}' to end function body, Line Number: " + mToken.LineNumber);
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
                else if (IsDataMember(index + 1))
                {
                    TModifier modifier = tokens[index] as TModifier;
                    mToken = modifier;
                    index++;
                    DeclareVar declareVar = new DeclareVar();
                    declareVar.ModifierName = modifier.Name;
                    VariableDiclaration(declareVar);
                    result = declareVar;
                }
                else if (IsFunctionMember(index + 1))
                {
                    parenthesisCounter = 0;
                    mToken = this.tokens[this.index] as TModifier;
                    result = ParseFunction(GetMethodInfo());
                    if (!(this.tokens[this.index] is TEndBrace))
                    {
                        throw new System.Exception("expected end brace '}' to end a function body, Line Number: " + mToken.LineNumber + "\nDo not forget to assaign a value when you declare a variable.");
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
                else if (IsList(index))
                {

                    TModifier modifier = tokens[index] as TModifier;
                    index++;
                    DeclareVar declareVar = new DeclareVar();
                    declareVar.ModifierName = modifier.Name;
                    VariableDiclaration(declareVar);
                    result = declareVar;
                }
                else if (IsDictionary(index))
                {
                    TModifier modifier = tokens[index] as TModifier;
                    index++;
                    DeclareVar declareVar = new DeclareVar();
                    declareVar.ModifierName = modifier.Name;
                    VariableDiclaration(declareVar);
                    result = declareVar;
                }
                else if (tokens[index + 1] is TEnum)
                {
                    TModifier modifier = tokens[index] as TModifier;
                    index++;
                    DeclareVar declareVar = new DeclareVar();
                    declareVar.ModifierName = modifier.Name;
                    VariableDiclarationForEnum(declareVar);
                    result = declareVar;
                }
                else
                {
                    throw new System.Exception("Member declaration is not correct at line no " + mToken.LineNumber + "\nDo not forget to assaign a value when you declare a variable.");
                }

            }
            #endregion

            #region Using Namespace
            else if (tokens[index] is TUsing)
            {
                do { index++; } while (!(tokens[index] is TSemi));
                result = new SUsing();
            }
            else if (tokens[index] is TNamespace)
            {
                SNamespace u = new SNamespace();
                index++;
                u.Name = "";
                do
                {
                    if (this.tokens[this.index] is TDot)
                        u.Name += ".";
                    else
                        u.Name += ((TStringLiteral)this.tokens[this.index]).Value;
                    this.index++;
                } while (!(this.tokens[this.index] is TBeginBrace));
                Namespace = u.Name;
                result = u;
                this.index++;
                if (tokens[index] is TBeginBraket)
                {
                    // [Angular(ModuleName="app", ActionType="controller", ActionName="test", DI="$scope")]
                    while (!(tokens[index] is TEndBraket))
                    {
                        if (tokens[index] is TStringLiteral)
                        {
                            switch (GetString(tokens[index]))
                            {
                                case "ModuleName":
                                    index += 2;
                                    if (tokens[index] is TStringInQuotation)
                                    {
                                        u.ModuleName = (tokens[index] as TStringInQuotation).Value;
                                    }
                                    break;
                                case "ActionType":
                                    index += 2;
                                    if (tokens[index] is TStringInQuotation)
                                    {
                                        u.ActionType = (tokens[index] as TStringInQuotation).Value;
                                    }
                                    break;
                                case "ActionName":
                                    index += 2;
                                    if (tokens[index] is TStringInQuotation)
                                    {
                                        u.ActionName = (tokens[index] as TStringInQuotation).Value;
                                    }
                                    break;
                                case "DI":
                                    index += 2;
                                    if (tokens[index] is TStringInQuotation)
                                    {
                                        u.DI = (tokens[index] as TStringInQuotation).Value;
                                    }
                                    break;
                            }
                        }
                        index++;
                    }
                    index++;

                }
                if (this.index < this.tokens.Count)
                {
                    StmtSequence sequence = new StmtSequence();
                    sequence.First = result;
                    sequence.Second = this.ParseStmt();
                    result = sequence;
                }

                if (this.index == this.tokens.Count || !(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected ending brace at line number:" + mToken.LineNumber);
                }
                this.index++;
            }
            #endregion

            #region IF
            else if (this.tokens[this.index] is TIf)
            {
                mToken = this.tokens[this.index] as TIf;
                chequeClassName("Invalid token 'if' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'if' in class. Line number  " + mToken.LineNumber);
                this.index++;
                IF astif = new IF();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TBeginParen))
                {
                    throw new System.Exception("expected '(' after 'if'. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.tokens[this.index] is TEndParen)
                {
                    throw new System.Exception("if missing conditional part'. Line number " + mToken.LineNumber);
                }
                astif.Condition = ParseExpr();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("expected ')' after conditional part of 'if' . Line number " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TBeginBrace))
                {
                    throw new System.Exception("expected '{' to start if body, Line Number: " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TEndBrace))
                    astif.Body = ParseStmt();
                result = astif;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end if body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Elseif
            else if (this.tokens[this.index] is TElseIf)
            {
                mToken = this.tokens[this.index] as TElseIf;
                chequeClassName("Invalid token 'elseif' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'elseif' in class. Line number  " + mToken.LineNumber);
                this.index++;
                ElseIF elseif = new ElseIF();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TBeginParen))
                {
                    throw new System.Exception("expected '(' after 'elseif'. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.tokens[this.index] is TEndParen)
                {
                    throw new System.Exception("elseif missing conditional part'. Line number " + mToken.LineNumber);
                }
                elseif.Condition = ParseExpr();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("expected ')' after conditional part of 'elseif' . Line number " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TBeginBrace))
                {
                    throw new System.Exception("expected '{' to start else if body, Line Number: " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TEndBrace))
                    elseif.Body = ParseStmt();
                result = elseif;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end elseif body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Else
            else if (this.tokens[this.index] is TElse)
            {
                mToken = this.tokens[this.index] as TElse;
                chequeClassName("Invalid token 'else' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'else' in class. Line number  " + mToken.LineNumber);
                this.index++;
                ELSE astelse = new ELSE();

                if (!(this.tokens[this.index] is TBeginBrace))
                {
                    throw new System.Exception("expected '{' to start else  body, Line Number: " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TEndBrace))
                    astelse.Body = ParseStmt();
                result = astelse;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end else body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Switch
            else if (this.tokens[this.index] is TSwitch)
            {
                mToken = this.tokens[this.index] as TSwitch;
                chequeClassName("Invalid token 'switch' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'switch' in class. Line number  " + mToken.LineNumber);
                this.index++;
                Switch stch = new Switch();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TBeginParen))
                {
                    throw new System.Exception("expected '(' after 'switch'. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.tokens[this.index] is TEndParen)
                {
                    throw new System.Exception("switch missing conditional part'. Line number " + mToken.LineNumber);
                }
                stch.Expr = ParseExpr();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("expected ')' after conditional part of 'switch' . Line number " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TBeginBrace))
                {
                    throw new System.Exception("expected '{' to start switch body, Line Number: " + mToken.LineNumber);
                }
                this.index++;
                stch.Body = ParseStmt();
                result = stch;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end switch body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Case
            else if (this.tokens[this.index] is TCase)
            {

                mToken = this.tokens[this.index] as TCase;
                chequeClassName("Invalid token 'case' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'case' in class. Line number  " + mToken.LineNumber);
                this.index++;
                Case acase = new Case();
                acase.ident = GetCaseValue(this.tokens[this.index]);
                result = acase;
                this.index++;
                if (!(this.tokens[this.index] is TClone))
                {
                    throw new System.Exception("Missing ':' in case statement at Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Case Default
            else if (this.tokens[this.index] is TDefault)
            {
                mToken = this.tokens[this.index] as TDefault;
                chequeClassName("Invalid token 'default' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'default' in class. Line number  " + mToken.LineNumber);
                this.index++;
                Default acase = new Default();
                result = acase;

                if (!(this.tokens[this.index] is TClone))
                {
                    throw new System.Exception("Missing ':' in default statement at Line Number: " + mToken.LineNumber);
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
            #endregion
            #region While Loop
            else if (this.tokens[this.index] is TWhile)
            {
                mToken = this.tokens[this.index] as TWhile;
                chequeClassName("Invalid token 'while' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'while' in class. Line number  " + mToken.LineNumber);
                this.index++;
                WhileLoop whileLoop = new WhileLoop();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TBeginParen))
                {
                    throw new System.Exception("expected '(' after 'while loop'. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.tokens[this.index] is TEndParen)
                {
                    throw new System.Exception("While loop missing conditional part'. Line number " + mToken.LineNumber);
                }
                whileLoop.Condition = ParseExpr();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("expected ')' after conditional part of 'while' loop. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TBeginBrace))
                {
                    throw new System.Exception("expected '{' to start while loop body, Line Number: " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TEndBrace))
                    whileLoop.Body = ParseStmt();
                result = whileLoop;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end while loop body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region For Loop
            else if (this.tokens[this.index] is TFor)
            {
                mToken = this.tokens[this.index] as TFor;
                chequeClassName("Invalid token 'for' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'for' in class. Line number  " + mToken.LineNumber);
                this.index++;
                ForLoop forLoop = new ForLoop();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TBeginParen))
                {
                    throw new System.Exception("expected '(' after 'for'. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.index < this.tokens.Count && ((this.tokens[this.index] is TInt) || (this.tokens[this.index] is TStringLiteral)))
                {
                    forLoop.Initialize = parseStmtInitForLoop();
                }
                else if (this.index < this.tokens.Count && !(this.tokens[this.index] is TSemi))
                {
                    throw new System.Exception("expected ';' after initialize part of 'for' loop. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.tokens[this.index] is TEndParen)
                {
                    throw new System.Exception("For loop missing conditional part'. Line number " + mToken.LineNumber);
                }
                forLoop.Condition = ParseExpr();
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TSemi))
                {
                    throw new System.Exception("expected ';' after initialize part of 'for' loop. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (this.index < this.tokens.Count && ((this.tokens[this.index] is TStringLiteral)
                    || (this.tokens[this.index] is TIncrement) || (this.tokens[this.index] is TDecrement)
                    || (this.tokens[this.index] is TAddEqual)
                    || (this.tokens[this.index] is TSubEqual)
                    || (this.tokens[this.index] is TDivEqual)
                    || (this.tokens[this.index] is TMulEqual)
                    ))
                {
                    forLoop.Increment = parseStmtIncrementForLoop();
                    if (forLoop.Increment is AddEqual
                    || (forLoop.Increment is SubEqual)
                    || (forLoop.Increment is DivEqual)
                    || (forLoop.Increment is MulEqual)
                        )
                    {
                        if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndParen))
                        {
                            throw new System.Exception("expected ')' after conditional part of 'for' loop. Line number " + mToken.LineNumber);
                        }
                    }
                    else
                    {
                        this.index++;
                    }
                }
                else if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("expected ')' after conditional part of 'for' loop. Line number " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TBeginBrace))
                {
                    throw new System.Exception("expected '{' to start for loop body, Line Number: " + mToken.LineNumber);
                }
                this.index++;
                if (!(this.tokens[this.index] is TEndBrace))
                    forLoop.Body = ParseStmt();
                result = forLoop;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end for loop body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Foreach
            else if (this.tokens[this.index] is TForeach)
            {
                mToken = this.tokens[this.index] as TWhile;
                // chequeClassName("Invalid token 'while' outside class. Line number  " + mToken.LineNumber);
                // chequeMethod("Invalid token 'while' in class. Line number  " + mToken.LineNumber);
                this.index++;
                ForEach each = new ForEach();
                do { this.index++; } while (!(this.tokens[this.index] is TIn));
                this.index--;
                each.varName = GetString(this.tokens[this.index]);
                this.index += 2;
                each.Source = ParseExpr();
                this.index += 2;
                if (!(this.tokens[this.index] is TEndBrace))
                    each.Body = ParseStmt();
                result = each;

                if (!(this.tokens[this.index] is TEndBrace))
                {
                    throw new System.Exception("expected '}' to end while loop body, Line Number: " + mToken.LineNumber);
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
            #endregion
            #region Continue
            else if (this.tokens[this.index] is TContinue)
            {
                mToken = this.tokens[this.index] as TContinue;
                chequeClassName("Invalid token 'continue' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'continue' in class. Line number  " + mToken.LineNumber);
                Continue cc = new Continue();
                this.index++;
                result = cc;
                if (!(this.tokens[this.index] is TSemi))
                {
                    throw new System.Exception("Missing ';' at Line Number:" + mToken.LineNumber);
                }
            }
            #endregion
            #region Break
            else if (this.tokens[this.index] is TBreak)
            {
                mToken = this.tokens[this.index] as TBreak;
                chequeClassName("Invalid token 'break' outside class. Line number  " + mToken.LineNumber);
                chequeMethod("Invalid token 'break' in class. Line number  " + mToken.LineNumber);
                Break bb = new Break();
                this.index++;
                result = bb;
                if (!(this.tokens[this.index] is TSemi))
                {
                    throw new System.Exception("Missing ';' at Line Number:" + mToken.LineNumber);
                }
            }
            #endregion
            #region Super Method
            else if (this.tokens[this.index] is TBase)
            {
                mToken = this.tokens[this.index] as TBase;
                string x = currentMethod.FunctionName;
                this.index++;
                if (!(this.tokens[this.index] is TDot))
                {
                    throw new System.Exception("parse error at token " + this.index + ": Line Number:" + mToken.LineNumber);
                }
                this.index++;
                if (GetString(this.tokens[index]) != currentMethod.FunctionName)
                {
                    throw new System.Exception("base keyword is only applicable for calling super version of current method at Line Number:" + mToken.LineNumber);
                }

                AstBase astBase = new AstBase();

                MethodCall mc = new MethodCall();
                mc.LineNo = mToken.LineNumber;
                mc.MethodName = "_super";
                //mc.ReturnType = lookupTable.GetFunctionReturnType(currentClass, mc.MethodName, mToken.LineNumber);
                //mc.ParamTypeList = lookupTable.GetFunctionParamJVMType(currentClass, mc.MethodName);
                mc.BeginParen = "(";
                this.index += 2;
                int c = -1;
                while (!(this.tokens[this.index] is TEndParen))
                {
                    if (!(this.tokens[this.index] is TComma))
                    {
                        if (this.tokens[this.index] is TFunc)
                            mc.paramList.Add(FuncMethodCall());
                        else
                            mc.paramList.Add(ParseExpr());
                        c++;
                    }
                    else
                    {
                        this.index++;
                        c--;
                    }
                }
                if (mc.paramList.Count > 0 && c != 0)
                {
                    throw new System.Exception(msg("Method calling '{0}' at line number:{1} is invalid", mc.MethodName, mToken.LineNumber));
                }
                if (this.index == this.tokens.Count || !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("...expected ending Parenthesis at line number:" + mToken.LineNumber);
                }
                mc.EndParen = ")";
                this.index++;
                astBase.Method = mc;
                result = astBase;

            }
            #endregion
            #region Comment for JS
            else if (this.tokens[this.index] is TJS)
            {
                AstJS js = new AstJS();
                js.JScript = ((TJS)this.tokens[this.index]).JS;
                this.index++;
                if (this.tokens[index] is TEndBrace)
                {
                    return js;
                }
                else
                {
                    StmtSequence sequence = new StmtSequence();
                    sequence.First = js;
                    sequence.Second = this.ParseStmt();
                    result = sequence;
                }
            }
            #endregion
            #region String Quatation
            else if (this.tokens[this.index] is TStringInQuotation)
            {
                ASTComplexStmt com = new ASTComplexStmt();
                com.LeftExpression = ParseExpr();
                if (tokens[index] is TEqual)
                {
                    index++;
                    com.RightExpression = ParseExpr();
                }
                result = com;
            }
            #endregion
            else if (IsClass(index - 1))
            {
                throw new System.Exception("Class Declaration must start with a Modifier(public, etc) ");
            }
            else if (this.tokens[this.index] is TBeginParen)
            {
                ASTComplexStmt com = new ASTComplexStmt();
                com.LeftExpression = ParseExpr();
                if (tokens[index] is TEqual)
                {
                    index++;
                    com.RightExpression = ParseExpr();
                }
                result = com;
            }

            else if (this.tokens[this.index] is TDot)
            {
                mToken = this.tokens[this.index] as TDot;
                //chequeClassName("Invalid token '.' outside class. Line number  " + mToken.LineNumber);
                //chequeMethod("Invalid token '.' in class. Line number  " + mToken.LineNumber);

                DOT bb = new DOT();
                this.index++;
                result = bb;
                if (this.index < this.tokens.Count)
                {
                    StmtSequence sequence = new StmtSequence();
                    sequence.First = result;
                    sequence.Second = this.ParseStmt();
                    result = sequence;
                }
            }
            else if (tokens[index] is TNew)
            {
                ASTComplexStmt com = new ASTComplexStmt();
                com.LeftExpression = ParseExpr();
                if (tokens[index] is TEqual)
                {
                    index++;
                    com.RightExpression = ParseExpr();
                }
                result = com;
            }
            else if (tokens[index] is TEnum)
            {

                DeclareVar declareVar = new DeclareVar();
                declareVar.ModifierName = "public";
                VariableDiclarationForEnum(declareVar);
                result = declareVar;
            }
            if (this.index < this.tokenCount && this.tokens[this.index] is TSemi)
            {
                this.index++;
                result.HasSemi = true;
                if (this.index < this.tokens.Count && !(this.tokens[this.index] is TEndBrace))
                {
                    StmtSequence sequence = new StmtSequence();
                    sequence.First = result;
                    sequence.Second = this.ParseStmt();
                    result = sequence;
                }
            }


            return result;
        }

        private Expr ParseExpr()
        {
            Expr result = null;
            if (this.index == this.tokens.Count)
            {
                throw new System.Exception("expected expression, got EOF");
            }

            else if (this.tokens[this.index] is TStringLiteral)
            {
                mToken = this.tokens[this.index++] as TStringLiteral;
                #region Method Calling
                if (IsMethodCall(index))
                {
                    MethodCall2 mc = new MethodCall2();
                    mc.LineNo = mToken.LineNumber;
                    mc.MethodName = GetString(mToken);

                    if (tokens[index] is TLT)
                    {
                        this.index = ResolveLTGT(this.index);
                        index++;
                    }
                    //mc.ReturnType = lookupTable.GetFunctionReturnType(currentClass, mc.MethodName, mToken.LineNumber);
                    //mc.ParamTypeList = lookupTable.GetFunctionParamJVMType(currentClass, mc.MethodName);
                    mc.BeginParen = "(";
                    this.index++;
                    int c = -1;
                    while (!(this.tokens[this.index] is TEndParen))
                    {
                        if (!(this.tokens[this.index] is TComma))
                        {
                            if (this.tokens[this.index] is TFunc)
                                mc.paramList.Add(FuncMethodCall());
                            else if (this.tokens[this.index] is TDelegate)
                                mc.paramList.Add(ParseDelegate());
                            else
                                mc.paramList.Add(ParseExpr());
                            c++;
                        }
                        else
                        {
                            this.index++;
                            c--;
                        }
                    }
                    if (mc.paramList.Count > 0 && c != 0)
                    {
                        throw new System.Exception(msg("Method calling '{0}' at line number:{1} is invalid", mc.MethodName, mToken.LineNumber));
                    }
                    if (this.index == this.tokens.Count || !(this.tokens[this.index] is TEndParen))
                    {
                        throw new System.Exception("...expected ending Parenthesis at line number:" + mToken.LineNumber);
                    }
                    mc.EndParen = ")";
                    this.index++;
                    if (this.tokens[this.index] is TBeginParen)
                    {
                        index++;
                        mc.paramListForAnnonimysMethod = new List<Expr>();
                        while (!(this.tokens[this.index] is TEndParen))
                        {
                            if ((this.tokens[this.index] is TComma))
                            {
                                index++;
                            }
                            else
                            {
                                mc.paramListForAnnonimysMethod.Add(ParseExpr());
                            }
                        }
                        index++;
                    }
                    result = mc;
                }
                #endregion

                else if (tokens[index] is TLembda)
                {
                    result = ParseLembda(true);
                }
                else if (this.tokens[this.index] is TIncrement)
                {
                    PostIncrementOpExpr inc = new PostIncrementOpExpr();
                    inc.Ident = GetString(mToken);
                    result = inc;
                    this.index++;
                }
                else if (this.tokens[this.index] is TDecrement)
                {
                    PostDecrementOpExpr inc = new PostDecrementOpExpr();
                    inc.Ident = GetString(mToken);
                    result = inc;
                    this.index++;
                }
                else
                {
                    Variable var = new Variable();
                    var.Ident = GetString(mToken);
                    result = var;
                    //index++;
                }
            }

            #region String Quotation
            if (this.tokens[this.index] is TStringInQuotation)
            {
                TStringInQuotation value = this.tokens[this.index++] as TStringInQuotation;
                StringQuotation stringLiteral = new StringQuotation();
                stringLiteral.Value = value.Value;
                result = stringLiteral;
            }
            #endregion
            #region Int Literal
            else if (this.tokens[this.index] is TIntegerValue)
            {
                TIntegerValue intVal = this.tokens[this.index++] as TIntegerValue;

                IntLiteral intLiteral = new IntLiteral();
                intLiteral.Value = intVal.Value;
                result = intLiteral;
            }
            #endregion
            #region double Literal
            else if (this.tokens[this.index] is TDoubleValue)
            {
                TDoubleValue dval = this.tokens[this.index++] as TDoubleValue;
                DoubleLiteral intLiteral = new DoubleLiteral();
                intLiteral.Value = dval.Value;
                result = intLiteral;
            }
            #endregion
            #region Begin Parenthesis
            else if (this.tokens[this.index] is TBeginParen)
            {
                mToken = this.tokens[this.index] as TBeginParen;
                this.index++;
                if (this.tokens[this.index] is TFrom)
                {
                    Expr linq = ParseLinqQuery();
                    index++;

                    if (this.tokens[this.index] is TDot && this.tokens[this.index + 1] is TStringLiteral && this.tokens[this.index + 2] is TBeginParen)
                    {
                        var str = "ToList|ToDictionary|ToArray";
                        if (str.IndexOf(GetString(this.tokens[this.index + 1])) >= 0)
                        {
                            index += 4;
                        }
                    }

                    result = linq;
                }
                else
                {
                    int currentIndex = this.index;
                    //check for lembda
                    while (!(this.tokens[this.index] is TEndParen)) { this.index++; }
                    this.index++;
                    if (this.tokens[this.index] is TLembda)
                    {
                        this.index = currentIndex - 1; ;
                        return ParseLembda();
                    }
                    this.index = currentIndex;
                    Parenthesis2 p = new Parenthesis2();
                    p.BeginParen = "(";
                    parenthesisCounter++;
                    p.content = ParseExpr();
                    ///////////////////
                    
                    //////////////////////

                    if (p.content == null)
                    {
                        if (this.tokens[this.index] is TList)
                        {
                            this.index++;
                            if (tokens[index] is TLT)
                            {
                                this.index = ResolveLTGT(this.index);
                                this.index += 1;
                            }
                        }
                        else if (this.tokens[this.index] is TDictionary)
                        {
                            this.index++;
                            if (tokens[index] is TLT)
                            {
                                this.index = ResolveLTGT(this.index);
                                this.index += 1;
                            }
                        }
                    }
                    else if (!(this.tokens[this.index] is TEndParen))
                    {
                        p.paramList = new List<Expr>();
                        p.paramList.Add(p.content);
                        p.content = null;
                        
                        while (!(this.tokens[this.index] is TEndParen))
                        {
                            if ((this.tokens[this.index] is TComma))
                            {
                                index++;
                            }
                            else
                            {
                                p.paramList.Add( ParseExpr());
                            }
                        }
                    }
                    if (this.index == this.tokens.Count || !(this.tokens[this.index] is TEndParen))
                    {
                        throw new System.Exception("expected ending Parenthesis at line number:" + mToken.LineNumber);
                    }
                    p.EndParen = ")";
                    parenthesisCounter--;
                    this.index++;
                    result = p;
                    if (this.tokens[this.index] is TStringLiteral)
                    {
                        result = ParseExpr();
                    }
                }
            }
            #endregion
            #region NEW Operator
            else if (tokens[index] is TNew)
            {

                mToken = tokens[index++] as TNew;
                #region List
                if (tokens[index] is TList)
                {
                    int temp = 0;
                    bool hasData = false;
                    ListHasData(ref temp, ref hasData);
                    index = temp;
                    if (hasData)
                    {
                        AstJsonArrey arr = new AstJsonArrey();
                        arr.ExprList = new List<Expr>();
                        do
                        {
                            if (this.tokens[this.index] is TComma)
                            {
                                index++;
                            }
                            else
                            {
                                arr.ExprList.Add(ParseExpr());
                            }

                        } while (!(this.tokens[this.index] is TEndBrace));
                        result = arr;
                    }
                    else
                    {
                        result = new AstEmptyList();
                    }
                }
                #endregion
                #region Dictionary
                else if (tokens[index] is TDictionary)
                {
                    this.index++;
                    if (tokens[index] is TLT)
                    {
                        this.index = ResolveLTGT(this.index);
                        if (tokens[index + 1] is TBeginParen)
                        {
                            this.index += 2;
                        }
                    }
                    else
                    {
                        throw new System.Exception("Invalid Dictionary at line number:" + mToken.LineNumber);
                    }
                    AstObjectConstructor ast = new AstObjectConstructor();
                    if (tokens[index + 1] is TBeginBrace)
                    {
                        index++;
                        if (!(tokens[index + 1] is TEndBrace))
                        {
                            index++;
                            do
                            {
                                //for begin brace
                                index++;

                                DicKeyValue dkv = new DicKeyValue();
                                dkv.Key = ParseExpr();

                                if (this.tokens[this.index] is TComma)
                                {
                                    index++;
                                }
                                else
                                {
                                    throw new System.Exception("Invalid Dictionary at line number:" + mToken.LineNumber + Environment.NewLine + "To initialize dictionary is not supported.");
                                }
                                dkv.Value = ParseExpr();
                                ast.Init.Add(dkv);
                               
                                //for end brace
                                index++;
                                if (this.tokens[this.index] is TComma)
                                {
                                    index++;
                                }

                            } while (!(this.tokens[this.index] is TEndBrace));
                        }
                        else
                        {
                            index++;
                        }
                        
                    }

                    ast.ObjectName = "Dictionary";
                    result = ast;
                }
                #endregion
                #region Array
                else if (tokens[index] is TBeginBraket)
                {
                    if (tokens[index + 1] is TEndBraket && tokens[index + 2] is TBeginBrace)
                    {
                        if (tokens[index + 3] is TEndBrace)
                        {
                            result = new AstEmptyList();
                            index = index + 3;
                        }
                        else
                        {
                            index += 3;
                            AstJsonArrey arr = new AstJsonArrey();
                            arr.ExprList = new List<Expr>();
                            do
                            {
                                if (this.tokens[this.index] is TComma)
                                {
                                    index++;
                                }
                                else
                                {
                                    arr.ExprList.Add(ParseExpr());
                                }

                            } while (!(this.tokens[this.index] is TEndBrace));
                            result = arr;
                        }
                    }
                    else
                    {
                        throw new System.Exception("Invalid array at line number:" + mToken.LineNumber);
                    }

                }
                #endregion
                #region Json Object

                else if (tokens[index + 1] is TBeginBrace || tokens[index] is TBeginBrace)
                {
                    if (tokens[index] is TBeginBrace) { index--; }
                    index++;
                    AstJsonObject obj = new AstJsonObject();
                    if (this.tokens[this.index] is TEndBrace)
                    {
                        this.index++;
                        result = obj;
                    }
                    else
                    {
                        this.index++;
                        do
                        {
                            if (this.tokens[this.index] is TComma)
                            {
                                index++;
                            }
                            if (this.tokens[this.index] is TStringLiteral)
                            {
                                Assign ass = new Assign();
                                ass.Ident = GetString(this.tokens[this.index]);
                                this.index += 2;
                                ass.Expr = ParseExpr();
                                obj.list.Add(ass);
                            }

                        } while (!(this.tokens[this.index] is TEndBrace));
                    }
                    result = obj;
                }
                #endregion
                #region Object
                else if (tokens[index] is TStringLiteral)
                {

                    string obName = "";

                    do
                    {
                        if (tokens[index] is TStringLiteral)
                        {
                            obName += GetString(tokens[index]);
                        }
                        else if (tokens[index] is TLT)
                        {
                            index = ResolveLTGT(index);

                        }

                        else if (tokens[index] is TDot)
                        {
                            obName += ".";
                        }
                        else
                        {
                            throw new System.Exception("Invalid Object at line number:" + mToken.LineNumber);
                        }
                        index++;
                    } while (!(this.tokens[this.index] is TBeginParen));

                    AstObjectConstructor ast = new AstObjectConstructor();
                    ast.ObjectName = obName;
                    index++;
                    do
                    {
                        if (this.tokens[this.index] is TComma)
                        {
                            index++;
                        }
                        else
                        {
                            ast.ParamList.Add(ParseExpr());
                        }

                    } while (!(this.tokens[this.index] is TEndParen));
                    if (tokens[index + 1] is TBeginBrace)
                    {
                        throw new System.Exception("Invalid Object at line number:" + mToken.LineNumber);
                    }
                    result = ast;
                }
                #endregion

                index++;
            }
            #endregion
            #region Not Operator
            else if (this.tokens[this.index] is TNot)
            {
                this.index++;
                NotExpr notExpr = new NotExpr();
                notExpr.Operand = ParseExpr();
                result = notExpr;
            }
            #endregion
            #region From
            else if (this.tokens[this.index] is TFrom)
            {
                Expr linq = ParseLinqQuery();
                index++;
                if (this.tokens[this.index] is TDot && this.tokens[this.index + 1] is TStringLiteral && this.tokens[this.index + 2] is TBeginParen)
                {
                    var str = "ToList|ToDictionary|ToArray";
                    if (str.IndexOf(GetString(this.tokens[this.index + 1])) >= 0)
                    {
                        index += 4;
                    }
                }
                result = linq;
            }
            #endregion
            #region Clone
            /*
            else if ((this.tokens[this.index] is TClone))
            {
                result = new ASTClone();
                this.index++;
                if (this.index < this.tokens.Count)
                {
                    ExprSequence sequence = new ExprSequence();
                    sequence.First = result;
                    sequence.Second = this.ParseExpr();
                    result = sequence;

                }
            }*/
            #endregion
            #region Delegate
            else if (this.tokens[this.index] is TDelegate)
            {
                result = ParseDelegate();
            }
            #endregion

            else if (this.tokens[this.index] is TIncrement)
            {
                PreIncrementOpExpr inc = new PreIncrementOpExpr();
                index++;
                inc.expr = ParseExpr();
                result = inc;
                // this.index++;
            }
            else if (this.tokens[this.index] is TDecrement)
            {
                PreDecrementOpExpr inc = new PreDecrementOpExpr();
                index++;
                inc.expr = ParseExpr();
                result = inc;
                // this.index++;
            }
            else if (this.tokens[this.index] is TBeginBraket)
            {

                ASTArrayIndex2 arrindex = new ASTArrayIndex2();
                arrindex.LeftExpr = result;
                this.index++;
                arrindex.index = ParseExpr();
                this.index++;
                arrindex.RightExpr = ParseExpr();
                result = arrindex;
            }
            #region Super Method
            else if (this.tokens[this.index] is TBase)
            {
                mToken = this.tokens[this.index] as TBase;
                string x = currentMethod.FunctionName;
                this.index++;
                if (!(this.tokens[this.index] is TDot))
                {
                    throw new System.Exception("parse error at token " + this.index + ": Line Number:" + mToken.LineNumber);
                }
                this.index++;
                if (GetString(this.tokens[index]) != currentMethod.FunctionName)
                {
                    throw new System.Exception("base keyword is only applicable for calling super version of current method at Line Number:" + mToken.LineNumber);
                }

                AstBase astBase = new AstBase();

                MethodCall2 mc = new MethodCall2();
                mc.LineNo = mToken.LineNumber;
                mc.MethodName = "this._super";               
                mc.BeginParen = "(";
                this.index += 2;
                int c = -1;
                while (!(this.tokens[this.index] is TEndParen))
                {
                    if (!(this.tokens[this.index] is TComma))
                    {
                        if (this.tokens[this.index] is TFunc)
                            mc.paramList.Add(FuncMethodCall());
                        else
                            mc.paramList.Add(ParseExpr());
                        c++;
                    }
                    else
                    {
                        this.index++;
                        c--;
                    }
                }
                if (mc.paramList.Count > 0 && c != 0)
                {
                    throw new System.Exception(msg("Method calling '{0}' at line number:{1} is invalid", mc.MethodName, mToken.LineNumber));
                }
                if (this.index == this.tokens.Count || !(this.tokens[this.index] is TEndParen))
                {
                    throw new System.Exception("...expected ending Parenthesis at line number:" + mToken.LineNumber);
                }
                mc.EndParen = ")";
                this.index++;
                //Token.astBase.Method = mc;
                //result = astBase;
                result = mc;

            }
            #endregion
            #region Binary Expression
            #region LOR Operator
            if (this.tokens[this.index] is TLADD)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.LAND;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region LANd Operator
            else if (this.tokens[this.index] is TLOR)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.LOR;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region LEqual Operator
            else if (this.tokens[this.index] is TEqualEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.LE;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Add Operator
            else if (this.tokens[this.index] is TAdd)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.Add;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Sub Op
            else if (this.tokens[this.index] is TSub)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.Sub;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Sub Op
            else if (this.tokens[this.index] is TJoinEquals)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.JoinEquals;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Mul OP
            else if (this.tokens[this.index] is TMul)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.Mul;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Div op
            else if (this.tokens[this.index] is TDiv)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.Div;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region LT OP
            else if (this.tokens[this.index] is TLT)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.LT;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region LTE OP
            else if (this.tokens[this.index] is TLTE)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.LTE;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region GT OP
            else if (this.tokens[this.index] is TGT)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.GT;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region GTE OP
            else if (this.tokens[this.index] is TGTE)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.GTE;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Mod Op
            else if (this.tokens[this.index] is TMod)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.MOD;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region NE OP
            else if (this.tokens[this.index] is TNE)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.NE;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region What ?
            else if (this.tokens[this.index] is TWhat)
            {
                ASTWhat what = new ASTWhat();
                what.Left = result;
                this.index++;
                what.Expr1 = ParseExpr();
                this.index++;
                what.Expr2 = ParseExpr();
                if (this.tokens[this.index] is TSemi)
                    what.HasSemi = true;
                result = what;

            }
            #endregion
            #region +=
            else if (this.tokens[this.index] is TAddEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.ADD_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region -=
            else if (this.tokens[this.index] is TSubEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.SUB_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region *=
            else if (this.tokens[this.index] is TMulEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.MUL_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region \=
            else if (this.tokens[this.index] is TDivEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.DIV_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region mod=
            else if (this.tokens[this.index] is TModEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.MOD_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region and=
            else if (this.tokens[this.index] is TAndEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.AND_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region or=
            else if (this.tokens[this.index] is TOrEqual)
            {
                this.index++;
                BinExpr binaryExpr = new BinExpr();
                binaryExpr.Left = result;
                binaryExpr.Op = BinOp.OR_EQUAL;
                binaryExpr.Right = ParseExpr();
                result = binaryExpr;
            }
            #endregion
            #region Casting by AS
            else if (this.tokens[this.index] is TAs)
            {
                ASTAs what = new ASTAs();
                what.Left = result;
                index++;
                if (this.tokens[this.index] is TList)
                {
                    this.index++;
                    if (tokens[index] is TLT)
                    {
                        this.index = ResolveLTGT(this.index);
                        this.index += 1;
                    }
                }
                else if (this.tokens[this.index] is TDictionary)
                {
                    this.index++;
                    if (tokens[index] is TLT)
                    {
                        this.index = ResolveLTGT(this.index);
                        this.index += 1;
                    }
                }
                else if (this.tokens[this.index] is TStringLiteral)
                {
                    index++;
                }
                else
                {
                    throw new System.Exception("Invalid casting at line number:" + mToken.LineNumber);
                }
                result = what;

            }
            #endregion
            #endregion
            #region Comment for JS
            if (this.tokens[this.index] is TJS)
            {
                AstJSExpr js = new AstJSExpr();
                js.LeftExpr = result;
                js.JScript = ((TJS)this.tokens[this.index]).JS;
                this.index++;

                result = js;

            }
            #endregion
            if (this.index < this.tokens.Count && this.tokens[this.index] is TDot)
            {
                index++;
                ExprDot sequence = new ExprDot();
                sequence.First = result;
                sequence.Second = this.ParseExpr();
                result = sequence;

            }

            return result;
        }


    }
}
