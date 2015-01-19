using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Wrapper.Angular
{
    public delegate dynamic TwoParamReturnAny(IAngularEvent events, params object[] args);
    public interface IScope
    {
        object apply();
        object apply(string exp);
        object apply(Func<IScope, dynamic> exp);
        bool watch(string expression, TwoParamReturnVoid callbackFn);
        void on(string name, TwoParamReturnAny callbackFn);
        IAngularEvent broadcast(string name, params object[] args);
    }

    public interface IAngularEvent
    {
        IScope targetScope { get; set; }
        IScope currentScope { get; set; }
        string name { get; set; }
        Func<dynamic, dynamic> preventDefault { get; set; }
        bool defaultPrevented { get; set; }
        Func<dynamic, dynamic> stopPropagation { get; set; }
    }
}
