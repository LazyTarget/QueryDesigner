using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using QueryDesigner.Core;

namespace QueryDesigner.Web.Controllers
{
    public class BaseController : Controller
    {
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Error
        };


        private readonly Lazy<IDataSource<SqlQuery>> _dataSource = new Lazy<IDataSource<SqlQuery>>(() =>
        {
            var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            IDataSource<SqlQuery> dataSource = new SqlDatabase(connString.ConnectionString);
            return dataSource;
        });



        protected IDataSource<SqlQuery> DataSource
        {
            get { return _dataSource.Value; }
        }



        protected ContentResult JsonResult(object value)
        {
            var json = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
            var result = Content(json, "application/json", Encoding.UTF8);
            return result;
        }



        protected HttpStatusCodeResult Error(Exception exception)
        {
            var res = new HttpStatusCodeResult(500, exception.Message);
            return res;
        }

    }
}