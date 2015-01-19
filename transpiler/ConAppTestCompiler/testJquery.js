
(function(window){

var Testx=jsClass.extend({
AddEventListener:function()
{
alert("Super Add Event Listener");


}});
namespace('ConAppTestCompiler.Testx',Testx);
var TestJquery=ConAppTestCompiler.Testx.extend({
init:function()
{
this.AddEventListener();



},
AddEventListener:function()
{
this._super();
var that=this;
jQuery("#btn-1").click(function(){that.LoadData();

});


},
LoadData:function()
{
var studentList=[{Id:101,Name:"Jasim",Address:"Tangail"},{Id:102,Name:"Abdulla",Address:"Tangail"},{Id:103,Name:"Abdur Rohman",Address:"Tangail"}];
var str=[];
studentList.forEach(function(item){
str.push("{0}!!!{1}".format(item.Name,item.Address));


});
alert(str.join(""));

var html=[];
studentList.forEach(function(item,index){html.push("<tr>");

html.push("<td>{0}</td><td>{1}</td><td>{2}</td>".format(item.Id,item.Name,item.Address));

html.push("</tr>");

});

jQuery("#result").html(html.join("")).animate({width:"70%",opacity:0.4,marginLeft:"0.6in",fontSize:"3em"},1500);


}});
namespace('ConAppTestCompiler.TestJquery',TestJquery);
})(window);