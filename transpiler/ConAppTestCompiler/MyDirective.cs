using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
namespace ConAppTestCompiler
{
    [Angular(ActionName = "myDir", ActionType = "directive", ModuleName = "app", DI = "$sec")]
    public class MyDirective:BaseDirective
    {
        
        private int x = 23;
        public int y = 22;
        public int doSmth(int x) {
            return 12;
        }
        public MyDirective(dynamic scope) {
            var that = this;
        }

        public override ngDirective getDirective() {
            return null;
        }
    }
}
