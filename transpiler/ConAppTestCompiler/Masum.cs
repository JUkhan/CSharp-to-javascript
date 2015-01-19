using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharp.Wrapper.JS;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.PhoneGap;

namespace ConAppTestCompiler
{
    public interface MasumScope:IScope
    {
        string name { get; set; }
        string address { get; set; }
        List<Student> list { get; set; }
        List<KeyItems> listGroup { get; set; }
    }

    public class Testq : jsClass
    {
        protected void DoSMth()
        {
            Func<int, string> fx = delegate(int x)
            {
                return "KV" + x;
            };
            navigator.camera.getPicture(data => { }, message => { },
                new CameraSettings
                {
                    quality = 79,
                    destinationType = Camera.DestinationType.DATA_URL,
                    sourceType = Camera.PictureSourceType.SAVEDPHOTOALBUM,
                    encodingType = Camera.EncodingType.JPEG,
                    popoverOptions = new CameraPopoverOptions { arrowDir = Camera.PopoverArrowDirection.ARROW_ANY }
                });
        }
        protected bool doSmth(Func<string, int> fx)
        {
            return fx("KravMaga") == 101;
        }
    }
    public class Masum : jsClass
    {
        MasumScope scope = null;
        SQLight sqLight = null;

        public Masum(MasumScope _s_scope)
        {

            this.scope = _s_scope;
            this.scope.name = "";
            this.scope.address = "";
            this.scope.list = null;
            this.scope.listGroup = null;
           
            SQLight.setDbOptions("sqloght_helloworld", "1.0", "SQLight Hello World", 1024);
            this.sqLight = new SQLight();
            this.sqLight.query("CREATE TABLE IF NOT EXISTS Students ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL , Address TEXT NOT NULL );", new List<object>());

            this.loadAllStudent();
            this.DoSMth();
            console.log(this.doSmth(x =>
            {
                return 107;
            }));
            
            
        }
        protected void DoSMth()
        {
           
            
           // base.DoSMth();

            Func<int, string> fx = delegate(int x)
            {
                return "KV" + x;
            };
            console.log(fx(12));
        }
        protected bool doSmth(Func<string, int> fx)
        {
            return !false;
        }
        public void AddStudent()
        {

            this.sqLight.query("INSERT INTO Students (Name, Address) VALUES ( ?, ? )", new List<object>() { this.scope.name, this.scope.address })
               .success(res =>
               {
                   this.loadAllStudent();
               });
        }

        private void loadAllStudent()
        {

            this.sqLight.query("SELECT * FROM Students order by Name", new List<object>())
                 .success(res =>
                 {
                     this.scope.list = SQLight.getList<Student>(res);
                     
                     this.scope.listGroup = this.scope.list.groupBy(x =>
                     {
                         string val = x.Address;
                         switch (val.ToLower())
                         {
                             case "tangail":
                                 val = "TANGAIL";
                                 break;

                         }
                         return val;

                     });
                     this.scope.apply();

                 });

        }
    }
}
