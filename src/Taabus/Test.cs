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
using hw.Debug;
using hw.Helper;
using hw.UnitTest;

namespace Taabus
{
    [TestFixture]
    public sealed class Test
    {
        [Test]
        public void Base()
        {
            try
            {
                var server = new Server("ANNE\\OJB_NET");
                var dataBases = server.DataBases;
                var dataBase = dataBases.Single(db => db.Name == "cwg_adsalesng_devtest");
                var items = dataBase.Items;
                var item = items.First(i => i.Name.Contains("OrgOrgRole"));
                var members = item.Items; 
                Tracer.AssertionFailed("", members.Dump);
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