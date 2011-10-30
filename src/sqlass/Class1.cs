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

using System;
using System.Collections.Generic;
using System.Linq;
using HWClassLibrary.Debug;
using HWClassLibrary.UnitTest;
using sqlass.Tables;

namespace sqlass
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test()
        {
            var context = new Context("test");

            var address = new Address {Id = 1, Text = "5th Ave, City23"};
            var customer = new Customer {Id = 1, Name = "Cust co ldt.", Address = new Reference<Address>(address)};
            context.Address.Add(address);
            context.Customer.Add(customer);
            context.SaveChanges();
            var customerFound = context.Customer.Where(c => c.Id == 1).Single();
        }
    }

    sealed partial class Context : SQLContext
    {
        public Context(string dbPath)
            : base(dbPath)
        {
        }
    }
}