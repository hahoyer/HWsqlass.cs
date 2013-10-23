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

        internal static BasicType GetInstance(SQLSysViews.columnsClass column)
        {
            var probe = new BasicType(
                isNullable: column.is_nullable == true,
                dataType: column.Type.name,
                characterOctetLength: column.max_length,
                numericPrecision: column.precision,
                numericScale: column.scale
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
                    && x.NumericPrecision == y.NumericPrecision
                    && x.NumericScale == y.NumericScale
                    ;
            }
            public int GetHashCode(BasicType obj)
            {
                unchecked
                {
                    var hashCode = obj.IsNullable.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.DataType != null ? obj.DataType.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.CharacterOctetLength.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.NumericPrecision.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.NumericScale.GetHashCode();
                    return hashCode;
                }
            }
        }

        static readonly IEqualityComparer<BasicType> _comparer = new Comparer();

        [DisableDump]
        public readonly bool IsNullable;
        [DisableDump]
        public readonly string DataType;
        [DisableDump]
        public readonly int? CharacterOctetLength;
        [DisableDump]
        public readonly Byte? NumericPrecision;
        [DisableDump]
        public readonly int? NumericScale;

        BasicType(bool isNullable, string dataType, int? characterOctetLength, byte? numericPrecision, int? numericScale)
        {
            IsNullable = isNullable;
            DataType = dataType;
            CharacterOctetLength = characterOctetLength;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
        }

        [DisableDump]
        internal bool IsText
        {
            get
            {
                switch(DataType)
                {
                    case "char":
                    case "nchar":
                    case "varchar":
                    case "nvarchar":
                        return true;
                    default:
                        return false;
                }
            }
        }

        [DisableDump]
        internal bool IsInteger
        {
            get
            {
                switch(DataType)
                {
                    case "int":
                    case "smallint":
                    case "tinyint":
                        return true;
                    default:
                        return false;
                }
            }
        }

        protected override string GetName()
        {
            var result = DataType;

            switch(DataType)
            {
                case "int":
                case "smallint":
                case "tinyint":
                case "timestamp":
                case "bit":
                case "uniqueidentifier":
                    break;
                case "datetime":
                    Tracer.Assert(NumericScale != null);
                    result += "("+NumericScale.Value+")";
                    break;
                case "char":
                case "varchar":
                    Tracer.Assert(CharacterOctetLength != null);
                    result += "(" + CharacterOctetLength.Value + ")";
                    break;
                case "nchar":
                case "nvarchar":
                    Tracer.Assert(CharacterOctetLength != null);
                    result += "(" + CharacterOctetLength.Value / 2 + ")";
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