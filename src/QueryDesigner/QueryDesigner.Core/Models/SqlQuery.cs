using System.Collections.Generic;
using System.Data.Common;

namespace QueryDesigner.Core
{
    public class SqlQuery : Query, IQuery
    {
        public SqlQuery()
        {
            Parameters = new List<DbParameter>();
        }

        public string Sql { get; set; }

        public List<DbParameter> Parameters { get; set; }


        public override string ToString()
        {
            return Sql;
        }
    }
}
