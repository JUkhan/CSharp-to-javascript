using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using JCompiler.CodeGen;


namespace JCompiler.Utilities
{
    public class Compiler
    {
        
        public static  string Compile(string filePath, bool isBindWithThis=true) {

            try
            {
               
               JCompiler.Scanner.Scanner scanner = null;
                using (TextReader input = File.OpenText(filePath))
                {
                    scanner = new JCompiler.Scanner.Scanner(input);
                }

                JCompiler.Parser.Parser parser = new JCompiler.Parser.Parser(scanner.Tokens);
                JCompiler.Ast.Stmt result = parser.Result;
                CodeGenJavascript codeGen = new CodeGenJavascript(parser.Result, isBindWithThis);               
                return codeGen.Result;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }
    }
}
