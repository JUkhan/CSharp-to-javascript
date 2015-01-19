using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharp.Wrapper.Angular;
using JsMakerLib.JQuery;

namespace CSharp.Wrapper.JS
{
    public class jsClass:window
    {
        public jsClass() { }

       protected jQuery jQuery(object selector) { return null; }
        protected JNumber Number(object val) { return null; }
        protected JConsole console { get; set; }
        protected IScope GetParentScope(IScope scope) { return null; }
        protected IScope GetScopeRoot(IScope scope) { return null; }
        protected bool confirm(string nessage) { return false; }
        protected void SetScopePropValue(IScope scope, string propName, object value) { }
        public void init(IScope scope) { }
    }

    public class JNumber {
        public object toFixed(int fractionDigit) { return null; }
        public object toExponential(int fractionDigit) { return null; }
        public string toLocaleString() { return ""; }
    }

    public class JConsole {
        public void log(params object[] args){}
    }

    public class KeyItems {
        public dynamic key { get; set; }
        public dynamic items { get; set; }
    }
    public class RegExp
    {
        public RegExp(object expre) { }
        public RegExp(object expre, string scope) { }

        public List<string> exec(object stringValue) { return null; }
        public bool test(object input) { return false; }
    }
    public class window
    {
        public window() { }

        public static string location { get; set; }

        public static void alert(object msg) { }
        public static void clearInterval(string intervalId) { }
        public static void clearTimeout(string timeoutId) { }
        public static float parseFloat(object val) { return 0; }
        public static int parseInt(object val) {return 0; }
        public static string setInterval(EmptyDelegate code, int interval) { return ""; }
        public static string setTimeout(EmptyDelegate code, int interval) { return ""; }
    }
    public class HTMLElement
    {
        public HTMLElement() { }

        public bool Checked { get; set; }
        public object children { get; set; }
        public string classx { get; set; }
        public string cls { get; set; }
        public string href { get; set; }
        public string html { get; set; }
        public string id { get; set; }
        public string innerHTML { get; set; }
        public object name { get; set; }
        public int offsetHeight { get; set; }
        public int offsetLeft { get; set; }
        public int offsetTop { get; set; }
        public int offsetWidth { get; set; }
        public string src { get; set; }
        public Style style { get; set; }
        public string tag { get; set; }
        public string value { get; set; }

        public void mask(object str) { }
        public void unmask() { }
    }
    public sealed class Style
    {
        public Style() { }

        public bool accelerator { get; set; }
        public string background { get; set; }
        public string backgroundAttachment { get; set; }
        public string backgroundColor { get; set; }
        public string backgroundImage { get; set; }
        public string backgroundPosition { get; set; }
        public string backgroundPositionX { get; set; }
        public string backgroundPositionY { get; set; }
        public string backgroundRepeat { get; set; }
        public string border { get; set; }
        public string borderBottom { get; set; }
        public string borderBottomColor { get; set; }
        public string borderBottomStyle { get; set; }
        public string borderBottomWidth { get; set; }
        public string borderCollapse { get; set; }
        public string borderColor { get; set; }
        public string borderLeft { get; set; }
        public string borderLeftColor { get; set; }
        public string borderLeftStyle { get; set; }
        public string BorderLeftWidth { get; set; }
        public string borderRight { get; set; }
        public string borderRightColor { get; set; }
        public string borderRightStyle { get; set; }
        public string borderRightWidth { get; set; }
        public string borderStyle { get; set; }
        public string borderTop { get; set; }
        public string borderTopColor { get; set; }
        public string borderTopStyle { get; set; }
        public string borderTopWidth { get; set; }
        public string borderWidth { get; set; }
        public string bottom { get; set; }
        public string clear { get; set; }
        public string clip { get; set; }
        public string color { get; set; }
        public string cssText { get; set; }
        public string cursor { get; set; }
        public string direction { get; set; }
        public string display { get; set; }
        public string filter { get; set; }
        public string font { get; set; }
        public string fontFamily { get; set; }
        public string fontSize { get; set; }
        public string fontStyle { get; set; }
        public string fontVariant { get; set; }
        public string fontWeight { get; set; }
        public object height { get; set; }
        public object left { get; set; }
        public string letterSpacing { get; set; }
        public string lineHeight { get; set; }
        public string listStyle { get; set; }
        public string listStyleImage { get; set; }
        public string listStylePosition { get; set; }
        public string listStyleType { get; set; }
        public string margin { get; set; }
        public string marginBottom { get; set; }
        public string marginLeft { get; set; }
        public string marginRight { get; set; }
        public string marginTop { get; set; }
        public string maxHeight { get; set; }
        public string maxWidth { get; set; }
        public string minHeight { get; set; }
        public string minWidth { get; set; }
        public string msInterpolationMode { get; set; }
        public object opacity { get; set; }
        public string overflow { get; set; }
        public string overflowX { get; set; }
        public string overflowY { get; set; }
        public string padding { get; set; }
        public string paddingBottom { get; set; }
        public string paddingLeft { get; set; }
        public string paddingRight { get; set; }
        public string paddingTop { get; set; }
        public string pageBreakAfter { get; set; }
        public string pageBreakBefore { get; set; }
        public int pixelBottom { get; set; }
        public int pixelHeight { get; set; }
        public int pixelLeft { get; set; }
        public int pixelRight { get; set; }
        public int pixelTop { get; set; }
        public int pixelWidth { get; set; }
        public int posBottom { get; set; }
        public int posHeight { get; set; }
        public string position { get; set; }
        public int posLeft { get; set; }
        public int posRight { get; set; }
        public int posTop { get; set; }
        public int posWidth { get; set; }
        public string right { get; set; }
        public string styleFloat { get; set; }
        public string tableLayout { get; set; }
        public string textAlign { get; set; }
        public string textDecoration { get; set; }
        public string textDecorationBlink { get; set; }
        public string textDecorationLineThrough { get; set; }
        public string textDecorationNone { get; set; }
        public string textDecorationOverline { get; set; }
        public string textDecorationUnderline { get; set; }
        public string textIndent { get; set; }
        public string textJustify { get; set; }
        public string textOverflow { get; set; }
        public string textTransform { get; set; }
        public object top { get; set; }
        public string verticalAlign { get; set; }
        public string visibility { get; set; }
        public string whiteSpace { get; set; }
        public object width { get; set; }
        public string wordSpacing { get; set; }
        public string wordWrap { get; set; }
        public string writingMode { get; set; }
        public short zIndex { get; set; }
        public string zoom { get; set; }
    }
    public class document
    {
        public static document body;

        public document() { }

        public static Dictionary<object, object> form { get; set; }

        public static HTMLElement getElementById(string id) { return null; }
    }
    public delegate void EmptyDelegate();
}
