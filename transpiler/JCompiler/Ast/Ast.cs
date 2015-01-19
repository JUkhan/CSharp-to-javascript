using System.Collections.Generic;
namespace JCompiler.Ast
{
    
    public abstract class Stmt
    {
        public bool HasSemi=false;
    }
    public class ASTComplexStmt : Stmt
    {

        public Expr LeftExpression;
        public Expr RightExpression;

    }
    public class AstJS : Stmt {
        public string JScript { get; set; }
    }
    public class AstJSExpr : Expr
    {
        public Expr LeftExpr;
        public string JScript { get; set; }
    }
    // var <ident> = <expr>
    public class DeclareVar : Stmt
    {
        public string TypeName;
        public string Ident;
        public Expr Expr;
        public bool HasSemi;
        public string ModifierName;
        public List<Assign> list = new List<Assign>();
    }
    public class SUsing : Stmt
    {
        public string Name;
    }
    public class SNamespace : Stmt
    {
        public string Name;
        public string ModuleName;
        public string DI;
        public string ActionType;
        public string ActionName;
    }
    public class SModifier : Stmt
    {
        public string Name;
    }
   
   
    public class Return : Stmt
    {
        public Expr Expr;
    }
    public class AstClass:Stmt
    {
        public string ClassName;
        public Stmt Content;
        public string Namespace;
        public string InheritedBy;
    }
    public class Function : Stmt
    {
        public string FunctionName;
        public List<FunctionParam> ParamList = new List<FunctionParam>();
        public Stmt Content;
        public string ReturnType;
        public string ModifierName;
        public Expr Base;
    }
    public class FunctionParam
    {
        public string ParamType { get; set; }
        public string ParamName { get; set; }
        public List<string> FuncSignature = new List<string>();
       
    }
    public class ExprNew : Expr
    {
        public ExprNew()
        {
            ObjectType = "ignore";
        }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public List<Expr> ValueList { get; set; }

    }
    public class AstJsonObject:Expr
    {       
        public List<Assign> list = new List<Assign>();
    }
    public class AstJsonArrey : Expr
    {
        public List<AstJsonObject> list = new List<AstJsonObject>();
        public List<Expr> ExprList;
    }
    public class AstLinq : Expr
    {        
        public string varName;
        public Expr InExp;
        public Expr WhereExp;
        public AstJsonObject SelectObject;
        public Expr SelectVarName;

        public AstLinq Join; 
    }
    public class AstEmptyList : Expr
    { 
    
    }
    // <ident> = <expr>
    public class Assign : Stmt
    {
        public string Ident;
        public Expr Expr;
        
    }
    public class AddEqual : Stmt
    {
        public string Ident;
        public Expr Expr;
       
    }
   
    public class SubEqual : Stmt
    {
        public string Ident;
        public Expr Expr;
       
    }
    public class MulEqual : Stmt
    {
        public string Ident;
        public Expr Expr;
       
    }
    public class DivEqual : Stmt
    {
        public string Ident;
        public Expr Expr;
       
    }
    public class PreIncrementOp : Stmt
    {
        public string Ident;
       
    }
    public class PreDecrementOp : Stmt
    {
        public string Ident;
       
    }
    public class PreIncrementOpExpr : Expr
    {
        public Expr expr;

    }
    public class PreDecrementOpExpr : Expr
    {
        public Expr expr;

    }
    public class PostIncrementOp : Stmt
    {
        public string Ident;
      
    }
    public class PostIncrementOpExpr : Expr
    {
        public string Ident;

    }
    public class PostDecrementOp : Stmt
    {
        public string Ident;
       
    }
    public class PostDecrementOpExpr : Expr
    {
        public string Ident;

    }
    // for (<ident> = <expr>; <condition>; <increment>| <ident> Op <expr>){<body>} 
    public class ForLoop : Stmt
    {
        public Stmt Initialize;
        public Expr Condition;
        public Stmt Increment;
        public Stmt Body;

    }

    public class ASTArrayIndex : Stmt
    {
        public string Ident;
        public Expr index;
        public Expr Expr;
        public bool isEqual;
    }
    public class ASTArrayIndex2 : Expr
    {
        public Expr LeftExpr;
        //public string Ident;
        public Expr index;
        public Expr RightExpr;
        public bool isEqual;
    }
    public class WhileLoop : Stmt
    {
        public Expr Condition;
        public Stmt Body;
    }
    public class ForEach : Stmt
    {
        public string varName;
        public Expr Source;
        public Stmt Body;
    }
    public class IF : Stmt
    {
        public Expr Condition;
        public Stmt Body;
    }
    public class ElseIF : Stmt
    {
        public Expr Condition;
        public Stmt Body;
    }
    public class ELSE : Stmt
    {
        public Stmt Body;
    }
    public class DoWhile : Stmt
    {
        public Expr Condition;
        public Stmt Body;
    }
    public class Continue : Stmt { }
    public class Break : Stmt { }

