using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Wrapper.JS
{
    public delegate void TwoParamReturnVoid<T>(T newValue, int index);
    public delegate bool PredicateTakeItem<T>(T newValue);
    public delegate bool PredicateTakeItems<T1, T2>(T1 item1, T2 item2);
    public delegate object SelectItemCallback<T>(T newValue);
    public delegate object SelectItemsCallback<T1, T2>(T1 item1, T2 item2);
	public static class Extension
	{

        public static T ToJsonObject<T>(this Dictionary<object, object> dic)
        {
            return default(T);
        }

        public static T ToJsonObject<T>(this Dictionary<string, object> dic)
        {
            return default(T);
        }
        public static void ForEach<T>(this IEnumerable<T> list,TwoParamReturnVoid<T> callbackFn){
    
         }
        public static List<T>  remove<T>(this IEnumerable<T> list, PredicateTakeItem<T> callbackFn)
        {
            return null;
        }
       
       
        public static List<T> select<T>(this IEnumerable<T> list, PredicateTakeItem<T> callbackFn, SelectItemCallback<T> selectItemCallback)
        {
            return null;
        }
       
        public static List<T> selectWithJoin<T, T2>(this IEnumerable<T> list, IEnumerable<T2> list2, PredicateTakeItems<T, T2> list2Condition, PredicateTakeItems<T, T2> whereCondition, SelectItemsCallback<T, T2> selectItemCallback)
        {
            return null;
        }
        public static List<T> paging<T>(this IEnumerable<T> list, int page, int size)
        {
            return null;
        }

        public static List<KeyItems> groupBy<T>(this IEnumerable<T> list, Func<T, object> callback)
        {
            return null;
        }
        public static string Join<T>(this IEnumerable<T> list, string args)
        {
            return null;
        }
        public static void push<T>(this IEnumerable<T> list, T item)
        {
            
        }
        public static T pop<T>(this IEnumerable<T> list)
        {
            return default(T);
        }
        public static List<T> reverse<T>(this IEnumerable<T> list)
        {
            return null;
        }
        public static T shift<T>(this IEnumerable<T> list)
        {
            return default(T); ;
        }
        public static int unshift<T>(this IEnumerable<T> list, params object[] args)
        {
            return 0;
        }
        public static List<T> slice<T>(this IEnumerable<T> list, int start, int end)
        {
            return null;
        }
        public static List<T> sort<T>(this IEnumerable<T> list, Func<T, T, object> callback)
        {
            return null;
        }
        public static List<T> sort<T>(this IEnumerable<T> list)
        {
            return null;
        }
        //String
        public static string substr(this string str, int startIndex, int length)
        {
            return null;
        }
        public static string substring(this string str, int startIndex )
        {
            return null;
        }
        public static string substring(this string str, int startIndex, int endIndex)
        {
            return null;
        }
        public static string format(this string str, params object[] rgs)
        {
            return null;
        }

        public static List<string> match(this string str, RegExp regex)
        {
            return null;
        }
        public static char charAt(this string str, int index)
        {
            return 'a';
        }
        public static int charCodeAt(this string str, int index)
        {
            return 0;
        }
        public static List<string> Split(this string str, string args)
        {
            return null;
        }
        public static List<string> Split(this string str, RegExp exp)
        {
            return null;
        }
        public static string toString(this object str)
        {
            return null;
        }
        public static string replace(this string str, RegExp regex, string newValue)
        {
            return null;
        }
        public static string replace(this string str, RegExp regex, Func<string, string, int, string, string> callback)
        {//match, contents, offset, s
            return null;
        }
        public static string replace(this string str, RegExp regex, Func<string, string, string> callback)
        {//match, contents, offset, s
            return null;
        }
	}
}
