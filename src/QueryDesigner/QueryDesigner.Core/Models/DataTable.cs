using System.Collections.Generic;
using System.Linq;

namespace QueryDesigner.Core
{
    public class DataTable
    {
        public DataTable()
        {
            Rows = new List<DataRow>();
        }


        public string Name { get; set; }

        public IEnumerable<DataColumn> Columns { get; set; }

        public IEnumerable<DataRow> Rows { get; set; }



        public static IEnumerable<DataTable> FromDataSet(System.Data.DataSet dataSet)
        {
            if (dataSet != null)
                foreach (System.Data.DataTable dt in dataSet.Tables)
                {
                    var t = FromDataTable(dt);
                    yield return t;
                }
        }


        public static DataTable FromDataTable(System.Data.DataTable dataTable)
        {
            if (dataTable == null)
                return null;
            
            var table = new DataTable
            {
                Name = dataTable.TableName
            };

            table.Columns = dataTable.Columns.Cast<System.Data.DataColumn>().Select(DataColumn.FromDataColumn);
            table.Rows = DataRow.FromDataTable(dataTable);

            return table;
        }

    }
}