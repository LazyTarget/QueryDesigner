using System;

namespace QueryDesigner.Core
{
    public interface IDataSource : IDataSource<IQuery>
    {
        //IQueryResult Execute(IQuery query);
    }

    public interface IDataSource<T> : IDisposable
        where T : IQuery
    {
        IQueryResult Execute(T query);
    }
}
