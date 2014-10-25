using System.Collections.Generic;
using System.Dynamic;
using QueryDesigner.Core;

namespace QueryDesigner.Web.Models
{
    public class QueryResultViewModel
    {
        public QueryResultViewModel()
        {
            //Data = new List<dynamic>();
            Data = new List<DataTable>();
        }


        public string Query { get; set; }

        public IEnumerable<DataTable> Data { get; set; }



        public static QueryResultViewModel Generate(IQueryResult result)
        {
            var model = new QueryResultViewModel
            {
                Query = result.Query.ToString()
            };


            model.Data = result.Data;
            
            /*
            if (result.Data != null)
            {
                //foreach (DataTable table in result.Data)
                //{
                //    dynamic tbl = new ExpandoObject();
                //    tbl.Rows = new List<dynamic>();

                //    foreach (DataRow row in table.Rows)
                //    {

                        
                //    }
                //    tbl.Rows = new List<dynamic>
                //    {
                //        new ExpandoObject
                //        {
                //            C
                //        },
                //    };

                //    model.Data.Add(tbl);
                //}
            }
            */
            return model;
        }

    }
}