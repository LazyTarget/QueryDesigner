using System.Collections.Generic;

namespace QueryDesigner.Core
{
    public class DataRow
    {
        public DataRow()
        {
            Cells = new List<DataCell>();
        }

        public List<DataCell> Cells { get; set; }


        public static IEnumerable<DataRow> FromDataTable(System.Data.DataTable dataTable)
        {
            if (dataTable != null)
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    var r = FromDataRow(row);
                    yield return r;
                }
        }

        public static DataRow FromDataRow(System.Data.DataRow dataRow)
        {
            if (dataRow == null)
                return null;

            var row = new DataRow();
            for (var i = 0; i < dataRow.ItemArray.Length; i++)
            {
                var c = dataRow.Table.Columns[i];
                var column = DataColumn.FromDataColumn(c);

                var cell = new DataCell
                {
                    Column = column,
                    Value = dataRow.ItemArray[i]
                };
                row.Cells.Add(cell);
            }
            return row;
        }
    }
}