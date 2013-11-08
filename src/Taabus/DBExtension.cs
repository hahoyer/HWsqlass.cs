using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using hw.Helper;

namespace Taabus
{
    static class DBExtension
    {
        public static SqlConnection ToConnection(this string serverName, string dataBase = null)
        {
            return ConnectionString(serverName, dataBase)
                .ToConnection();
        }

        internal static SqlConnection ToConnection(this SqlConnectionStringBuilder connectionString)
        {
            var connection = new SqlConnection(connectionString.ConnectionString);
            connection.Open();
            return connection;
        }

        static SqlConnectionStringBuilder ConnectionString(string serverName, string dataBase)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true,
                InitialCatalog = dataBase
            };
        }

        public static string NullableName(this Type type)
        {
            if(type.IsClass)
                return type.PrettyName();
            return type.PrettyName() + "?";
        }


        public static T Eval<T>(this Expression x)
        {
            return (T) Expression.Lambda(x)
                .Compile()
                .DynamicInvoke();
        }

        public static string SQLFormat(this string data) { return "'" + data.Replace("'", "''") + "'"; }

        /// <summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        /// <param name="items">The enumerable to search.</param>
        /// <param name="predicate">The expression to test the items against.</param>
        /// <returns>The index of the first matching item, or null if no items match.</returns>
        public static int? IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if(items == null)
                throw new ArgumentNullException("items");
            if(predicate == null)
                throw new ArgumentNullException("predicate");

            var result = 0;
            foreach(var item in items)
            {
                if(predicate(item))
                    return result;
                result++;
            }
            return null;
        }

        public static IEnumerable<T> Chain<T>(this T current, Func<T, T> getNext)
            where T : class
        {
            while(current != null)
            {
                yield return current;
                current = getNext(current);
            }
        }

        internal static int BeginMatch(string a, string b)
        {
            for(var i = 0;; i++)
                if(i >= a.Length || i >= b.Length || a[i] != b[i])
                    return i;
        }

        internal static bool In(this string a, params string[] b) { return b.Contains(a); }
    }
}