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
using System.Linq;

namespace Taabus.MetaData
{
    partial class SQLSysViews
    {
// ReSharper disable once InconsistentNaming
        partial class columnsClass
        {

            public string TypeName
            {
                get
                {
                    var result = Type.name;
                    switch(Type.name)
                    {
                        case "decimal":
                            return result + "(" + precision + ","+ scale+")";
                        case "char":
                            return result + "(" + max_length + ")";
                        case "nvarchar":
                            return result + "(" + (max_length == -1? "max":(max_length/2).ToString())+ ")";
                        case "nchar":
                            return result + "(" + max_length / 2 + ")";
                            
                        default:
                            return result;
                    }
                }
            }
        }
    }
}