using System.Collections.Generic;

namespace QueryDesigner.Core
{
    public interface IQueryResult
    {
        IQuery Query { get; }
        
        IEnumerable<DataTable> Data { get; }
    }
}
