using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using hw.Debug;
using hw.Helper;
using JetBrains.Annotations;

namespace Taabus
{
    class Serializer : Dumpable
    {
        static readonly Dictionary<Type, string[]> _relevantMembers = new Dictionary<Type, string[]>
        {
            {typeof(Rectangle), new[] {"X", "Y", "Height", "Width"}}
        };

        public static string Serialize(object data) { return Serialize(data.GetType(), data); }
        static string Serialize(Type type, object data)
        {
            if(data == null)
                return "null";

            var enable = type.GetAttribute<Enable>(false);
            if(enable != null)
                return Serialize(type, data, RelevantMembers(type, data));

            var xl = data as IList;
            if(xl != null)
                return "new " + type.CompleteName() + " " + Serialize(xl).Surround("{", "}");

            if(data is string)
                return data.ToString().Quote();

            if(data is bool)
                return ((bool) data) ? "true" : "false";

            var xt = data as Type;
            if(xt != null)
                return "typeof(" + xt.CompleteName() + ")";

            if(type.IsPrimitive)
                return data.ToString();

            if(_relevantMembers.ContainsKey(type))
                return Serialize(type, data, RelevantMembers(type, data, relevantMemberNames: _relevantMembers[type]));

            NotImplementedFunction(type, data);
            return null;
        }

        static string Serialize(IList data)
        {
            return data
                .Cast<object>()
                .Select(Serialize)
                .Stringify(",\n");
        }

        static string Serialize(Type type, object data, IEnumerable<MemberInfo> relevantMembers)
        {
            var memberData = relevantMembers
                .Select(m => FormatMember(m, data));
            var name = type.CompleteName();
            return "new " + name + " " + memberData.Stringify(",\n").Surround("{", "}");
        }

        static string FormatMember(MemberInfo info, object data)
        {
            var value = GetValue(info, data);
            return info.Name
                + " = "
                + FormatValue(value);
        }

        static object GetValue(MemberInfo info, object data)
        {
            return info is PropertyInfo
                ? ((PropertyInfo) info).GetValue(data)
                : ((FieldInfo) info).GetValue(data);
        }

        static void SetValue(MemberInfo info, object data, object value)
        {
            var propertyInfo = info as PropertyInfo;
            if(propertyInfo == null)
                ((FieldInfo) info).SetValue(data, value);
            else
                propertyInfo.SetValue(data, value);
        }

        static string FormatValue(object data) { return Serialize(data).Indent(); }

        static bool IsRelevant(MemberInfo info, object data)
        {
            if(!IsPossible(info))
                return false;
            if(info.GetAttribute<Disable>(false) != null)
                return false;
            var exceptions = info.GetAttributes<ExceptionFunctionAttribute>(false).ToArray();
            if(!exceptions.Any())
                return true;

            var value = GetValue(info,data);
            return !exceptions.Any(e => e.Check(value));
        }

        static bool IsPossible(MemberInfo info)
        {
            var p = info as PropertyInfo;
            if(p != null)
                return IsRelevant(p);

            var f = info as FieldInfo;
            if(f != null)
                return IsRelevant(f);

            return false;
        }

        static bool IsRelevant(PropertyInfo info) { return info.CanRead && info.CanWrite && info.GetMethod.IsPublic && info.SetMethod.IsPublic; }
        static bool IsRelevant(FieldInfo info) { return info.IsPublic; }

        [MeansImplicitUse]
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public sealed class Enable : Attribute
        {}

        [MeansImplicitUse]
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public sealed class Disable : Attribute
        {}

        [MeansImplicitUse]
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
        public abstract class ExceptionFunctionAttribute : Attribute
        {
            public abstract bool Check(object value);
        }

        public sealed class Except : ExceptionFunctionAttribute
        {
            readonly object _exception;
            public Except(object exception) { _exception = exception; }
            public override bool Check(object value) { return Equals(value, _exception); }
        }


        public new static bool Equals(object x, object y)
        {
            if(x == y)
                return true;
            var t = x.GetType();
            if(t != y.GetType())
                return false;
            return Serialize(x) == Serialize(y);
        }

        public static T Clone<T>(T x)
            where T : new()
        {
            var result = new T();
            var type = typeof(T);
            var a = RelevantMembers(type, x);
            foreach(var info in a)
                SetValue(info, result, GetValue(info, x));
            return result;
        }

        static IEnumerable<MemberInfo> RelevantMembers(Type type, object data, string[] relevantMemberNames = null)
        {
            var result = type
                .GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => IsRelevant(x, data));
            if(relevantMemberNames == null)
                return result;
            return result.Where(m => relevantMemberNames.Contains(m.Name));
        }
    }
}