//     Compiler for programming language "Reni"
//     Copyright (C) 2011 Harald Hoyer
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HWClassLibrary.Debug;

namespace HWClassLibrary.Helper
{
    public static class ListExtender
    {
        public static bool AddDistinct<T>(this IList<T> a, IEnumerable<T> b, Func<T, T, bool> isEqual) { return InternalAddDistinct(a, b, isEqual); }
        public static bool AddDistinct<T>(this IList<T> a, IEnumerable<T> b, Func<T, T, T> combine) where T : class { return InternalAddDistinct(a, b, combine); }

        static bool InternalAddDistinct<T>(ICollection<T> a, IEnumerable<T> b, Func<T, T, bool> isEqual)
        {
            var result = false;
            foreach(var bi in b)
                if(AddDistinct(a, bi, isEqual))
                    result = true;
            return result;
        }

        static bool InternalAddDistinct<T>(IList<T> a, IEnumerable<T> b, Func<T, T, T> combine) where T : class
        {
            var result = false;
            foreach(var bi in b)
                if(AddDistinct(a, bi, combine))
                    result = true;
            return result;
        }

        static bool AddDistinct<T>(ICollection<T> a, T bi, Func<T, T, bool> isEqual)
        {
            foreach(var ai in a)
                if(isEqual(ai, bi))
                    return false;
            a.Add(bi);
            return true;
        }

        static bool AddDistinct<T>(IList<T> a, T bi, Func<T, T, T> combine) where T : class
        {
            for(var i = 0; i < a.Count; i++)
            {
                var ab = combine(a[i], bi);
                if(ab != null)
                {
                    a[i] = ab;
                    return false;
                }
            }
            a.Add(bi);
            return true;
        }

        public static string Dump<T>(this IEnumerable<T> x) { return x.Aggregate(x.ToArray().Length.ToString(), (a, xx) => a + " " + xx.ToString()); }

        public static string DumpLines<T>(this IEnumerable<T> x)
            where T : Dumpable
        {
            var i = 0;
            return x.Aggregate("", (a, xx) => a + "[" + i++ + "] " + xx.Dump() + "\n");
        }

        public static string Format<T>(this IEnumerable<T> x, string separator)
        {
            var result = "";
            foreach(var element in x)
            {
                if(result != "")
                    result += separator;
                result += element.ToString();
            }
            return result;
        }

        public static TimeSpan Sum<T>(this IEnumerable<T> x, Func<T, TimeSpan> selector)
        {
            var result = new TimeSpan();
            foreach(var element in x)
                result += selector(element);
            return result;
        }

        /// <summary>
        ///     Checks if object starts with given object.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "x">The x.</param>
        /// <param name = "y">The y.</param>
        /// <returns></returns>
        public static bool StartsWith<T>(this IList<T> x, IList<T> y)
        {
            if(x.Count < y.Count)
                return false;
            for(var i = 0; i < y.Count; i++)
                if(!Equals(x[i], y[i]))
                    return false;
            return true;
        }

        /// <summary>
        ///     Checks if object starts with given object and is longer.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "x">The x.</param>
        /// <param name = "y">The y.</param>
        /// <returns></returns>
        public static bool StartsWithAndNotEqual<T>(this IList<T> x, IList<T> y)
        {
            if(x.Count == y.Count)
                return false;
            return x.StartsWith(y);
        }

        public static T OnlyOne<T>(this IEnumerable<T> x)
        {
            var xx = x.ToArray();
            Tracer.Assert(xx.Length == 1);
            return xx[0];
        }

        public static TResult CheckedApply<T, TResult>(this T target, Func<T, TResult> function)
            where T : class
            where TResult : class { return target == default(T) ? default(TResult) : function(target); }

        public static IEnumerable<T> Array<T>(this int count, Func<int, T> getValue) { return new ArrayQuery<T>(count, getValue); }

        sealed class ArrayQuery<T> : IEnumerable<T>
        {
            readonly int _count;
            readonly Func<int, T> _getValue;
            public ArrayQuery(int count, Func<int, T> getValue)
            {
                _count = count;
                _getValue = getValue;
            }

            IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable<T>) this).GetEnumerator(); }
            IEnumerator<T> IEnumerable<T>.GetEnumerator() { return new Enumerator(this); }

            sealed class Enumerator : IEnumerator<T>
            {
                readonly ArrayQuery<T> _arrayQuery;
                int? _index;
                public Enumerator(ArrayQuery<T> arrayQuery) { _arrayQuery = arrayQuery; }

                void IDisposable.Dispose() { }
                bool IEnumerator.MoveNext()
                {
                    if(_index == null)
                        _index = 0;
                    else
                        _index++;

                    return _index < _arrayQuery._count;
                }

                void IEnumerator.Reset() { _index = null; }
// ReSharper disable PossibleInvalidOperationException
                T IEnumerator<T>.Current { get { return _arrayQuery._getValue(_index.Value); } } // should throw an exception if _index is null
// ReSharper restore PossibleInvalidOperationException
                object IEnumerator.Current { get { return ((IEnumerator<T>) this).Current; } }
            }
        }
    }
}