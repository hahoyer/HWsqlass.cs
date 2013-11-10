using System;
using System.Collections.Generic;
using System.Linq;
using Taabus.MetaData;

namespace Taabus
{
    partial class MetaDataGenerator
    {
        readonly string _className;
        readonly string _schema;
        readonly string[] _objectNames;
        readonly DataBase _dataBase;
        readonly Dictionary<string, Relation[]> _relations;
        internal MetaDataGenerator(string schema, DataBase dataBase, string className, string[][] relations)
        {
            _dataBase = dataBase;
            _objectNames = relations.Select(r => r[0]).ToArray();
            _className = className;
            _relations =
                relations
                    .Select
                    (r =>
                        new
                        {
                            key = r[0],
                            value = r.Skip(1).Select(rr => new Relation(rr)).ToArray()
                        }
                    )
                    .ToDictionary(o => o.key, o => o.value);
            _schema = schema;
        }
    }
}