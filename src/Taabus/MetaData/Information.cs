using System;
using System.Collections.Generic;
using System.Linq;
using hw.DebugFormatter;
using hw.Helper;

namespace Taabus.MetaData
{
    sealed class Information : DumpableObject
    {
        readonly SQLSysViews _sqlSysViews;
        readonly FunctionCache<string, CompountType> _compountTypeCache;

        public Information(IDataProvider provider)
        {
            _sqlSysViews = Profiler.Measure(() => new SQLSysViews(provider));
            _compountTypeCache = new FunctionCache<string, CompountType>(GetCompountType);
        }

        internal CompountType[] CompountTypes
        {
            get
            {
                var allObjectsClasses = _sqlSysViews.CompountTypes;
                var compountTypes = Profiler.Measure(() => allObjectsClasses.Select(t => _compountTypeCache[t.name]));
                return Profiler.Measure(() => compountTypes.ToArray());
            }
        }

        CompountType GetCompountType(string name)
        {
            var type = _sqlSysViews
                .all_objects
                .Single(t => t.name == name);
            return new CompountType(type);
        }
    }

    sealed class ObjectType : EnumEx
    {
        public static readonly ObjectType Check = new ObjectType("C ");
        public static readonly ObjectType ForeignKey = new ObjectType("F ");
        public static readonly ObjectType PrimaryKey = new ObjectType("PK");
        public static readonly ObjectType UniqueIndex = new ObjectType("UQ");
        public static readonly ObjectType InternalTable = new ObjectType("IT", isCompountType: true);
        public static readonly ObjectType Table = new ObjectType("U ", isCompountType: true);
        public static readonly ObjectType View = new ObjectType("V ", isCompountType: true);
        public static readonly ObjectType SystemBaseTable = new ObjectType("S ", isCompountType: true);
        public static readonly ObjectType ServiceQueue = new ObjectType("SQ");
        public static readonly ObjectType Default = new ObjectType("D ");
        public static readonly ObjectType SQLStoredProcedure = new ObjectType("P ");
        public static readonly ObjectType ExtendedStoredProcedure = new ObjectType("X ");
        public static readonly ObjectType SQLScalarFunction = new ObjectType("FN");
        public static readonly ObjectType SQLInlineTableValuedFunction = new ObjectType("IF");
        public static readonly ObjectType SQLTableValuedFunction = new ObjectType("TF");
        public static readonly ObjectType AggregateFunction = new ObjectType("AF");
        public static readonly ObjectType CLRScalarFunction = new ObjectType("FS");
        public static readonly ObjectType CLRStoredProcedure = new ObjectType("PC");
        public static readonly ObjectType Trigger = new ObjectType("TR");
        public static readonly ObjectType TableType = new ObjectType("TT");
        public static readonly ObjectType Synonym = new ObjectType("SN");

        internal readonly string Name;
        internal readonly bool IsCompountType;

        ObjectType(string name, bool isCompountType = false)
        {
            Name = name;
            IsCompountType = isCompountType;
        }

        public static IEnumerable<ObjectType> All { get { return AllInstances<ObjectType>(); } }
    }
}