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
using sqlass.Tables;

namespace sqlass
{
    public sealed class Container
    {
        public readonly Table<Customer, int> Customer;
        public readonly Table<Address, int> Address;
        public Container(Context context)
        {
            Customer = new Table<Customer, int>(context, SQLSupport.Customer.MetaDataSupport);
            Address = new Table<Address, int>(context, SQLSupport.Address.MetaDataSupport);
        }
    }

    public sealed class Context : HWClassLibrary.sqlass.Context
    {
        public readonly Container Container;
        public Context() { Container = new Container(this); }
        public void UpdateDatabase() { UpdateDatabase(Container); }
    }
}

#endregion Generated Code