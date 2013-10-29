#region Copyright (C) 2013

//     Project Taabus
//     Copyright (C) 2013 - 2013 Harald Hoyer
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;

namespace Taabus.MetaData
{
    partial class SQLSysViews
    {
// ReSharper disable once InconsistentNaming
        partial class all_columnsClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + ":" + column_id + "]" + name + "(" + TypeName + ")"; }

            internal string TypeName
            {
                get
                {
                    var result = Type.name;
                    switch(Type.name)
                    {
                        case "decimal":
                            return result + "(" + precision + "," + scale + ")";
                        case "char":
                            return result + "(" + max_length + ")";
                        case "nvarchar":
                            return result + "(" + (max_length == -1 ? "max" : (max_length / 2).ToString()) + ")";
                        case "nchar":
                            return result + "(" + max_length / 2 + ")";

                        default:
                            return result;
                    }
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        partial class all_objectsClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "]" + name; }

            public all_columnsClass[] Columns
            {
                get
                {
                    return _parent
                        .all_columns
                        .Where(c => c.Object == this)
                        .ToArray();
                }
            }

            internal ObjectType Type { get { return ObjectType.All.SingleOrDefault(c => c.Name == type); } }
            [DisableDump]
            internal key_constraintsClass AsKeyConstraint { get { return _parent.key_constraints.Single(k => k.object_id == object_id); } }
            [DisableDump]
            internal foreign_keysClass AsForeignKey { get { return _parent.foreign_keys.Single(k => k.object_id == object_id); } }
            [DisableDump]
            internal indexesClass[] Indexes { get { return _parent.indexes.Where(k => k.object_id == object_id).ToArray(); } }
            [DisableDump]
            internal key_constraintsClass[] KeyConstraints { get { return _parent.key_constraints.Where(k => k.Parent == this).ToArray(); } }
            [DisableDump]
            internal foreign_keysClass[] ForeignKeys { get { return _parent.foreign_keys.Where(k => k.Parent == this).ToArray(); } }
        }

        // ReSharper disable once InconsistentNaming
        partial class schemasClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + schema_id + "]" + name; }
        }

        // ReSharper disable once InconsistentNaming
        partial class indexesClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "." + index_id + "]" + name; }

            internal index_columnsClass[] Columns
            {
                get
                {
                    return _parent
                        .index_columns
                        .Where(c => c.index_id == index_id && c.Object == Object)
                        .ToArray();
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        sealed partial class key_constraintsClass:DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "]" + name; }

        }
        // ReSharper disable once InconsistentNaming
        partial class foreign_keysClass : DumpableObject
        {
            protected override string GetNodeDump() { return "[" + object_id + "]" + name; }

            internal foreign_key_columnsClass[] Columns
            {
                get
                {
                    return _parent
                        .foreign_key_columns
                        .Where(c => c.Constraint == this)
                        .ToArray();
                }
            }
        }
    }
}