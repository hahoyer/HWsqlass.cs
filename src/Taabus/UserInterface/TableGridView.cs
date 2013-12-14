using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Taabus.Data;

namespace Taabus.UserInterface
{
    sealed class TableGridView : DataGridView
    {
        readonly IControlledItem _item;
        public TableGridView(IControlledItem item)
        {
            _item = item;
            Columns.AddRange(_item.Columns.Select(CreateColumn).ToArray());
            Rows.AddRange(_item.Data.Take(10).Select(CreateRow).ToArray());
        }

        static DataGridViewRow CreateRow(DataRecord arg)
        {
            var result = new DataGridViewRow();
            result.Cells.AddRange(arg.Values.Select(CreateCell).ToArray());
            return result;
        }

        static DataGridViewCell CreateCell(DataItem arg)
        {
            var result = new DataGridViewTextBoxCell();
            result.Value = arg.Value;
            return result;
        }

        static DataGridViewColumn CreateColumn(IDataColumn dataColumn)
        {
            var result = new DataGridViewTextBoxColumn();
            result.Name = dataColumn.Name;
            return result;
        }
    }
}