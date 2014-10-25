using System;
using System.Web.Mvc;
using QueryDesigner.Core;
using QueryDesigner.Web.Models;

namespace QueryDesigner.Web.Controllers
{
    public class QueryController : BaseController
    {
        public ActionResult Index()
        {
            return Compose();
        }

        public ActionResult Compose()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Execute(QueryRequestModel request)
        {
            try
            {
                var query = new SqlQuery
                {
                    Sql = request.Query
                };
                IQueryResult result = DataSource.Execute(query);
                var model = QueryResultViewModel.Generate(result);

                //var json = JsonResult(model);
                //return json;

                var view = PartialView("_ExecuteResult", model);
                return view;
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        
    }
}