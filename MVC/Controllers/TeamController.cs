using Newtonsoft.Json;
using MVC.INTERFACE;
using MVC.MODEL;
using MVC.WEB.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class TeamController : Controller
    {
        private ITeamService teamService;
        Response response;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
            response = new Response();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            if (Session["NOTEBOOK_USER_ID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Get(string id)
        {
            if (Session["NOTEBOOK_USER_ID"] == null)
            {
                response.status = HttpStatusCode.Unauthorized;
                response.statusText = "Login First";
                var result1 = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            double totalitems;
            try
            {

                TEAM team = teamService.Get(id);
                response.items = team;
                response.status = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                response.status = HttpStatusCode.InternalServerError;
            }
            var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPaginatedData(string pageNumber, string pageSize, string searchText)
        {
            if (Session["NOTEBOOK_USER_ID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            double totalitems;
            try
            {
                if (pageNumber == "" || pageSize == "")
                {
                    pageNumber = "1";
                    pageSize = "10";
                }
                if (searchText == "")
                {
                    searchText = "ALL";
                }
                int pageTo = Convert.ToInt32(pageNumber) * Convert.ToInt32(pageSize);
                int pageFrom = pageTo - Convert.ToInt32(pageSize);

                IList<TEAM> data =teamService.GetList(pageFrom.ToString(), pageTo.ToString(), searchText, Session["NOTEBOOK_EMPLOYEE_ID"].ToString());

                response.items = data;
                response.status = HttpStatusCode.OK;
                response.statusText = "Success";
            }
            catch (Exception ex)
            {
                Exception realerror = ex;
                while (realerror.InnerException != null)
                    realerror = realerror.InnerException;
                response.status = HttpStatusCode.InternalServerError;
                response.statusText = realerror.ToString();
            }


            var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


       
        [HttpPost]
        public ActionResult SaveTeam(List<TEAM_EMPLOYEE> EMPLOYEE_LIST, string TEAM_NAME)
        {
            try
            {
                
                if (Session["NOTEBOOK_USER_ID"] == null)
                {
                    response.status = HttpStatusCode.InternalServerError;
                    response.statusText = "Please login first...";
                    var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (TEAM_NAME == "")
                {
                    response.status = HttpStatusCode.InternalServerError;
                    response.statusText = "Please give team title...";
                    var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                
                
                TEAM team = new TEAM();
                team.TEAM_ID = Guid.NewGuid().ToString();
                team.BRANCH_ID ="0001";
                team.TEAM_NAME = TEAM_NAME;
                team.ENTRY_DATE = DateTime.Now;
                team.ENTRY_BY = Session["NOTEBOOK_EMPLOYEE_ID"].ToString();
                team.EMPLOYEE_LIST = EMPLOYEE_LIST;
                bool resultResponse = teamService.Save(team);
                if (resultResponse)
                {
                    response.status = HttpStatusCode.OK;
                    response.statusText = "YOUR TEAM HAS BEEN CREATED SUCCESSFULLY. " + System.Environment.NewLine + " \n  ";
                }

            }
            catch (Exception ex)
            {
                Exception realerror = ex;
                while (realerror.InnerException != null)
                    realerror = realerror.InnerException;
                response.status = HttpStatusCode.InternalServerError;
                response.statusText = realerror.ToString();
            }
            var responseResult = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateTeam(List<TEAM_EMPLOYEE> EMPLOYEE_LIST, string TEAM_NAME,string TEAM_ID)
        {
            try
            {
                if (TEAM_ID == null)
                {
                    response.status = HttpStatusCode.InternalServerError;
                    response.statusText = "Please give team id...";
                    var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (Session["NOTEBOOK_USER_ID"] == null)
                {
                    response.status = HttpStatusCode.InternalServerError;
                    response.statusText = "Please login first...";
                    var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (TEAM_NAME == "")
                {
                    response.status = HttpStatusCode.InternalServerError;
                    response.statusText = "Please give team title...";
                    var result = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }


                TEAM team = new TEAM();
                team.TEAM_ID = TEAM_ID;
                team.TEAM_NAME = TEAM_NAME;
                team.ENTRY_DATE = DateTime.Now;
                team.ENTRY_BY = Session["NOTEBOOK_EMPLOYEE_ID"].ToString();
                team.EMPLOYEE_LIST = EMPLOYEE_LIST;
                bool resultResponse = teamService.Update(team);
                if (resultResponse)
                {
                    response.status = HttpStatusCode.OK;
                    response.statusText = "YOUR TEAM HAS BEEN UPDATED SUCCESSFULLY. " + System.Environment.NewLine + " \n  ";
                }

            }
            catch (Exception ex)
            {
                Exception realerror = ex;
                while (realerror.InnerException != null)
                    realerror = realerror.InnerException;
                response.status = HttpStatusCode.InternalServerError;
                response.statusText = realerror.ToString();
            }
            var responseResult = JsonConvert.SerializeObject(response, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });

            return Json(responseResult, JsonRequestBehavior.AllowGet);
        }


    }
}
