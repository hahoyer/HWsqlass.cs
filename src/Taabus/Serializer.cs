using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using hw.Debug;
using hw.Helper;
using JetBrains.Annotations;

namespace Taabus
{
    class Serializer : Dumpable
    {
        public static string Serialize(object data) { return Serialize(data.GetType(), data); }
        static string Serialize(Type type, object data)
        {
            var enable = type.GetAttribute<Enable>(false);
            if(enable != null)
                return Serialize(enable, type, data);

            var xl = data as IList;
            if(xl != null)
                return "new " + type.CompleteName() + " " + Serialize(xl).Surround("{", "}");

            if(data is string)
                return data.ToString().Quote();

            if(data is bool)
                return ((bool) data) ? "true" : "false";

            var xd = data as IDictionary;
            if(xd != null)
                NotImplementedFunction(type, data);

            var xc = data as ICollection;
            if(xc != null)
                NotImplementedFunction(type, data);

            var co = data as CodeObject;
            if(co != null)
                NotImplementedFunction(type, data);

            var xt = data as Type;
            if(xt != null)
                NotImplementedFunction(type, data);

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

        static string Serialize(Enable enable, Type type, object data)
        {
            var memberData = enable
                .RelevantMembers(type)
                .Select(m => enable.Serialize(data, m));
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

        static bool IsRelevant(MemberInfo info)
        {
            return IsPossible(info)
                && info.GetAttribute<Disable>(false) == null;
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
        public class Enable : Attribute
        {
            public virtual string Serialize(object data, MemberInfo m) { return FormatMember(m, data); }

            public virtual IEnumerable<MemberInfo> RelevantMembers(Type type)
            {
                return type
                    .GetMembers(BindingFlags.Instance | BindingFlags.Public)
                    .Where(IsRelevant);
            }
        }

        [MeansImplicitUse]
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public sealed class Disable : Attribute
        {}

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
            where T: new ()
        {
            var result = new T();
            var a = typeof(T).GetAttribute<Enable>(false)
                .AssertNotNull()
                .RelevantMembers(typeof(T));
            foreach(var info in a)
                SetValue(info, result, GetValue(info, x));
            return result;
        }
    }
}