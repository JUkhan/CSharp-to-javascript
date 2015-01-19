using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler.Scanner;
using System.IO;
using JCompiler.Parser;
using JCompiler.Ast;

namespace ConAppTestCompiler
{
    class Program
    {
         
        static void Main(string[] args)
        {
            try
            {
                //Console.WriteLine(doSmth()(3, 5));
                string file = @"F:\projects\R2\ConAppTestCompiler\MyDirective.cs";
                file = @"F:\projects\JWT.Studio\jwtTest\AppScript\Services\StudentService.cs";
                string res = JCompiler.Utilities.Compiler.Compile(file);
               // File.WriteAllText(@"F:\projects\IonicMobile\IonicMobile\IonicMobile\js\StarterConfig.js", res);
                //File.WriteAllText(@"F:\projects\R2\ConAppTestCompiler\TestAngular.js", res);
                Console.WriteLine(res);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static Func<int, int, int> doSmth() {

            return (a, b) => a + b;
        }
    }
}
