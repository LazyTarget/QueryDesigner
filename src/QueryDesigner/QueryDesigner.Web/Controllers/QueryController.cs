using System;
using System.Linq;
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
                var temp = new User
                {
                    Username = "Peter",
                    Email = "peter@sourcetech.se"
                };
                var user = DataContext.Users.FirstOrDefault(x => x.Username == temp.Username);
                if (user == null)
                {
                    DataContext.Users.Add(temp);
                    DataContext.SaveChanges();
                }



                var query = new SqlQuery
                {
                    Sql = request.Query
                };
                IQueryResult result = GetUserDataSource().Execute(query);
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