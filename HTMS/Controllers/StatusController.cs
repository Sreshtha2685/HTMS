using DataModel;
using HTMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Controllers
{
    public class StatusController : Controller
    {
        HttpClient client = new HttpClient();
        Status status = null;
        public List<Status> LstAllStatus = new List<Status>();
       
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();


        public StatusController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Status
        public ActionResult Status()
        {
            return View();
        }
        public IEnumerable<Status> GetAllStatus()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Status").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllStatus = res.Content.ReadAsAsync<List<Status>>().Result.ToList();
                    return LstAllStatus.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
      
        public ActionResult GetAllStatusList([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var skilldetails = GetAllStatus().OrderByDescending(a => a.id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private List<Status> GetStatusData()
        {
            try
            {
                status = new Status();
                var request = new RestRequest("api/Status", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Status>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BindAllStatus([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

               

                var data1 = GetAllStatus();

                var query = (from a in data1
                             

                             select new StatusModel
                             {
                                 id = a.id,
                                 Status1 = a.Status1,
                                 Description = a.Description,
                                
                             }).ToList();


                if (query.Count > 0)
                {

                    return Json(query.ToDataSourceResult(req), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult GetStatusById(int id)
        {
            try
            {
                status = new Status();
                var request = new RestRequest("api/Status/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Status>>(request);

                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);

                return Json(response.Data.ToList()[0]);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        [HttpPost]
        public ActionResult AddStatus(Status obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Status", obj).Result;
            if (response1.IsSuccessStatusCode)
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }


        [AcceptVerbs("Post")]
        public ActionResult EditingPopup_Update(Status status)
        {
            try
            {
                string result = "fail";
                var ss = GetAllStatus();
                if (ss != null)
                {
                    status.InsertedBy = 1;
                    status.InsertedOn = DateTime.Now;
                    status.IsActive = true;
                    status.IsDelete = false;
                    var res = new RestRequest("api/Status/", Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(status);
                    var response = _client.Execute<List<Status>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Status", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    status.InsertedBy = 1;
                    status.InsertedOn = DateTime.Now;
                    status.IsActive = true;
                    status.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/Status/", status).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Status", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteStatus(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Status/" + Convert.ToInt32(id[i])).Result;
                if (clientRequest.IsSuccessStatusCode)
                {

                    i++;
                }
                else
                {
                    return null;
                }
            }
            return Json("OK", JsonRequestBehavior.AllowGet);

        }

    }
}