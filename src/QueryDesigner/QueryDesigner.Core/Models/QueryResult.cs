using System.Collections.Generic;

namespace QueryDesigner.Core
{
    public class QueryResult : IQueryResult
    {
        internal QueryResult(IQuery query, IEnumerable<DataTable> data)
        {
            Query = query;
            Data = data;
        }


        public IQuery Query { get; private set; }

        public IEnumerable<DataTable> Data { get; private set; }
    }
}