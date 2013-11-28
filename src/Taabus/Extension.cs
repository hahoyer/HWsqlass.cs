using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using hw.Helper;

namespace Taabus
{
    static class Extension
    {
        internal static IEnumerable<TreeNode> _(this TreeNodeCollection value) { return value.Cast<TreeNode>(); }
        internal static IEnumerable<Control> _(this Control.ControlCollection value) { return value.Cast<Control>(); }
        internal static IEnumerable<TreeNode> SelectHierachical(this TreeView tree)
        {
            return tree
                .Nodes
                ._()
                .SelectMany(n => n.SelectHierachical(nn => nn.Nodes._()));
        }

        internal static T Invoke<T>(this object target, string method, params object[] args) { return (T) target.GetType().InvokeMember(method, BindingFlags.InvokeMethod, null, target, args); }
        internal static T Invoke<T>(this Type type, string method, params object[] args) { return ExceptionGuard(() => (T) type.InvokeMember(method, BindingFlags.InvokeMethod, null, null, args)); }

        internal static T ExceptionGuard<T>(this Func<T> function)
        {
            try
            {
                return function();
            }
            catch(Exception e)
            {
                return default(T);
            }
            ;
        }

        internal static T ThreadCallGuard<T>(this Control control, Func<T> function)
        {
            if(control.InvokeRequired)
                return (T) control.Invoke(function);
            return function();
        }

        internal static void ThreadCallGuard(this Control control, Action function)
        {
            if(control.InvokeRequired)
                control.Invoke(function);
            else
                function();
        }
    }
}