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
    public class StateController : Controller
    {
        HttpClient client = new HttpClient();
        State state = null;
        public List<City> LstAllCity = new List<City>();
        public List<State> LstAllState = new List<State>();
        public List<Country> LstAllCountry = new List<Country>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public StateController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: State
        public ActionResult State()
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
                    LstAllCity = res.Content.ReadAsAsync<List<City>>().Result.ToList();
                    return LstAllCity.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
        public IEnumerable<State> GetAllState()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/State").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllState = res.Content.ReadAsAsync<List<State>>().Result.ToList();
                    return LstAllState.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
        public ActionResult GetAllStateList([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var skilldetails = GetAllState().OrderByDescending(a => a.Id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private List<State> GetStateData()
        {
            try
            {
                state = new State();
                var request = new RestRequest("api/State", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<State>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BindAllState([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllCity();
                var data1 = GetAllState();
                
                var query = (from a in data
                             join b in data1 on a.Id equals b.CityId
                            
                             select new StateModel
                             {
                                   id = a.Id,
                                 CityName = a.CityName,
                                 //StateId = a.StateId,
                                 //CountryId = a.CountryId,
                                 StateName = b.StateName,
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
        public ActionResult GetStateById(int id)
        {
            try
            {
                state = new State();
                var request = new RestRequest("api/State/" + id, Method.GET) { RequestFormat = DataFormat.Json };

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
        public ActionResult AddState(State obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/State", obj).Result;
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
        public ActionResult EditingPopup_Update(State state)
        {
            try
            {
                string result = "fail";
                var ss = GetAllCity().ToList().Where(a => a.Id == state.Id).FirstOrDefault();
                if (ss != null)
                {
                    state.InsertedBy = ss.InsertedBy;
                    state.InsertedOn = ss.InsertedOn;
                    state.IsActive = true;
                    state.IsDelete = false;
                    var res = new RestRequest("api/State/" + state.Id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(state);
                    var response = _client.Execute<List<State>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "State", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    state.InsertedBy = ss.InsertedBy;
                    state.InsertedOn = ss.InsertedOn;
                    state.IsActive = true;
                    state.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/State/" + ss.Id, state).Result;
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

        public JsonResult DeleteState(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/State/" + Convert.ToInt32(id[i])).Result;
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