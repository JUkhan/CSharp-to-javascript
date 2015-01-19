using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JCompiler.Tokens
{
    public class Token
    {
        public Token(int lineNumber) 
        {
            LineNumber = lineNumber;
        }
        public int LineNumber { get; set; }
    }
    
    public class TDelegate : Token
    {
        public TDelegate(int lineNumber) : base(lineNumber) { }
    }
    public class TAs : Token
    {
        public TAs(int lineNumber) : base(lineNumber) { }
    }
    public class TBeginBraket : Token
    {
        public TBeginBraket(int lineNumber) : base(lineNumber) { }
    }
    public class TEndBraket : Token
    {
        public TEndBraket(int lineNumber) : base(lineNumber) { }
    }
    public class TUsing : Token
    {
        public TUsing(int lineNumber) : base(lineNumber) { }
    }
    public class TFrom : Token
    {
        public TFrom(int lineNumber) : base(lineNumber) { }
    }
    public class TForeach : Token
    {
        public TForeach(int lineNumber) : base(lineNumber) { }
    }
    public class TIn : Token
    {
        public TIn(int lineNumber) : base(lineNumber) { }
    }
    public class TSelect : Token
    {
        public TSelect(int lineNumber) : base(lineNumber) { }
    }
    public class TJoin : Token
    {
        public TJoin(int lineNumber) : base(lineNumber) { }
    }
    public class TJoinEquals : Token
    {
        public TJoinEquals(int lineNumber) : base(lineNumber) { }
    }
    public class TJoinOn : Token
    {
        public TJoinOn(int lineNumber) : base(lineNumber) { }
    }
    public class TWhere : Token
    {
        public TWhere(int lineNumber) : base(lineNumber) { }
    }
    public class TList : Token
    {
        public TList(int lineNumber) : base(lineNumber) { }
    }
    public class TDictionary : Token {
        public TDictionary(int lineNumber) : base(lineNumber) { }
    }
    public class TVar : Token
    {
        public TVar(int lineNumber) : base(lineNumber) { }
    }
    public class TWhat : Token
    {
        public TWhat(int lineNumber) : base(lineNumber) { }
    }
    public class TNamespace : Token
    {
        public TNamespace(int lineNumber) : base(lineNumber) { }
    }
    public class TModifier : Token
    {
        public TModifier(string name, int lineNumber) : base(lineNumber) { this.Name = name; }
        public string Name;
    }
   
    public class TClass : Token
    {
        public TClass(int lineNumber) : base(lineNumber) { }
    }
    public class TFunc : Token
    {
        public TFunc(int lineNumber) : base(lineNumber) { }
    }
    public class TVirtual : Token
    {
        public TVirtual(int lineNumber) : base(lineNumber) { }
    }
     
    public class TOverride : Token
    {
        public TOverride(int lineNumber) : base(lineNumber) { }
    }
    public class TEnum : Token
    {
        public TEnum(int lineNumber) : base(lineNumber) { }
    }
    public class TReturn : Token
    {
        public TReturn(int lineNumber) : base(lineNumber) { }
    }
    public class TString : Token
    {
        public TString(int lineNumber, string value) : base(lineNumber) { Value = value; }
        public string Value { get; set; }
    }
   
    public class TBeginBrace : Token
    {
        public TBeginBrace(int lineNumber) : base(lineNumber) { }
    }
    public class TEndBrace : Token
    {
        public TEndBrace(int lineNumber) : base(lineNumber) { }
    }
    public class TBeginParen : Token
    {
        public TBeginParen(int lineNumber) : base(lineNumber) { }
    }
    public class TEndParen : Token
    {
        public TEndParen(int lineNumber) : base(lineNumber) { }
    }
     public class TStringInQuotation : Token
    {
        public TStringInQuotation(int lineNumber, string value) : base(lineNumber) { Value = value; }
        public string Value { get; set; }
    }
    public class TInt : Token
    {
        public TInt(int lineNumber) : base(lineNumber) { }
       
    }
    public class TStringLiteral : Token
    {
        public TStringLiteral(int lineNumber, string value) : base(lineNumber) { Value = value; }
        public string Value { get; set; }

    }
    
    public class TFloat : Token
    {
        public TFloat(int lineNumber) : base(lineNumber) { }
    }
    public class TDouble : Token
    {
        public TDouble(int lineNumber) : base(lineNumber) { }
    }
    public class TNew : Token
    {
        public TNew(int lineNumber) : base(lineNumber) { }
    }
    public class TAdd : Token
    {
        public TAdd(int lineNumber) : base(lineNumber) { }
    }
    public class TLADD : Token
    {
        public TLADD(int lineNumber) : base(lineNumber) { }
    }
    public class TLOR : Token
    {
        public TLOR(int lineNumber) : base(lineNumber) { }
    }
    public class TSub : Token
    {
        public TSub(int lineNumber) : base(lineNumber) { }
    }
    public class TModEqual: Token
    {
        public TModEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TAndEqual : Token
    {
        public TAndEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TOrEqual: Token
    {
        public TOrEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TMul : Token
    {
        public TMul(int lineNumber) : base(lineNumber) { }
    }
    
    public class TDiv : Token
    {
        public TDiv(int lineNumber) : base(lineNumber) { }
    }
    public class TMod : Token
    {
        public TMod(int lineNumber) : base(lineNumber) { }
    }
    public class TFor : Token
    {
        public TFor(int lineNumber) : base(lineNumber) { }
    }
    public class TBool : Token
    {
        public TBool(int lineNumber) : base(lineNumber) { }
    }
    public class TSwitch : Token
    {
        public TSwitch(int lineNumber) : base(lineNumber) { }
    }
    public class TBreak : Token
    {
        public TBreak(int lineNumber) : base(lineNumber) { }
    }
    public class TIf : Token
    {
        public TIf(int lineNumber) : base(lineNumber) { }
    }
    public class TElse : Token
    {
        public TElse(int lineNumber) : base(lineNumber) { }
    }
    public class TElseIf : Token
    {
        public TElseIf(int lineNumber) : base(lineNumber) { }
    }
    public class TCase : Token
    {
        public TCase(int lineNumber) : base(lineNumber) { }
    }
    public class TContinue : Token
    {
        public TContinue(int lineNumber) : base(lineNumber) { }
    }
    public class TIdentifier : Token
    {
        public TIdentifier(int lineNumber) : base(lineNumber) { }
    }
    
    //public class TIntLiteral : Token
    //{
    //    public TIntLiteral(int lineNumber) : base(lineNumber) { }
    //}
    //public class TFloatLiteral : Token
    //{
    //    public TFloatLiteral(int lineNumber) : base(lineNumber) { }
    //}
    //public class TDoubleLiteral : Token
    //{
    //    public TDoubleLiteral(int lineNumber) : base(lineNumber) { }
    //}
    public class TLT : Token
    {
        public TLT(int lineNumber) : base(lineNumber) { }
    }
    public class TGT : Token
    {
        public TGT(int lineNumber) : base(lineNumber) { }
    }
    public class TGTE : Token
    {
        public TGTE(int lineNumber) : base(lineNumber) { }
    }
    public class TLTE : Token
    {
        public TLTE(int lineNumber) : base(lineNumber) { }
    }
    public class TNot : Token
    {
        public TNot(int lineNumber) : base(lineNumber) { }
    }
    public class TEqual : Token
    {
        public TEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TEqualEqual : Token
    {
        public TEqualEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TSemi : Token
    {
        public TSemi(int lineNumber) : base(lineNumber) { }
    }
    public class TComma : Token
    {
        public TComma(int lineNumber) : base(lineNumber) { }
    }
    public class TIntegerValue : Token
    {
        public int Value { get; set; }
        public TIntegerValue(int value, int lineNumber) : base(lineNumber) { 
         Value=value;
        }
    }
    public class TDoubleValue : Token
    {
        public double Value { get; set; }
        public TDoubleValue(double value,int lineNumber)
            : base(lineNumber)
        {
            Value=value;
        }
    }
    public class TIncrement : Token
    {
        public TIncrement(int lineNumber) : base(lineNumber) { }
    }
    public class TDecrement : Token
    {
        public TDecrement(int lineNumber) : base(lineNumber) { }
    }
    public class TAddEqual : Token
    {
        public TAddEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TSubEqual : Token
    {
        public TSubEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TMulEqual : Token
    {
        public TMulEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TDivEqual : Token
    {
        public TDivEqual(int lineNumber) : base(lineNumber) { }
    }
    public class TJS : Token
    {
        public TJS(int lineNumber) : base(lineNumber) { }
        public string JS { get; set; }
    }
    public class TNE : Token
    {
        public TNE(int lineNumber) : base(lineNumber) { }
    }
    public class TBase : Token
    {
        public TBase(int lineNumber) : base(lineNumber) { }
    }
    public class TWhile : Token
    {
        public TWhile(int lineNumber) : base(lineNumber) { }
    }
    public class TClone : Token
    {
        public TClone(int lineNumber) : base(lineNumber) { }
    }
    public class TDefault : Token
    {
        public TDefault(int lineNumber) : base(lineNumber) { }
    }
    public class TDot:Token
    {
        public TDot(int lineNumber) : base(lineNumber) { }
    }
    public class TLembda : Token
    {
        public TLembda(int lineNumber) : base(lineNumber) { }
    }
   //43tokens
}
