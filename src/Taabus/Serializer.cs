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
    sealed class Serializer : Dumpable
    {
        public string Serialize(object data) { return Serialize(data.GetType(), data); }
        string Serialize(Type type, object data)
        {
            var @class = type.GetAttribute<Class>(true);
            if(@class != null)
                return @class.SerializeClass(this, type, data);

            var xl = data as IList;
            if(xl != null)
                return "new " + type.CompleteName() + " " + Serialize(xl).Surround("{", "}");

            if (data is string)
                return data.ToString().Quote();

            if (data is bool)
                return ((bool)data) ? "true" : "false";

            var xd = data as IDictionary;
            if(xd != null)
                NotImplementedMethod(type, data);

            var xc = data as ICollection;
            if(xc != null)
                NotImplementedMethod(type, data);

            var co = data as CodeObject;
            if(co != null)
                NotImplementedMethod(type, data);

            var xt = data as Type;
            if(xt != null)
                NotImplementedMethod(type, data);

            NotImplementedMethod(type, data);
            return null;
        }

        string Serialize(IList data)
        {
            return data
                .Cast<object>()
                .Select(Serialize)
                .Stringify(",\n");
        }

        string FormatMember(MemberInfo info, object data)
        {
            var value = info is PropertyInfo ? ((PropertyInfo) info).GetValue(data) : ((FieldInfo) info).GetValue(data);
            return info.Name
                + " = "
                + FormatValue(info, value);
        }

        string FormatValue(MemberInfo info, object data)
        {
            return info
                .GetAttribute<Member>(true)
                .AssertNotNull()
                .Serialize(data, this)
                .Indent();
        }

        static bool IsRelevant(MemberInfo info)
        {
            var a = info.GetAttribute<Member>(true);
            if(a == null)
                return false;

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
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class Member : Attribute
        {
            public virtual string Serialize(object data, Serializer serializer) { return serializer.Serialize(data); }
        }

        [MeansImplicitUse]
        [AttributeUsage(AttributeTargets.Class)]
        public class Class : Attribute
        {
            public virtual string SerializeClass(Serializer serializer, Type type, object data)
            {
                var formattedData = type
                    .GetMembers()
                    .Where(IsRelevant)
                    .Select(m => serializer.FormatMember(m, data))
                    .Stringify(",\n")
                    .Surround("{", "}");
                return "new " + type.CompleteName() + " " + formattedData;
            }
        }

        internal sealed class MissingClassAttribute : Exception
        {
            public Type Type { get; set; }
            public MissingClassAttribute(Type type) { Type = type; }
        }
    }
}