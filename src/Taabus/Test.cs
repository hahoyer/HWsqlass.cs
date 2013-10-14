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
using System.Windows.Forms;
using hw.Debug;
using hw.Forms;
using hw.Helper;
using hw.UnitTest;

namespace Taabus
{
    [TestFixture]
    public sealed class Test
    {
        [Test]
        public void UI()
        {
            try
            {
                var server = new Server("ANNE\\OJB_NET");
                var dataBases = server.DataBases;
                var form = new TreeForm {Target = dataBases};
                Application.Run(form);
                Tracer.AssertionFailed("", () => "");
            }
            catch(Exception exception)
            {
                Tracer.AssertionFailed("");
                throw;
            }
        }

        [Test]
        public void Base()
        {
            try
            {
                var server = new Server("ANNE\\OJB_NET");
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
                Tracer.AssertionFailed("");
                throw;
            }
        }

        [Test]
        public void MetaData()
        {
            var server = new Server("ANNE\\OJB_NET");
            var metaDatas = server
                .DataBases
                .Select(dataBase => new MetaDataGenerator(dataBase).TransformText())
                .ToArray();
            Tracer.Assert(metaDatas.Distinct().Count() == 1);
            Tracer.FlaggedLine("\n" + metaDatas.First());
        }
    }

    partial class MetaDataGenerator
    {
        readonly string[] _objectNames =
        {
            "CHECK_CONSTRAINTS",
            "REFERENTIAL_CONSTRAINTS",
            "COLUMN_DOMAIN_USAGE",
            "ROUTINES",
            "COLUMN_PRIVILEGES",
            "ROUTINE_COLUMNS",
            "COLUMNS",
            "SCHEMATA",
            "CONSTRAINT_COLUMN_USAGE",
            "TABLE_CONSTRAINTS",
            "CONSTRAINT_TABLE_USAGE",
            "TABLE_PRIVILEGES",
            "DOMAIN_CONSTRAINTS",
            "TABLES",
            "DOMAINS",
            "VIEW_COLUMN_USAGE",
            "KEY_COLUMN_USAGE",
            "VIEW_TABLE_USAGE",
            "PARAMETERS",
            "VIEWS"
        };
        readonly DataBase _dataBase;
        internal MetaDataGenerator(DataBase dataBase) { _dataBase = dataBase; }
    }
}