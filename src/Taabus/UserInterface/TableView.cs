using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using Taabus.Data;
using Taabus.External;

namespace Taabus.UserInterface
{
    sealed class TableView
        : MetroPanel
            , Internalizer.IItem
            , WorkspaceView.IControl
            , DragDropController.IProxySource
    {
        static int _nextId;

        readonly WorkspaceView _parent;
        readonly IItem _item;

        readonly DataGridView _grid;
        readonly MetroButton _header;

        public TableView(IItem item, WorkspaceView parent)
        {
            _item = item;
            _parent = parent;

            _header = new MetroButton
            {
                AutoSize = true,
                Text = _item.Title,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            Controls.Add(_header);

            _grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                Location = new Point(0, _header.Size.Height),
                Size = new Size(Size.Width, Size.Height - _header.Size.Height),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top
            };
            _grid.ColumnWidthChanged += OnColumnWidthChanged;
            Controls.Add(_grid);

        }

        ItemData Internalizer.IItem.Convert(Externalizer externalizer)
        {
            return new External.TableView
            {
                Data = _item.Convert(externalizer),
                ColumnConfig = ExternalColumnConfig
            };
        }

        Control DragDropController.ISource.Control { get { return this; } }
        DragDropController.IItem DragDropController.ISource.GetItemAt(Point point) { return this; }
        Control DragDropController.IProxySource.DragControl { get { return _header; } }
        Size DragDropController.ISource.GetDisplacementAt(Point point) { return new Size(point); }

        internal ColumnConfig[] ExternalColumnConfig
        {
            private get
            {
                var columns = _grid
                    .Columns
                    ._()
                    .OrderBy(c => c.DisplayIndex);
                var result = columns
                    .Select(Externalizer.Convert)
                    .ToArray();
                return result;
            }
            set
            {
                _grid.ColumnWidthChanged -= OnColumnWidthChanged;
                foreach(var column in _grid.Columns._().ToArray())
                    _grid.Columns.Remove(column);

                _grid.Columns.AddRange(value.Select(CreateColumn).ToArray());
                LoadRows();
                _grid.ColumnWidthChanged += OnColumnWidthChanged;
            }
        }

        void OnColumnWidthChanged(object sender, DataGridViewColumnEventArgs e) { _parent.Save(); }

        void LoadColumns()
        {
            _grid.Columns.Add(CreateColumn("#"));
            _grid.Columns[0].Frozen = true;
            _grid.Columns.AddRange(_item.Columns.Select(CreateColumn).ToArray());
        }

        void LoadRows() { _grid.Rows.AddRange(_item.Data.Select(CreateRow).ToArray()); }

        internal void LoadData()
        {
            LoadColumns();
            LoadRows();
        }

        static DataGridViewColumn CreateColumn(ColumnConfig config)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = config.Name,
                Width = config.Width
            };
        }

        static DataGridViewColumn CreateColumn(IDataColumn dataColumn)
        {
            var result = CreateColumn(dataColumn.Name);
            result.Name = dataColumn.Name;
            return result;
        }

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

        static DataGridViewCell CreateCell(DataItem arg) { return CreateCell(arg.Value); }

        static DataGridViewCell CreateCell(object value)
        {
            return new DataGridViewTextBoxCell
            {
                Value = value,
            };
        }

        internal interface IItem : IColumnsAndDataProvider, IExternalizeable<Id>
        {
            string Title { get; }
        }

        public void OnAdded() { _grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader); }
    }
}