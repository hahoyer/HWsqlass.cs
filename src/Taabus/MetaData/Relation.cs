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
using hw.Helper;

namespace Taabus.MetaData
{
    sealed class Relation
    {
        public readonly string Type;
        public readonly string Name;
        public readonly KeyPart[] Key;
        public Relation(string s)
        {
            var x = s.Split(' ');
            Name = x[0];
            Type = x[1];
            Key = x.Skip(2).Select(KeyPart.Create).ToArray();
        }

        internal sealed class KeyPart
        {
            public readonly string This;
            public readonly string Foreign;
            KeyPart(string @this, string foreign)
            {
                This = @this;
                Foreign = foreign;
            }
            public static KeyPart Create(string s)
            {
                var y = s.Split('>');
                return new KeyPart(y[0], y[y.Length - 1]);
            }
        }

        public string GenerateKeyCompare(string foreign)
        {
            return Key
                .Select(k => k.This + "==" + foreign + "." + k.Foreign)
                .Stringify("&&");
        }
    }
}