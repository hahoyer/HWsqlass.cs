// 
//     Project sqlass
//     Copyright (C) 2011 - 2011 Harald Hoyer
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

#region Generated Code

using System;
using System.Collections.Generic;
using System.Linq;
using HWClassLibrary.Debug;
using HWClassLibrary.sqlass;
using HWClassLibrary.sqlass.MetaData;

namespace sqlass.Tables
{
    public class Address
        : ISQLSupportProvider
          , ISQLKeyProvider<int>
    {
        public int Id;
        public string Text;

        ISQLSupport ISQLSupportProvider.SQLSupport { get { return new SQLSupport.Address(this); } }
        int ISQLKeyProvider<int>.SQLKey { get { return Id; } }
    }
}

namespace sqlass.SQLSupport
{
    public class Address : ISQLSupport
    {
        readonly Tables.Address _target;
        public Address(Tables.Address target) { _target = target; }

        string ISQLSupport.Insert
        {
            get
            {
                var result = "insert into Address values(";
                result += _target.Id.SQLFormat();
                result += ", ";
                result += _target.Text.SQLFormat();
                result += ")";

                return result;
            }
        }

        public static readonly Table MetaDataSupport = new Table
            (
            TableName.Find("", "", "Address"),
            () => new[]
                  {
                      new Column
                      {
                          Name = "Id",
                          Type = typeof(int),
                          IsKey = true,
                          IsNullable = false,
                      },
                      new Column
                      {
                          Name = "Text",
                          Type = typeof(string),
                          IsKey = false,
                          IsNullable = false,
                      },
                  },
            (record, context) => new Tables.Address
                                 {
                                     Id = ((int) record["Id"]),
                                     Text = ((string) record["Text"]),
                                 }
            );
    }
}

#endregion Generated Code