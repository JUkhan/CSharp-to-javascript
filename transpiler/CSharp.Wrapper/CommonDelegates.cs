using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Wrapper
{
    public delegate void EmptyParamReturnVoid();
    public delegate void TwoParamReturnVoid(dynamic newValue, dynamic oldValue);   
    public delegate void HttpCallback(dynamic data, int status);
}
