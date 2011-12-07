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
    public class Customer
        : ISQLSupportProvider
          , ISQLKeyProvider<int>
    {
        public int Id;
        public string Name;
        public Address Address;
        public Address DeliveryAddress;

        ISQLSupport ISQLSupportProvider.SQLSupport { get { return new SQLSupport.Customer(this); } }
        int ISQLKeyProvider<int>.SQLKey { get { return Id; } }
    }
}

namespace sqlass.SQLSupport
{
    public class Customer : ISQLSupport
    {
        readonly Tables.Customer _target;
        public Customer(Tables.Customer target) { _target = target; }

        string ISQLSupport.Insert
        {
            get
            {
                var result = "insert into Customer values(";
                result += _target.Id.SQLFormat();
                result += ", ";
                result += _target.Name.SQLFormat();
                result += ", ";
                result += _target.Address.SQLFormat();
                result += ", ";
                result += _target.DeliveryAddress.SQLFormat();
                result += ")";

                return result;
            }
        }

        public static readonly Table MetaDataSupport = new Table
            (
            TableName.Find("", "", "Customer"),
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
                          Name = "Name",
                          Type = typeof(string),
                          IsKey = false,
                          IsNullable = false,
                      },
                      new Column
                      {
                          Name = "Address",
                          Type = typeof(int),
                          IsKey = false,
                          IsNullable = false,
                          ReferencedTable = Address.MetaDataSupport
                      },
                      new Column
                      {
                          Name = "DeliveryAddress",
                          Type = typeof(int),
                          IsKey = false,
                          IsNullable = true,
                          ReferencedTable = Address.MetaDataSupport
                      },
                  },
            (record, context) => new Tables.Customer
                                 {
                                     Id = ((int) record["Id"]),
                                     Name = ((string) record["Name"]),
                                     Address = ((Context) context).Container.Address.Find((int) record["Address"]),
                                     DeliveryAddress = ((Context) context).Container.Address.Find((int) record["DeliveryAddress"]),
                                 }
            );
    }
}

#endregion Generated Code