using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Wrapper.JS;

namespace ConAppTestCompiler
{
    public class Testx : jsClass
    {

        public void AddEventListener()
        {
            alert("Super Add Event Listener");
        }
    }
    public class TestJquery : Testx
    {
        public TestJquery()
        {
            this.AddEventListener();
        }

        private void AddEventListener()
        {
            base.AddEventListener();
            var that = this;
            jQuery("#btn-1").click(delegate() { that.LoadData(); });
        }
        public void LoadData()
        {
            List<Student> studentList = new List<Student> { 
                new Student{Id=101, Name="Jasim", Address="Tangail"},
                new Student{Id=102, Name="Abdulla", Address="Tangail"},
                 new Student{Id=103, Name="Abdur Rohman", Address="Tangail"}
            };
            List<string> str = new List<string>();
            foreach (var item in studentList)
            {
                str.Add("{0}!!!{1}".format(item.Name, item.Address));
            }

            alert(str.Join(""));
            List<String> html = new List<string>();
            studentList.ForEach((item, index) =>
                {
                    html.Add("<tr>");
                    html.Add("<td>{0}</td><td>{1}</td><td>{2}</td>".format(item.Id, item.Name, item.Address));
                    html.Add("</tr>");
                });

            jQuery("#result")
                .html(html.Join(""))
                .animate(new Style { width = "70%", opacity = 0.4, marginLeft = "0.6in", fontSize = "3em" }, 1500);

        }
    }
}