    public class AstBase:Stmt{        
        public MethodCall Method;
    }
    // read_int <ident>
    public class ReadInt : Stmt
    {
        public string Ident;
    }
    public class ReadIntMessageBox : Stmt
    {
        public string Msg;

        public string Ident;
    }
    // <stmt> ; <stmt>
    public class StmtSequence : Stmt
    {
        public Stmt First;
        public Stmt Second;
    }
    public class ExprSequence : Expr
    {
        public Expr First;
        public Expr Second;
    }
    public class ExprDot : Expr
    {
        public Expr First;
        public Expr Second;
    }
    
    public abstract class Expr
    {     
        public bool HasSemi;
    }   

    // <int> := <digit>+
    public class IntLiteral : Expr
    {
        public int Value;
    }

    // <double> := <digit>+
    public class DoubleLiteral : Expr
    {
        public double Value;
    }

    // <float> := <digit>+
    public class FloatLiteral : Expr
    {
        public float Value;
    }

    // <ident> := <char> <ident_rest>*
    // <ident_rest> := <char> | <digit>
    public class Variable : Expr
    {
        public string Ident;
    }
    public class ASTWhat : Expr
    {
        public Expr Left;
        public Expr Expr1;
        public Expr Expr2;
    }
    public class ASTAs : Expr
    {
        public Expr Left;       
       
    }
    public class ASTClone : Expr
    {

    }
    public class Comma : Expr
    {

    }

    // <bin_expr> := <expr> <bin_op> <expr>
    public class BinExpr : Expr
    {
        public Expr Left;
        public Expr Right;
        public BinOp Op;
       
    }
    public class NotExpr : Expr
    {
        public Expr Operand;

    }
    // <Parenthesis> := <(> <expr> <)>
    public class Parenthesis :Stmt
    {
        public string BeginParen;
        public Expr content;
        public string EndParen;
    }
    public class Parenthesis2 : Expr
    {
        public string BeginParen;
        public Expr content;
        public string EndParen;
        public List<Expr> paramList = null;
    }
    // <MethodCall> := <(> <params*> <)>
    public class MethodCall2 : Expr
    {
        public string ParamTypeList;
        public string ReturnType;
        public string BeginParen;
        public List<Expr> paramList = new List<Expr>();
        public string EndParen;
        public string MethodName;
        public List<Expr> paramListForAnnonimysMethod { get; set; }
        public int LineNo = 0;
    }
    
    public class MethodCall : Stmt
    {
        public string ParamTypeList;
        public string ReturnType;
        public string BeginParen;
        public List<Expr> paramList = new List<Expr>();
        public string EndParen;
        public string MethodName;
        public int LineNo = 0;
       
    }
    public class DicKeyValue {
        public Expr Key { get; set; }
        public Expr Value { get; set; }
    }
    public class AstObjectConstructor : Expr {
        public string ObjectName { get; set; }
        public List<Expr> ParamList = new List<Expr>();
        public List<DicKeyValue> Init = new List<DicKeyValue>();
    }
    public class Func : Expr
    {
        public List<string> ParamNameList = new List<string>();
        public Stmt Body;
    }
    public class ASTDelegate : Expr
    {
        public List<string> ParamNameList = new List<string>();
        public Stmt Body;
    }
    public class ASTLembda : Expr
    {
        public List<string> ParamNameList = new List<string>();
        public Stmt Body;
        public Expr singleBody;
    }
    public class Switch : Stmt
    {
        public Expr Expr;
        public Stmt Body;
    }
    public class Case : Stmt
    {
        public object ident;
    }
    public class Default : Stmt
    {

    }
    public class DOT : Stmt
    {

    }
    public class DOTLeft : Stmt
    {
        public string Ident;
    }
    public class DOT2 :Expr
    {

    }
    public class DOTLeft2 : Expr
    {
        public string Ident;
    }
   
    public class New : Stmt
    {
       
    }
    public class New2 : Expr
    {

    }
   public class StringQuotation:Expr{
       public string Value { get; set; }
    }
    // <bin_op> := + | - | * | /
    public enum BinOp
    {        
        Add,
        Sub,
        Mul,
        Div,
        LT,
        LTE,
        GT,
        GTE,
        NE, LAND, LOR, LE, MOD, JoinEquals, ADD_EQUAL, SUB_EQUAL, MUL_EQUAL, DIV_EQUAL, MOD_EQUAL, AND_EQUAL, OR_EQUAL
    }
}