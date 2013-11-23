using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using hw.UnitTest;

namespace Taabus
{
    [TestFixture]
    public sealed class Test : DumpableObject
    {
        [Test]
        public void FullTextSearch()
        {
            try
            {
                var server = new Server { DataSource = "ANNE\\OJB_NET" };
                var dataBase = server
                    .DataBases
                    .Single(db => db.Name == "cwg_adsalesng_devtest");

                var query = dataBase
                    .Types
                    .SelectMany(item => item.FindAllText("Contract").Select(record => new TableRecord {Name = item.Name, Record = record}));
                var result = query.ToArray();
                Tracer.FlaggedLine(result.Length + " records found.");
                Tracer.Assert(result.Any());
                Tracer.Line(result[0].Dump());
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed("");
                throw;
            }
        }

        [Test]
        public void UserInterface()
        {
            try
            {
                var server = new Server { DataSource = "ANNE\\OJB_NET" };
                var dataBases = server.DataBases;
                var form = new TreeForm {Target = dataBases};
                Application.Run(form);
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed(exception.Message);
                throw;
            }
        }

        [Test]
        public void ForceFeedbackUserInterface()
        {
            try
            {
                var controller = new TaabusController();
                controller.Run();
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed(exception.GetType().PrettyName() + " " + exception.Message);
                throw;
            }
        }

        [Test]
        public void Base()
        {
            try
            {
                var server = new Server { DataSource = "ANNE\\OJB_NET" };
                var dataBases = server.DataBases;
                var dataBase = dataBases.Single(db => db.Name == "cwg_adsalesng_devtest");
                var types = dataBase.Types;
                var typetypeOrgOrgRole = types.Where(i => i.Name.Contains("OrgOrgRole"));
                var type = typetypeOrgOrgRole.First();
                var refs = type.References;
                var refsTarget = refs.Select(r => r.TargetType).ToArray();
                var members = type.Members;
                Tracer.FlaggedLine(members.Dump());
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed(exception.Message);
                throw;
            }
        }

        [Test]
        public void MetaData()
        {
            var relations = new[]
            {
                new[]
                {
                    "all_columns",
                    "Object all_objects object_id",
                    "Type types user_type_id"
                },
                new[]
                {
                    "all_objects",
                    "Schema schemas schema_id",
                    "Parent all_objects parent_object_id>object_id"
                },
                new[]
                {
                    "key_constraints",
                    "Object all_objects object_id",
                    "Parent all_objects parent_object_id>object_id",
                    "Schema schemas schema_id",
                    "Index indexes parent_object_id>object_id unique_index_id>index_id"
                },
                new[]
                {
                    "foreign_keys",
                    "Object all_objects object_id",
                    "Parent all_objects parent_object_id>object_id",
                    "Reference all_objects referenced_object_id>object_id",
                    "Schema schemas schema_id",
                    "Index indexes referenced_object_id>object_id key_index_id>index_id"
                },
                new[]
                {
                    "foreign_key_columns",
                    "Constraint foreign_keys constraint_object_id>object_id",
                    "ConstraintColumn all_columns Constraint.parent_object_id>object_id constraint_column_id>column_id",
                    "Parent all_objects parent_object_id>object_id",
                    "ParentColumn all_columns parent_object_id>object_id parent_column_id>column_id",
                    "Reference all_objects referenced_object_id>object_id",
                    "ReferenceColumn all_columns referenced_object_id>object_id referenced_column_id>column_id",
                },
                new[]
                {
                    "indexes",
                    "Object all_objects object_id",
                },
                new[]
                {
                    "index_columns",
                    "Object all_objects object_id",
                    "Index indexes object_id index_id",
                    "Column all_columns object_id column_id",
                },
                new[] {"schemas"},
                new[] {"types"}
            };

            var server = new Server { DataSource = "ANNE\\OJB_NET" };
            var metaDatas = server
                .DataBases
                .Select
                (
                    dataBase => new MetaDataGenerator
                        (
                        className: "SQLSysViews",
                        schema: "sys",
                        dataBase: dataBase,
                        relations: relations
                        )
                        .TransformText()
                )
                .ToArray();
            Tracer.Assert(metaDatas.Distinct().Count() == 1);
            Tracer.FlaggedLine("\n" + metaDatas.First());
        }

        const string ColumnInfoStatement = @"
SELECT 
	Name = s.name + '.' + o.name + '.' + c.name, 
	SchemeName = s.name + '', 
	ObjectName = o.name + '', 
	ColumnName = c.name + '', 
	ObjectType = o.type , 
	ColumnType = t.name +
	case
	when t.name = 'decimal' then '(' + convert(varchar, c.precision) + ',' + convert(varchar, c.scale) + ')'
	when t.name = 'char' then '(' + convert(varchar, c.max_length) + ')'
	when t.name = 'nvarchar' and c.max_length = -1 then '(max)'
	when t.name = 'nvarchar' then '(' + convert(varchar, c.max_length/2) + ')'
	when t.name = 'nchar' then '(' + convert(varchar, c.max_length/2) + ')'
	else ''
	end, 
	IsNull = c.is_nullable
FROM sys.columns AS c  
JOIN sys.objects AS o ON o.object_id = c.object_id 
JOIN sys.schemas AS s ON o.schema_id = s.schema_id 
JOIN sys.types AS t ON c.user_type_id = t.user_type_id
";
        [Test]
        public void KeySearch()
        {
            try
            {
                var server = new Server { DataSource = "ANNE\\OJB_NET" };
                var dataBase = server
                    .DataBases
                    .Single(db => db.Name == "cwg_adsalesng_devtest");

                var x = server.Select(ColumnInfoStatement, ColumnInfo.Create);
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed("");
                throw;
            }
        }

        sealed class ColumnInfo
        {
            public string Name;
            public string SchemeName;
            public string ObjectName;
            public string ColumnName;
            public string ObjectType;
            public string ColumnType;
            public bool IsNull;

            internal static ColumnInfo Create(DbDataRecord arg)
            {
                return new ColumnInfo()
                {
                    Name = arg.GetString(0),
                    SchemeName = arg.GetString(1),
                    ObjectName = arg.GetString(2),
                    ColumnName = arg.GetString(3),
                    ObjectType = arg.GetString(4),
                    ColumnType = arg.GetString(5),
                    IsNull = arg.GetBoolean(6)
                };
            }
        }
    }
}