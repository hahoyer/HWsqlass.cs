using System;
using System.Collections.Generic;
using System.Linq;
using hw.Debug;

namespace Taabus.MetaData
{
    sealed class Information : DumpableObject
    {
        readonly SQLInformation _sqlInformation;

        public Information(SQLInformation.IDataProvider provider)
        {
            _sqlInformation = new SQLInformation(provider);
            Tracer.Assert(IsSimpleScheme);
        }

        /// <summary>
        ///     Check if scheme is not required to identify an object, i. e. there are not objects with the same name under
        ///     different schemes
        /// </summary>
        bool IsSimpleScheme
        {
            get
            {
                if(_sqlInformation.SCHEMATA.Count() == 1)
                    return true;
                var groupBy = Types.GroupBy(t => t.Name).FirstOrDefault(g => g.Count() > 1);
                if(groupBy == null)
                    return true;
                var d = groupBy.ToArray();
                return false;
            }
        }

        internal CompountType[] Types
        {
            get
            {
                return _sqlInformation
                    .TABLES
                    .Select(t => new CompountType(t.TABLE_NAME, t.TABLE_SCHEMA, GetMembers(t)))
                    .ToArray();
            }
        }

        Member[] GetMembers(SQLInformation.TABLESClass table)
        {
            return _sqlInformation
                .COLUMNS
                .Where(column => column.TABLE_NAME == table.TABLE_NAME && column.TABLE_SCHEMA == table.TABLE_SCHEMA)
                .Select(CreateMember)
                .ToArray();
        }

        static Member CreateMember(SQLInformation.COLUMNSClass column) { return new Member(column.COLUMN_NAME, BasicType.GetInstance(column)); }
    }
}