#region Copyright (C) 2013

//     Project Taabus
//     Copyright (C) 2013 - 2013 Harald Hoyer
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;

namespace Taabus.MetaData
{
    sealed class BasicType : Type
    {
        static readonly HashSet<BasicType> _cache = new HashSet<BasicType>(_comparer);

        internal static Type GetInstance(SQLInformation.COLUMNSClass column)
        {
            var probe = new BasicType(
                column.IS_NULLABLE != "NO",
                column.DATA_TYPE,
                column.CHARACTER_MAXIMUM_LENGTH,
                column.CHARACTER_OCTET_LENGTH,
                column.NUMERIC_PRECISION,
                column.NUMERIC_PRECISION_RADIX,
                column.NUMERIC_SCALE,
                column.DATETIME_PRECISION,
                column.CHARACTER_SET_CATALOG,
                column.CHARACTER_SET_SCHEMA,
                column.CHARACTER_SET_NAME,
                column.COLLATION_CATALOG,
                column.COLLATION_SCHEMA,
                column.COLLATION_NAME,
                column.DOMAIN_CATALOG,
                column.DOMAIN_SCHEMA,
                column.DOMAIN_NAME
                );

            var result = _cache.FirstOrDefault(e => _comparer.Equals(e, probe));
            if(result != null)
                return result;
            _cache.Add(probe);
            return probe;
        }

        sealed class Comparer : IEqualityComparer<BasicType>
        {
            public bool Equals(BasicType x, BasicType y)
            {
                if(ReferenceEquals(x, y))
                    return true;
                if(ReferenceEquals(x, null))
                    return false;
                if(ReferenceEquals(y, null))
                    return false;
                if(x.GetType() != y.GetType())
                    return false;
                return x.IsNullable.Equals(y.IsNullable)
                    && string.Equals(x.DataType, y.DataType)
                    && x.CharacterOctetLength == y.CharacterOctetLength
                    && x.CharacterMaximumLength == y.CharacterMaximumLength
                    && x.NumericPrecision == y.NumericPrecision
                    && x.NumericScale == y.NumericScale
                    && x.NumericPrecisionRadix == y.NumericPrecisionRadix
                    && string.Equals(x.DomainName, y.DomainName)
                    && string.Equals(x.DomainSchema, y.DomainSchema)
                    && string.Equals(x.DomainCatalog, y.DomainCatalog)
                    && string.Equals(x.CollationName, y.CollationName)
                    && string.Equals(x.CollationSchema, y.CollationSchema)
                    && string.Equals(x.CollationCatalog, y.CollationCatalog)
                    && string.Equals(x.CharacterSetName, y.CharacterSetName)
                    && string.Equals(x.CharacterSetSchema, y.CharacterSetSchema)
                    && string.Equals(x.CharacterSetCatalog, y.CharacterSetCatalog)
                    && x.DatetimePrecision == y.DatetimePrecision;
            }
            public int GetHashCode(BasicType obj)
            {
                unchecked
                {
                    var hashCode = obj.IsNullable.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.DataType != null ? obj.DataType.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.CharacterOctetLength.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.CharacterMaximumLength.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.NumericPrecision.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.NumericScale.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.NumericPrecisionRadix.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.DomainName != null ? obj.DomainName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.DomainSchema != null ? obj.DomainSchema.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.DomainCatalog != null ? obj.DomainCatalog.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CollationName != null ? obj.CollationName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CollationSchema != null ? obj.CollationSchema.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CollationCatalog != null ? obj.CollationCatalog.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CharacterSetName != null ? obj.CharacterSetName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CharacterSetSchema != null ? obj.CharacterSetSchema.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CharacterSetCatalog != null ? obj.CharacterSetCatalog.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.DatetimePrecision.GetHashCode();
                    return hashCode;
                }
            }
        }

        static readonly IEqualityComparer<BasicType> _comparer = new Comparer();

        [EnableDumpExcept(false)]
        public readonly bool IsNullable;
        public readonly string DataType;
        [EnableDumpExcept(null)]
        public readonly int? CharacterMaximumLength;
        [EnableDumpExcept(null)]
        public readonly int? CharacterOctetLength;
        [EnableDumpExcept(null)]
        public readonly Byte? NumericPrecision;
        [EnableDumpExcept(null)]
        public readonly Int16? NumericPrecisionRadix;
        [EnableDumpExcept(null)]
        public readonly int? NumericScale;
        [EnableDumpExcept(null)]
        public readonly Int16? DatetimePrecision;
        [EnableDumpExcept(null)]
        public readonly string CharacterSetCatalog;
        [EnableDumpExcept(null)]
        public readonly string CharacterSetSchema;
        [EnableDumpExcept(null)]
        public readonly string CharacterSetName;
        [EnableDumpExcept(null)]
        public readonly string CollationCatalog;
        [EnableDumpExcept(null)]
        public readonly string CollationSchema;
        [EnableDumpExcept(null)]
        public readonly string CollationName;
        [EnableDumpExcept(null)]
        public readonly string DomainCatalog;
        [EnableDumpExcept(null)]
        public readonly string DomainSchema;
        [EnableDumpExcept(null)]
        public readonly string DomainName;

        BasicType(bool isNullable, string dataType, int? characterMaximumLength, int? characterOctetLength, byte? numericPrecision, short? numericPrecisionRadix, int? numericScale, short? datetimePrecision, string characterSetCatalog, string characterSetSchema, string characterSetName, string collationCatalog, string collationSchema, string collationName, string domainCatalog, string domainSchema, string domainName)
        {
            IsNullable = isNullable;
            DataType = dataType;
            CharacterMaximumLength = characterMaximumLength;
            CharacterOctetLength = characterOctetLength;
            NumericPrecision = numericPrecision;
            NumericPrecisionRadix = numericPrecisionRadix;
            NumericScale = numericScale;
            DatetimePrecision = datetimePrecision;
            CharacterSetCatalog = characterSetCatalog;
            CharacterSetSchema = characterSetSchema;
            CharacterSetName = characterSetName;
            CollationCatalog = collationCatalog;
            CollationSchema = collationSchema;
            CollationName = collationName;
            DomainCatalog = domainCatalog;
            DomainSchema = domainSchema;
            DomainName = domainName;
        }

        protected override string GetName()
        {
            var result = DataType;

            switch(DataType)
            {
                case "int":
                case "timestamp":
                    break;
                case "datetime":
                    Tracer.Assert(DatetimePrecision != null);
                    result += DatetimePrecision.Value;
                    break;
                default:
                    NotImplementedMethod();
                    break;
            }

            if(IsNullable)
                result += "?";
            return result;
        }
    }
}