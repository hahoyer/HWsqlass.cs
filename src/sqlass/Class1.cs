﻿#region Copyright (C) 2013

//     Project sqlass
//     Copyright (C) 2011 - 2013 Harald Hoyer
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
using System.Diagnostics;
using System.Linq;
using hw.UnitTest;
using sqlass.Tables;

namespace sqlass
{
    [TestFixture]
    public class Class1
    {
        public static string ConnectionString;
        [Test]
        public void Test()
        {
            try
            {
                var connectionString = new DbConnectionStringBuilder();
                connectionString["Data Source"] = "ANNE\\OJB_NET";
                connectionString["Initial Catalog"] = "unittest";
                connectionString["Integrated Security"] = "SSPI";
                connectionString["MultipleActiveResultSets"] = true;
                var connection = new SqlConnection(connectionString.ConnectionString);

                var context = new Context {Connection = connection, IsSqlCeConnectionBug = true};
                context.UpdateDatabase();

                var address = new Address {Id = 1, Text = "5th Ave, City23"};
                var customer = new Customer {Id = 1, Name = "Cust co ldt.", Address = address};
                context.Container.Address.Add(address);
                context.Container.Customer.Add(customer);
                //context.SaveChanges();
                var customers = context.Container.Customer.Where(c => c.Id == 1).Where(c => c.Id == 1);
                var customersFound = customers.ToArray();
                var customerFound = customers.Single();
            }
            catch(Exception)
            {
                Debugger.Break();
                throw;
            }
        }
    }
}