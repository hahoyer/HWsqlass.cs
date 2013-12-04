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

using hw.Debug;
using Taabus.MetaData;

namespace Taabus       .Data
{
    sealed class Field : DumpableObject
    {
        [DisableDump]
        public DataBase DataBase;
        public string Name;
        public BasicType Type;
        [DisableDump]
        public CompountType Container;
    }

    class FieldValue
    {
        public Field Parent;
        public object Key;
        public object Value;
    }
}