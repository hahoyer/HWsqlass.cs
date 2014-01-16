using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Taabus.Data;
using Taabus.External;

namespace Taabus.UserInterface
{
    sealed class TableView : DataGridView, IDataItemContainer
    {
        readonly IControlledItem _item;

        public TableView(IControlledItem item)
        {
            _item = item;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            RowHeadersVisible = false;

            Columns.Add(CreateColumn("#"));
            Columns[0].Frozen = true;
            Columns.AddRange(item.Columns.Select(CreateColumn).ToArray());
            Rows.AddRange(item.Data.Select(CreateRow).ToArray());
        }

        External.DataItem IDataItemContainer.Externalize(IExternalIdProvider idProvider) { return new TableVieItem { Data = _item.Externalize(idProvider)}; }

        static DataGridViewTextBoxColumn CreateColumn(string name)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name
            };
        }

        static DataGridViewRow CreateRow(DataRecord record, int index)
        {
            var result = new DataGridViewRow();
            result.Cells.Add(CreateCell(index));
            result.Cells.AddRange(record.Values.Select(CreateCell).ToArray());
            return result;
        }

        static DataGridViewCell CreateCell(Data.DataItem arg) { return CreateCell(arg.Value); }

        static DataGridViewCell CreateCell(object value)
        {
            return new DataGridViewTextBoxCell
            {
                Value = value,
            };
        }

        static DataGridViewColumn CreateColumn(IDataColumn dataColumn)
        {
            var result = CreateColumn(dataColumn.Name);
            result.Name = dataColumn.Name;
            return result;
        }

        internal interface IItem : IColumnsAndDataProvider
        {
        }

    }

}