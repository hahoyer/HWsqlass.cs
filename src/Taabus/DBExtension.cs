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
using System.Data.Common;
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
            var connectionString = new DbConnectionStringBuilder();
            connectionString["Data Source"] = serverName;
            connectionString["Integrated Security"] = "SSPI";
            if(dataBase != null)
                connectionString["Initial Catalog"] = dataBase;
            connectionString["MultipleActiveResultSets"] = true;
            var connection = new SqlConnection(connectionString.ConnectionString);
            connection.Open();
            return connection;
        }

        public static string NullableName(this Type type)
        {
            if(type.IsClass)
                return type.PrettyName();
            return type.PrettyName() + "?";
        }


        public static T Eval<T>(this Expression x)
        {
            return (T) Expression
                .Lambda(x)
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
    }
}