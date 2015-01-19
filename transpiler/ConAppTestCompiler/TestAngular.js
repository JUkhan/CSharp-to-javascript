
(function(window){

var Mac=jsClass.extend({
init:function(x)
{
console.log("SUPER->",x);



}});
namespace('ConApp.Mac',Mac);

var TestAngular=ConApp.Mac.extend({
scope:null,
dic:new Dictionary(),
listz:null,
print:function(data)
{
var sdfr=this.dic.ToList();
console.log(data);


},
init:function(scope,http)
{
this._super(12);
var sdx=(function(){var _x=new Dictionary();_x["asd"]={Id:101,Name:"Arif"};_x["asd23"]={Id:102,Name:"Shohel"};return _x;})().ToJsonObject();
console.log(sdx);

this.scope=scope;

this.scope.msg="Hello World";

var that=this;
this.scope.on("suna",function(e,args){this.print(that.TestFunc(args.toString())());


return false;

}.bind(this));

var list=["one","two","three","five"];
list.remove(function(item){return item.Length>3;}.bind(this)).ForEach(function(item,index){this.print(item);

}.bind(this));

this.dic.Add("one",[{Name:"jasim",Address:"Tangail"},{Name:"ripon",Address:"Tangail"},{Name:"manik",Address:"Dhaka"},{Name:"jake",Address:"Tangail"},{Name:"abdulla",Address:"Dhaka"}]);

this.dic.Add("two",[{Name:"jasim",Address:"Tangail"},{Name:"ripon",Address:"Tangail"},{Name:"manik",Address:"Dhaka"},{Name:"jake",Address:"Tangail"},{Name:"abdulla",Address:"Dhaka"}]);

console.log("------------------------Dictionary----------------------------");


if(this.dic.ContainsKey("one")){this.dic["one"].groupBy(function(item1){return item1.Address;}.bind(this)).ForEach(function(item2,index){console.log(item2.key);

(item2.items).ForEach(function(item,index2){console.log("{0}-{1}".format(item.Name,item.Address.substr(0,2)));

}.bind(this));

}.bind(this));

}
console.log("-----------------------End-Dictionary----------------------------");

"qw,we,er,ty".split(new RegExp(",")).ForEach(function(item,inddex){console.log(item,inddex);

}.bind(this));

alert([1,2,3,4,5,6,7].Join("-"));

this.dic["one"].select(function(u){return (u.Address.ToLower()=="dhaka");},function(u){ return {fullName:"{0}-{1}".format(u.Name,u.Id),address:u.Address}; }).ForEach(function(item,index){alert("{0}-{1}".format(item.fullName,item.address));

}.bind(this));

var str="<wiki>jasim</wiki><wiki>Abdur rohman</wiki>".replace(new RegExp("<wiki>(.+?)</wiki>","g"),function(match,content){
return "<a href='wiki/{0}'>{1}</a>".format(content.replace(new RegExp("\\s+","g"),"_"),content);

}.bind(this));
var sdfr=this.dic.ToList();
console.log(str);

this.checkJoinQuery();

this.dic.Clear();

this.dic.ForEach(function(x,i){console.log(x.Key);

}.bind(this));



},
checkJoinQuery:function()
{
var dList=[{DepartmentId:1,Name:"BBA"},{DepartmentId:2,Name:"CSE"},{DepartmentId:3,Name:"MBA"}];
var sList=[{Id:101,DepartmentId:1,Name:"jasim",Address:"Tangail"},{Id:102,DepartmentId:2,Name:"ripon",Address:"Tangail"},{Id:103,DepartmentId:3,Name:"manik",Address:"Dhaka"},{Id:104,DepartmentId:1,Name:"jake",Address:"Tangail"},{Id:105,DepartmentId:2,Name:"abdulla",Address:"Dhaka"}];
console.log("------------------joinQueryResult------------------------");

var list=dList.selectWithJoin(sList, function(d, s){ return d.DepartmentId==s.DepartmentId; },function(){return !0;},function(d, s){ return {dName:d.Name,sName:s.Name,address:s.Address}; });
list.sort().reverse().ForEach(function(item,index){console.log(item);

}.bind(this));

list.forEach(function(item){
console.log(item);

}.bind(this));

},
showMsg:function(msg)
{
this.scope.broadcast("suna",msg.format(123,"007"),123);


},
getObj:function()
{

return {name:"jasim"};


},
TestFunc:function(msg)
{
var fx=function(){
return msg.ToUpper();

}.bind(this);

return fx;


}});
namespace('ConApp.TestAngular',TestAngular);

})(window);