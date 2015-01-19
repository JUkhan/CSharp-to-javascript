using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharp.Wrapper.JS;
using CSharp.Wrapper.Angular;
using ConAppTestCompiler;

namespace ConApp
{
    public interface TestScope : IRootScope
    {
        string msg { get; set; }
    }
    public class Mac : jsClass {
        public Mac(int x) {
            console.log("SUPER->",x);
        }
    }
    
    public class TestAngular : Mac
    {
        private TestScope scope = null;
        private Dictionary<string, List<Student>> dic = new Dictionary<string, List<Student>>();
        private List<Student> listz = null;
        private void print(object data)
        {
            var sdfr = this.dic.ToList<KeyValuePair<string, List<Student>>>();
            console.log(data);
        }
        public TestAngular(TestScope scope, HttpService http):base(12)
        {

            dynamic sdx = new Dictionary<string, object> { { "asd", new Student { Id = 101, Name = "Arif" } }, { "asd23", new Student { Id = 102, Name = "Shohel" } } }.ToJsonObject<object>();
           // sdx.ForEach((x, i) => {
                console.log(sdx);
            //});
            this.scope = scope;
            this.scope.msg = "Hello World";
            var that = this;
            this.scope.on("suna", (e, args) =>
            {
                this.print(that.TestFunc(args.toString())());
                return false;
            });

            List<string> list = new List<string>() { "one", "two", "three", "five" };
           
          
           list.remove(item => item.Length>3).ForEach((item, index) =>
            {
                this.print(item);
            });

            this.dic.Add("one", new List<Student>()
            {
                new Student{ Name="jasim", Address="Tangail"},
                new Student{ Name="ripon", Address="Tangail"},
                new Student{ Name="manik", Address="Dhaka"},
                new Student{ Name="jake", Address="Tangail"},
                new Student{ Name="abdulla", Address="Dhaka"}
            });
            this.dic.Add("two", new List<Student>()
            {
                new Student{ Name="jasim", Address="Tangail"},
                new Student{ Name="ripon", Address="Tangail"},
                new Student{ Name="manik", Address="Dhaka"},
                new Student{ Name="jake", Address="Tangail"},
                new Student{ Name="abdulla", Address="Dhaka"}
            });
            console.log("------------------------Dictionary----------------------------");
            if (this.dic.ContainsKey("one"))
            {
                this.dic["one"].groupBy((item1) => item1.Address).ForEach((item2, index) =>
                {
                    console.log(item2.key);                   
                    ((List<Student>)item2.items).ForEach((item, index2) =>
                    {
                        console.log("{0}-{1}".format(item.Name, item.Address.substr(0, 2)));
                    });

                });
            }
           
            console.log("-----------------------End-Dictionary----------------------------");
            "qw,we,er,ty".Split(new RegExp(",")).ForEach((item, inddex) =>
            {
                console.log(item, inddex);
            });
            alert(new List<int> { 1, 2, 3, 4, 5, 6, 7 }.Join("-"));

            (from u in this.dic["one"] where u.Address.ToLower()=="dhaka" select new { fullName="{0}-{1}".format(u.Name, u.Id), address=u.Address })            
            .ForEach((item, index) =>
            {
                alert("{0}-{1}".format(item.fullName, item.address));
            });

            var str = "<wiki>jasim</wiki><wiki>Abdur rohman</wiki>".replace(new RegExp("<wiki>(.+?)</wiki>", "g"), (match, content) =>
            {
                return "<a href='wiki/{0}'>{1}</a>".format(content.replace(new RegExp("\\s+","g"), "_"), content);
            });
            var sdfr = this.dic.ToList<KeyValuePair<string, List<Student>>>();
            
            console.log(str);
            this.checkJoinQuery();
            this.dic.Clear();
            this.dic.ForEach((x, i) => { console.log(x.Key); });
        }
        private void checkJoinQuery() {
            List<Department> dList = new List<Department> { 
            new Department{DepartmentId=1, Name="BBA"},
            new Department{DepartmentId=2, Name="CSE"},
            new Department{DepartmentId=3, Name="MBA"},
            };
            List<Student> sList = new List<Student> { 
             new Student{ Id=101, DepartmentId=1, Name="jasim", Address="Tangail"},
                new Student{ Id=102, DepartmentId=2, Name="ripon", Address="Tangail"},
                new Student{Id=103, DepartmentId=3, Name="manik", Address="Dhaka"},
                new Student{Id=104, DepartmentId=1, Name="jake", Address="Tangail"},
                new Student{Id=105, DepartmentId=2, Name="abdulla", Address="Dhaka"}
            };
            console.log("------------------joinQueryResult------------------------");
            var list = (from d in dList
                        join s in sList on d.DepartmentId equals s.DepartmentId                        
                        //where d.Name!="CSE"
                        select new { dName = d.Name, sName = s.Name, address = s.Address }
                          );
            list.sort().reverse().ForEach((item, index) => { console.log(item); });
            foreach (var item in list)
            {
                console.log(item);
            }
            
        }
        public void showMsg(string msg)
        {

            this.scope.broadcast("suna", msg.format(123, "007"), 123);

        }
        protected object getObj() {

            return new { name = "jasim" };
        }
        public Func<string> TestFunc(string msg)
        {
            Func<string> fx = () =>
            {
                return msg.ToUpper();
            };
            return fx;
        }
    }
}
