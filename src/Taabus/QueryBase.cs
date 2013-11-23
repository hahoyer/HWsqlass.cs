using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using hw.Debug;
using hw.Helper;

namespace Taabus
{
    abstract class QueryBase : DumpableObject, IQueryable<DataRecord>
    {
        protected readonly QueryProvider Provider;
        readonly ValueCache<string> _sqlCache;

        protected QueryBase(QueryProvider provider)
        {
            Provider = provider;
            _sqlCache = new ValueCache<string>(CreateSQL);
        }

        IEnumerator<DataRecord> IEnumerable<DataRecord>.GetEnumerator() { return GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        Expression IQueryable.Expression { get { return Expression.Constant(this); } }
        Type IQueryable.ElementType { get { return typeof(DataRecord); } }
        IQueryProvider IQueryable.Provider { get { return Provider; } }
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

            DataRecord Current { get { return new DataRecord((DbDataRecord) _position.Current); } }
        }

        DbDataReader CreateReader() { return Provider.Server.ToDataReader(_sqlCache.Value); }

        internal abstract string CreateSQL();
    }

    [AttributeUsage(AttributeTargets.Method)]
    abstract class SQLConverter : Attribute
    {
        internal abstract string Convert(QueryProvider provider, Expression objectExpression, Expression[] args);
    }
}