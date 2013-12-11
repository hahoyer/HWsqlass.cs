using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using hw.Debug;
using hw.Helper;
using Taabus.UserInterface;

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
            catch(Exception)
            {
                return default(T);
            }
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

        internal static Task<T> STAStart<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch(Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        internal static Task STAStart(Action func)
        {
            var result = new Task(func);
            var thread = new Thread(result.Start);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return result;
        }

        internal static bool Has(this int bitArray, int bit) { return (bitArray & bit) == bit; }

        const int ShiftKey = 4;
        const int AltKey = 32;
        const int ControlKey = 8;
        const int LeftMouseButton = 1;
        const int RightMouseButton = 2;
        const int MiddleMouseButton = 16;

        internal static bool SetEffect<T>(this DragEventArgs e, Func<T, bool> getIsValid, DragDropEffects defaultEffect)
        {
            e.Effect = Effect(e, getIsValid, defaultEffect);
            return e.Effect != DragDropEffects.None;
        }

        static DragDropEffects Effect<T>(DragEventArgs e, Func<T, bool> getIsValid, DragDropEffects defaultEffect)
        {
            if(e.IsValid(getIsValid))
            {
                if(e.KeyState.Has(AltKey) && e.AllowedEffect.HasFlag(DragDropEffects.Link))
                    return DragDropEffects.Link;
                if(e.KeyState.Has(ShiftKey) && e.AllowedEffect.HasFlag(DragDropEffects.Move))
                    return DragDropEffects.Move;
                if(e.KeyState.Has(ControlKey) && e.AllowedEffect.HasFlag(DragDropEffects.Copy))
                    return DragDropEffects.Copy;
                if(e.AllowedEffect.HasFlag(defaultEffect))
                    return defaultEffect;
            }

            return DragDropEffects.None;
        }

        internal static bool IsValid<T>(this DragEventArgs e, Func<T, bool> getIsValid)
        {
            return e.Data.GetDataPresent(typeof(T))
                && getIsValid((T) e.Data.GetData(typeof(T)));
        }

        internal static bool SetEffectCopy(this DragEventArgs e, bool isValid)
        {
            Tracer.Assert(e.AllowedEffect == DragDropEffects.Copy);

            e.Effect = isValid && e.KeyState.Has(ControlKey) ? DragDropEffects.Copy : DragDropEffects.None;
            return e.Effect != DragDropEffects.None;
        }

        internal static Bitmap AsBitmap(this Control c)
        {
            var result = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(result, new Rectangle(0, 0, c.Width, c.Height));
            return result;
        }

        internal static Cursor ToCursor(this Control control, Point? hotSpot = null)
        {
            var iconInfo = new CursorUtil.IconInfo(control.AsBitmap());
            if(hotSpot != null)
                iconInfo.HotSpot = hotSpot.Value;
            return iconInfo.Cursor;
        }
    }
}