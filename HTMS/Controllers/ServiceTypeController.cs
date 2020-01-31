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
    public class ServiceTypeController : Controller
    {
        HttpClient client = new HttpClient();
        ServiceType st = null;
        public List<ServiceType> LstAllServiceType = new List<ServiceType>();
       
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public ServiceTypeController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: State
        public ActionResult ServiceType()
        {
            return View();
        }

        public IEnumerable<ServiceType> GetAllServiceType()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/ServiceType").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllServiceType = res.Content.ReadAsAsync<List<ServiceType>>().Result.ToList();
                    return LstAllServiceType.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
       
        public ActionResult GetAllServiceTypeList([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var skilldetails = GetAllServiceType().OrderByDescending(a => a.id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private List<ServiceType> GetServiceTypeData()
        {
            try
            {
                st = new ServiceType();
                var request = new RestRequest("api/ServiceType", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<ServiceType>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BindAllServiceType([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllServiceType();
              
                var query = (from a in data
                             select new ServiceTypeModel
                             {
                                 id = a.id,
                                 ServiceTypeName = a.ServiceTypeName,
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
        public ActionResult GetServiceTypeById(int id)
        {
            try
            {
                st = new ServiceType();
                var request = new RestRequest("api/ServiceType/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<ServiceType>>(request);

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
        public ActionResult AddServiceType(ServiceType obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/ServiceType", obj).Result;
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
        public ActionResult EditingPopup_Update(ServiceType serviceType)
        {
            try
            {
                string result = "fail";
                var ss = GetAllServiceType().ToList().Where(a => a.id == serviceType.id).FirstOrDefault();
                if (ss != null)
                {
                    serviceType.InsertedBy = ss.InsertedBy;
                    serviceType.InsertedOn = ss.InsertedOn;
                    serviceType.IsActive = true;
                    serviceType.IsDelete = false;
                    var res = new RestRequest("api/ServiceType/" + serviceType.id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(serviceType);
                    var response = _client.Execute<List<ServiceType>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "RoomType", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    serviceType.InsertedBy = ss.InsertedBy;
                    serviceType.InsertedOn = ss.InsertedOn;
                    serviceType.IsActive = true;
                    serviceType.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/ServiceType/" + ss.id, serviceType).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "State", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteServiceType(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/ServiceType/" + Convert.ToInt32(id[i])).Result;
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

        public List<City> GetAllCityData()
        {


            HttpResponseMessage res = client.GetAsync("api/City/").Result;
            if (res.IsSuccessStatusCode)
            {
                var city = res.Content.ReadAsAsync<List<City>>().Result.ToList();
                return city;
            }
            else
            {
                return null;
            }
        }
        public ActionResult City_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllCityData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.Id,
                                 a.CityName

                             }).ToList();

                if (query.Count > 0)
                {
                    return Json(query, JsonRequestBehavior.AllowGet);
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
    }
}