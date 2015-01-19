
(function(window){

var Test=jsClass.extend({
init:function(h,d)
{
var list=[1,3,4,5,6];
list.push(100);

list.ForEach(function(x){console.log(x);

}.bind(this));

list.First(function(x){return x==2;}.bind(this));

list.Find(function(x){return x==2;}.bind(this));

list.Last(function(x){return x==2;}.bind(this));

list.FindLast(function(x){return x==2;}.bind(this));

list.Where(function(x){return x==2;}.bind(this));

jQuery.ajax({url:"",success:function(x){}.bind(this),error:function(b){}.bind(this),headers:{asd:123}});



},
mac:function(dx,dox)
{
dx=dx||"0";
var d=0;
d=d||34;
d=d==12 ? "345" : "007";
var x=0;
x=x||0;
dox=dox||34;
var dic=(function(){var $$$dic=new Dictionary();$$$dic["Aaasq"]="asasasas";$$$dic["2344"]={x:12,y:234};return $$$dic;})().ToJsonObject();

}});
namespace('ConApp.Test',Test);

})(window);