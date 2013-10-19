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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using hw.Debug;
using hw.Helper;

namespace Taabus
{
    abstract class QueryBase : DumpableObject, IQueryable<DataRecord>
    {
        protected readonly QueryProvider _provider;
        readonly ValueCache<string> _sqlCache;

        protected QueryBase(QueryProvider provider)
        {
            _provider = provider;
            _sqlCache = new ValueCache<string>(CreateSQL);
        }

        IEnumerator<DataRecord> IEnumerable<DataRecord>.GetEnumerator() { return GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        Expression IQueryable.Expression { get { return Expression.Constant(this); } }
        Type IQueryable.ElementType { get { return typeof(DataRecord); } }
        IQueryProvider IQueryable.Provider { get { return _provider; } }
        public ValueCache<string> SQLCache { get { return _sqlCache; } }

        IEnumerator<DataRecord> GetEnumerator() { return new Enumerator(this); }

        sealed class Enumerator : DumpableObject, IEnumerator<DataRecord>
        {
            readonly QueryBase _parent;
            DbDataReader _reader;
            IEnumerator _position;
            public Enumerator(QueryBase parent) { _parent = parent; }
            void IDisposable.Dispose() { DropReader(); }
            
            void DropReader()
            {
                if(_reader != null)
                {
                    _reader.Close();
                    _reader.Dispose();
                }
                _reader = null;
                _position = null;
            }

            bool IEnumerator.MoveNext()
            {
                if(_position == null)
                {
                    _reader = _parent.CreateReader();
                    _position = _reader.GetEnumerator();
                }
                return _position.MoveNext();
            }

            void IEnumerator.Reset() { DropReader(); }
            DataRecord IEnumerator<DataRecord>.Current { get { return Current; } }
            object IEnumerator.Current { get { return Current; } }

            DataRecord Current { get { return new DataRecord((DbDataRecord)_position.Current); } }
        }

        DbDataReader CreateReader() { return _provider.Server.ToDataReader(_sqlCache.Value); }

        internal abstract string CreateSQL();
    }

    [AttributeUsage(AttributeTargets.Method)]
    abstract class SQLConverter : Attribute
    {
        internal abstract string Convert(QueryProvider provider, Expression objectExpression, Expression[] args);
    }
}