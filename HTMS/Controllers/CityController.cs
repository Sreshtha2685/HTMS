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
    public class CityController : Controller
    {
        HttpClient client = new HttpClient();
        City city = null;
        public List<City> LstAllRoomType = new List<City>();
        public List<State> LstAllState = new List<State>();
        public List<Country> LstAllCountry = new List<Country>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public CityController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: City
        public ActionResult City()
        {
            return View();
        }
        public IEnumerable<City> GetAllCity()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/City").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllRoomType = res.Content.ReadAsAsync<List<City>>().Result.ToList();
                    return LstAllRoomType.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
        public ActionResult GetAllCityList([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var skilldetails = GetAllCity().OrderByDescending(a => a.Id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private List<City> GetCityData()
        {
            try
            {
                city = new City();
                var request = new RestRequest("api/City", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<City>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BindAllCity([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllCity();
                var data1 = GetAllStateData();
                var data2 = GetAllCountryData();
                var query = (from a in data
                             join b in data1 on a.StateId equals b.Id
                             join c in data2 on a.CountryId equals c.Id
                             select new CityModel
                             {
                                id = a.Id,
                                 CityName = a.CityName,
                                 //StateId = a.StateId,
                                 //CountryId = a.CountryId,
                                 StateName = b.StateName,
                                 CountryName = c.CountryName
                                 

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
        public ActionResult GetCityById(int id)
        {
            try
            {
                city = new City();
                var request = new RestRequest("api/City/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<City>>(request);

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
        public ActionResult AddCity(City obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/City", obj).Result;
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
        public ActionResult EditingPopup_Update(City city)
        {
            try
            {
                string result = "fail";
                var ss = GetAllCity().ToList().Where(a => a.Id == city.Id).FirstOrDefault();
                if (ss != null)
                {
                    city.InsertedBy = ss.InsertedBy;
                    city.InsertedOn = ss.InsertedOn;
                    city.IsActive = true;
                    city.IsDelete = false;
                    var res = new RestRequest("api/City/" + city.Id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(city);
                    var response = _client.Execute<List<RoomType>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "RoomType", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    city.InsertedBy = ss.InsertedBy;
                    city.InsertedOn = ss.InsertedOn;
                    city.IsActive = true;
                    city.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/City/" + ss.Id, city).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "City", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult Deletecity(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/City/" + Convert.ToInt32(id[i])).Result;
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


        public List<State> GetAllStateData()
        {


            HttpResponseMessage res = client.GetAsync("api/State/").Result;
            if (res.IsSuccessStatusCode)
            {
                var state = res.Content.ReadAsAsync<List<State>>().Result.ToList();
                return state;
            }
            else
            {
                return null;
            }
        }
        public ActionResult State_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllStateData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.Id,
                                 a.StateName,

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


        public List<Country> GetAllCountryData()
        {


            HttpResponseMessage res = client.GetAsync("api/Country/").Result;
            if (res.IsSuccessStatusCode)
            {
                var country = res.Content.ReadAsAsync<List<Country>>().Result.ToList();
                return country;
            }
            else
            {
                return null;
            }
        }
        public ActionResult Country_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllCountryData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.Id,
                                 a.CountryName,

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