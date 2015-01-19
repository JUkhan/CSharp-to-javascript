using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JCompiler;
using System.IO;
using System.Windows.Forms;

namespace jsEmitter
{
    class Program
    {
        static string nameof_ALLAH() {
            string res = 
@"--------------------------------
----------          ------------
---------  BISMILLA  -----------
----------          ------------
--------------------------------";

            return res;
        }
         [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new EmitterForm());
            }
            else
            {
                Console.WriteLine(nameof_ALLAH());
                Console.WriteLine("Welcome to jsEmitter.");
                bool flag = true;

                while (flag)
                {
                    Console.Write(">>");
                    string[] input = Console.ReadLine().Split(new char[] { ' ' });
                    if (input.Length == 1 && input[0].ToLower() == "exit")
                    {
                        flag = false;
                    }
                    else if (input.Length == 2 && input[0].ToLower() == "compile")
                    {
                        Compile(input[1]);
                    }
                    else if (input.Length == 3 && input[0].ToLower() == "compile")
                    {
                        CompileAndSave(input[1], input[2]);
                    }
                }
            }

        }

        static void Compile(string fileName)
        {

            try
            {
                string res = JCompiler.Utilities.Compiler.Compile(fileName);
                Console.WriteLine(res);
                Console.Beep();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        static void CompileAndSave(string fileName, string outputFilePath)
        {
            try
            {
                string res = JCompiler.Utilities.Compiler.Compile(fileName);
                File.WriteAllText(outputFilePath, res);
                Console.Beep();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
