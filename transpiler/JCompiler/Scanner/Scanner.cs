using Collections = System.Collections.Generic;
using IO = System.IO;
using Text = System.Text;
using System.Collections;
using JCompiler.Tokens;
using System.Collections.Generic;

namespace JCompiler.Scanner
{
    public sealed class Scanner
    {
        private readonly Collections.IList<Token> result;
        private int mLineNumber = 1;
        public Scanner(IO.TextReader input)
        {
            this.result = new Collections.List<Token>();
            this.Scan(input);
        }

        public Collections.IList<Token> Tokens
        {
            get { return this.result; }
        }
        private bool isLinqQuery = false;
        private void Scan(IO.TextReader input)
        {
            string currentStringLiteral = "";
            while (input.Peek() != -1)
            {
                char ch = (char)input.Peek();

                // Scan individual tokens
                if (char.IsWhiteSpace(ch))
                {
                    if (ch == '\n')
                    {
                        this.mLineNumber++;
                    }
                    // eat the current char and skip ahead!
                    input.Read();
                }
                else if (char.IsLetter(ch) || ch == '_')
                {
                    // keyword or identifier

                    Text.StringBuilder accum = new Text.StringBuilder();

                    while (char.IsLetter(ch)
                        || ch == '_' || ch == '1' || ch == '2' || ch == '3'
                        || ch == '4' || ch == '5' || ch == '6' || ch == '7'
                        || ch == '8' || ch == '9' || ch == '0')
                    {
                        accum.Append(ch);
                        input.Read();

                        if (input.Peek() == -1)
                        {
                            break;
                        }
                        else
                        {
                            ch = (char)input.Peek();
                        }

                    }
                    switch (accum.ToString())
                    {
                        case "delegate":
                            this.result.Add(new TDelegate(mLineNumber));
                            break;
                        case "foreach":
                            this.result.Add(new TForeach(mLineNumber));
                            break;
                        case "using":
                            this.result.Add(new TUsing(mLineNumber));
                            break;
                        case "namespace":
                            this.result.Add(new TNamespace(mLineNumber));
                            break;
                        case "private":
                        case "public":
                        case "protected":
                            this.result.Add(new TModifier(accum.ToString(), mLineNumber));
                            break;

                        case "from":
                            isLinqQuery = true;
                            this.result.Add(new TFrom(mLineNumber));
                            break;
                        case "join":
                            if (isLinqQuery)
                                this.result.Add(new TJoin(mLineNumber));
                            else
                                this.result.Add(new TStringLiteral(mLineNumber, "join"));
                            break;
                        case "equals":
                            if (isLinqQuery)
                                this.result.Add(new TJoinEquals(mLineNumber));
                            else
                                this.result.Add(new TStringLiteral(mLineNumber, "equals"));
                            break;
                        case "on":
                            if (isLinqQuery)
                                this.result.Add(new TJoinOn(mLineNumber));
                            else
                                this.result.Add(new TStringLiteral(mLineNumber, "on"));

                            break;

                        case "where":
                            if (isLinqQuery)
                                this.result.Add(new TWhere(mLineNumber));
                            else
                                this.result.Add(new TStringLiteral(mLineNumber, "where"));

                            break;
                        case "select":
                            if (isLinqQuery)
                                this.result.Add(new TSelect(mLineNumber));
                            else
                                this.result.Add(new TStringLiteral(mLineNumber, "select"));

                            break;
                        case "in":
                            this.result.Add(new TIn(mLineNumber));
                            break;

                        case "as":
                            this.result.Add(new TAs(mLineNumber));
                            break;
                        case "List":
                        case "IList":
                            this.result.Add(new TList(mLineNumber));
                            break;
                        case "Dictionary":
                        case "IDictionary":
                            this.result.Add(new TDictionary(mLineNumber));
                            break;

                        case "new":
                            this.result.Add(new TNew(mLineNumber));
                            break;
                        case "while":
                            this.result.Add(new TWhile(mLineNumber));
                            break;
                        case "switch":
                            this.result.Add(new TSwitch(mLineNumber));
                            break;
                        case "if":
                            this.result.Add(new TIf(mLineNumber));
                            break;
                        case "else":
                            input.Read();
                            ch = (char)input.Peek();

                            while (char.IsWhiteSpace(ch))
                            {
                                if (ch == '\n')
                                {
                                    mLineNumber++;
                                }
                                input.Read();
                                ch = (char)input.Peek();

                            }
                            if (ch == '{')
                            {
                                this.result.Add(new TElse(mLineNumber));
                                this.result.Add(new TBeginBrace(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                string xif = ch.ToString();
                                input.Read();
                                xif += (char)input.Peek();
                                input.Read();

                                if (xif == "if")
                                {
                                    this.result.Add(new TElseIf(mLineNumber));
                                }
                                else
                                {
                                    throw new System.Exception("Wrong expression at line number " + mLineNumber);
                                }
                            }
                            break;
                        case "elseif":
                            this.result.Add(new TElseIf(mLineNumber));
                            break;
                        case "case":
                            this.result.Add(new TCase(mLineNumber));
                            break;
                        case "break":
                            this.result.Add(new TBreak(mLineNumber));
                            break;
                        case "continue":
                            this.result.Add(new TContinue(mLineNumber));
                            break;
                        case "default":
                            this.result.Add(new TDefault(mLineNumber));
                            break;
                        case "for":
                            this.result.Add(new TFor(mLineNumber));
                            break;
                        case "return":
                            this.result.Add(new TReturn(mLineNumber));
                            break;
                        case "class":
                            this.result.Add(new TClass(mLineNumber));
                            break;
                        case "base":
                            this.result.Add(new TBase(mLineNumber));
                            break;
                        case "Func":
                            this.result.Add(new TFunc(mLineNumber));
                            break;
                        case "virtual":
                            this.result.Add(new TVirtual(mLineNumber));
                            break;
                        case "override":
                            this.result.Add(new TOverride(mLineNumber));
                            break;
                        case "enum":
                            this.result.Add(new TEnum(mLineNumber));
                            break;
                        case "interface":
                            int count = this.result.Count;
                            if (this.result[count - 1] is TModifier)
                            {
                                this.result.RemoveAt(count - 1);
                            }
                            do
                            {
                                ch = (char)input.Peek();
                                input.Read();
                            } while (ch != '{');
                            Stack stack = new Stack();
                            stack.Push('{');
                            do
                            {
                                ch = (char)input.Peek();
                                input.Read();
                                if (ch == '{') stack.Push('{');
                                else if (ch == '}') stack.Pop();
                                else if (ch == '\n')
                                {
                                    mLineNumber++;
                                }
                            } while (stack.Count != 0);

                            break;
                        default:
                            currentStringLiteral = accum.ToString();
                            this.result.Add(new TStringLiteral(mLineNumber, currentStringLiteral));
                            break;
                    }

                }
                else if (ch == '"')
                {
                    // string literal
                    Text.StringBuilder accum = new Text.StringBuilder();

                    input.Read(); // skip the '"'

                    if (input.Peek() == -1)
                    {
                        throw new System.Exception("unterminated string literal");
                    }

                    while ((ch = (char)input.Peek()) != '"')
                    {
                        accum.Append(ch);
                        input.Read();
                        if (ch == '\\')
                        {
                            ch = (char)input.Peek();
                            accum.Append(ch);
                            input.Read();
                        }
                        if (input.Peek() == -1)
                        {
                            throw new System.Exception("unterminated string literal");
                        }
                    }

                    // skip the terminating "
                    input.Read();
                    this.result.Add(new TStringInQuotation(mLineNumber, accum.ToString()));
                }
                else if (char.IsDigit(ch))
                {
                    // numeric literal

                    Text.StringBuilder accum = new Text.StringBuilder();
                    bool flag = false;
                    while (char.IsDigit(ch))
                    {
                        accum.Append(ch);
                        input.Read();

                        if (input.Peek() == -1)
                        {
                            break;
                        }
                        else
                        {
                            ch = (char)input.Peek();
                            if (ch == '.')
                            {
                                flag = true;
                                accum.Append(ch);
                                input.Read();
                                ch = (char)input.Peek();
                            }
                        }
                    }
                    if (flag)
                        this.result.Add(new TDoubleValue(double.Parse(accum.ToString()), mLineNumber));
                    else
                        this.result.Add(new TIntegerValue(int.Parse(accum.ToString()), mLineNumber));
                }
                else switch (ch)
                    {
                        case '+':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '+')
                            {
                                this.result.Add(new TIncrement(mLineNumber));
                                input.Read();
                            }
                            else if (ch == '=')
                            {
                                this.result.Add(new TAddEqual(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TAdd(mLineNumber));
                            }
                            break;

                        case '-':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '-')
                            {
                                this.result.Add(new TDecrement(mLineNumber));
                                input.Read();
                            }
                            else if (ch == '=')
                            {
                                this.result.Add(new TSubEqual(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TSub(mLineNumber));
                            }
                            break;

                        case '*':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '=')
                            {
                                this.result.Add(new TMulEqual(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TMul(mLineNumber));
                            }
                            break;
                        case '%':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '=')
                            {
                                this.result.Add(new TModEqual(mLineNumber));
                                input.Read();
                            }
                            else
                                this.result.Add(new TMod(mLineNumber));
                            break;
                        case '!':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '=')
                            {
                                this.result.Add(new TNE(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TNot(mLineNumber));
                            }
                            break;
                        case '/':
                            input.Read();
                            ch = (char)input.Peek();
                            switch (ch)
                            {

                                case '/':
                                    while ((ch = (char)input.Peek()) != '\n')
                                    {

                                        input.Read();

                                        if (input.Peek() == -1)
                                        {
                                            throw new System.Exception("unterminated string literal");
                                        }
                                    }
                                    break;
                                case '*':
                                    input.Read();
                                    ch = (char)input.Peek();
                                    if (ch == '$')
                                    {
                                        System.Text.StringBuilder sb = new Text.StringBuilder();
                                        //input.Read();
                                        while (true)
                                        {
                                            input.Read();

                                            if (input.Peek() == -1)
                                            {
                                                throw new System.Exception("unterminated string literal :" + ch);
                                            }
                                            ch = (char)input.Peek();

                                            if (ch == '$')
                                            {
                                                string str = ch.ToString();
                                                input.Read();
                                                ch = (char)input.Peek();
                                                str += ch.ToString();
                                                if (ch == '*')
                                                {
                                                    input.Read();
                                                    input.Read();
                                                    break;
                                                }
                                                else
                                                {
                                                    sb.Append(str);
                                                }
                                            }
                                            else if (ch == '\n')
                                            {
                                                mLineNumber++;
                                            }
                                            else
                                            {
                                                sb.Append(ch);
                                            }
                                        }
                                        TJS js = new TJS(mLineNumber);
                                        js.JS = sb.ToString();
                                        this.result.Add(js);
                                    }
                                    else
                                    {
                                        while (true)
                                        {
                                            input.Read();

                                            if (input.Peek() == -1)
                                            {
                                                throw new System.Exception("unterminated string literal");
                                            }
                                            ch = (char)input.Peek();
                                            if (ch == '*')
                                            {
                                                input.Read();
                                                ch = (char)input.Peek();
                                                if (ch == '/')
                                                {
                                                    input.Read();
                                                    break;
                                                }
                                            }
                                            else if (ch == '\n')
                                            {
                                                mLineNumber++;
                                            }
                                        }
                                    }
                                    break;
                                case '=':
                                    this.result.Add(new TDivEqual(mLineNumber));
                                    input.Read();

                                    break;
                                default:
                                    this.result.Add(new TDiv(mLineNumber));
                                    break;
                            }

                            break;

                        case '=':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '=')
                            {
                                this.result.Add(new TEqualEqual(mLineNumber));
                                input.Read();
                            }
                            else if (ch == '>')
                            {
                                this.result.Add(new TLembda(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TEqual(mLineNumber));
                            }
                            break;
                        case '<':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '=')
                            {
                                this.result.Add(new TLTE(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TLT(mLineNumber));
                            }
                            break;
                        case '>':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '=')
                            {
                                this.result.Add(new TGTE(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                this.result.Add(new TGT(mLineNumber));
                            }
                            break;
                        case '&':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '&')
                            {
                                this.result.Add(new TLADD(mLineNumber));
                                input.Read();
                            }
                            else if (ch == '=')
                            {
                                this.result.Add(new TAndEqual(mLineNumber));
                                input.Read();
                            }
                            break;
                        case '|':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '|')
                            {
                                this.result.Add(new TLOR(mLineNumber));
                                input.Read();
                            }
                            else if (ch == '=')
                            {
                                this.result.Add(new TOrEqual(mLineNumber));
                                input.Read();
                            }
                            break;
                        case ';':
                            input.Read();
                            this.result.Add(new TSemi(mLineNumber));
                            isLinqQuery = false;
                            break;
                        case '(':
                            input.Read();
                            this.result.Add(new TBeginParen(mLineNumber));
                            break;
                        case ')':
                            input.Read();
                            this.result.Add(new TEndParen(mLineNumber));
                            break;
                        case '{':
                            input.Read();
                            this.result.Add(new TBeginBrace(mLineNumber));
                            break;
                        case '}':
                            input.Read();
                            this.result.Add(new TEndBrace(mLineNumber));
                            break;
                        case ',':
                            input.Read();
                            this.result.Add(new TComma(mLineNumber));
                            break;
                        case ':':
                            input.Read();
                            this.result.Add(new TClone(mLineNumber));
                            break;
                        case '.':
                            input.Read();
                            this.result.Add(new TDot(mLineNumber));
                            break;
                        case '?':
                            input.Read();
                            ch = (char)input.Peek();
                            if (ch == '?')
                            {
                                this.result.Add(new TLOR(mLineNumber));
                                input.Read();
                            }
                            else
                            {
                                //to ignore ? from int?,bool?,...etc
                                List<string> primDataList = new List<string> { "int", "float", "double", "bool" };
                                input.Read();
                                if (string.IsNullOrEmpty(primDataList.Find(x => x == currentStringLiteral)))
                                {                                   
                                    this.result.Add(new TWhat(mLineNumber));
                                }
                            }
                            break;
                        case '[':
                            input.Read();
                            this.result.Add(new TBeginBraket(mLineNumber));
                            break;
                        case ']':
                            input.Read();
                            this.result.Add(new TEndBraket(mLineNumber));
                            break;
                        case '#':
                            input.Read();
                            ch = (char)input.Peek();
                            while (ch != '\n')
                            {

                                input.Read();

                                if (input.Peek() == -1)
                                {
                                    break;
                                }
                                else
                                {
                                    ch = (char)input.Peek();
                                }

                            }

                            break;
                        default:
                            throw new System.Exception("Scanner encountered unrecognized character '" + ch + "'. Line Number:" + mLineNumber);
                    }

            }
        }
    }
}
