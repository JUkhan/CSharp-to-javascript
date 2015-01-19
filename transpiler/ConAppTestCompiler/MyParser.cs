using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConAppTestCompiler
{
    public class MyParser<T>:Utils.Pager<Student>
    {
        public MyParser(int size) : base(size) { 
        
        }
        public List<Student> Next() {
            return base.Next();
        }
    }
}
