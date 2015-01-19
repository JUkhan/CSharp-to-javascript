
(function(window){

var Testq=jsClass.extend({
DoSMth:function()
{
var fx=function(x){
return "KV"+x;

}.bind(this);
navigator.camera.getPicture(function(data){}.bind(this),function(message){}.bind(this),{quality:79,destinationType:Camera.DestinationType.DATA_URL,sourceType:Camera.PictureSourceType.SAVEDPHOTOALBUM,encodingType:Camera.EncodingType.JPEG,popoverOptions:{arrowDir:Camera.PopoverArrowDirection.ARROW_ANY}});


},
doSmth:function(fx)
{

return fx("KravMaga")==101;


}});
namespace('ConAppTestCompiler.Testq',function(){return new Testq();});
var Masum=jsClass.extend({
scope:null,
sqLight:null,
init:function($scope)
{
this.scope=$scope;

this.scope.name="";

this.scope.address="";

this.scope.list=null;

this.scope.listGroup=null;

SQLight.setDbOptions("sqloght_helloworld","1.0","SQLight Hello World",1024);

this.sqLight=new SQLight();

this.sqLight.query("CREATE TABLE IF NOT EXISTS Students ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL , Address TEXT NOT NULL );",[]);

this.loadAllStudent();

this.DoSMth();

console.log(this.doSmth(function(x){
return 107;

}.bind(this)));



this.scope.AddStudent=this.AddStudent.bind(this);
},
DoSMth:function()
{
var fx=function(x){
return "KV"+x;

}.bind(this);
console.log(fx(12));


},
doSmth:function(fx)
{

return !false;


},
AddStudent:function()
{
this.sqLight.query("INSERT INTO Students (Name, Address) VALUES ( ?, ? )",[this.scope.name,this.scope.address]).success(function(res){this.loadAllStudent();

}.bind(this));


},
loadAllStudent:function()
{
this.sqLight.query("SELECT * FROM Students order by Name",[]).success(function(res){this.scope.list=SQLight.getList(res);

this.scope.listGroup=this.scope.list.groupBy(function(x){var val=x.Address;

switch(val.ToLower())
{

case 'tangail':val="TANGAIL";
break;

}
return val;

}.bind(this));

this.scope.$apply();

}.bind(this));


}});
namespace('ConAppTestCompiler.Masum',function($scope){return new Masum($scope);});
})(window);