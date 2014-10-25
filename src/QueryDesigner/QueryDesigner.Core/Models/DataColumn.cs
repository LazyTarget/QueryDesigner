namespace QueryDesigner.Core
{
    public class DataColumn
    {
        public string Name { get; set; }
        
        public string DataType { get; set; }

        public bool AutoIncrement { get; set; }

        public object DefaultValue { get; set; }

        public bool Unique { get; set; }

        public bool AllowNull { get; set; }

        public int MaxLength { get; set; }


        public static DataColumn FromDataColumn(System.Data.DataColumn dataColumn)
        {
            if (dataColumn == null)
                return null;

            var res = new DataColumn
            {
                Name = dataColumn.Caption ?? dataColumn.ColumnName,
                DataType = dataColumn.DataType.FullName,
                AllowNull = dataColumn.AllowDBNull,
                AutoIncrement = dataColumn.AutoIncrement,
                DefaultValue = dataColumn.DefaultValue,
                Unique = dataColumn.Unique,
                MaxLength = dataColumn.MaxLength
            };
            return res;
        }

    }
}
