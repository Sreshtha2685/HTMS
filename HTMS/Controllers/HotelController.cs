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
    public class HotelController : Controller
    {
        HttpClient client = new HttpClient();
        Hotel hotel = null;
        public List<Hotel> LstAllHotel = new List<Hotel>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public HotelController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Hotel
        public ActionResult Hotel()
        {
            return View();
        }
        public IEnumerable<Hotel> GetAllHotel()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Hotel").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllHotel = res.Content.ReadAsAsync<List<Hotel>>().Result.ToList();
                    return LstAllHotel.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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

        public ActionResult GetAllHotelList([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var skilldetails = GetAllHotel().OrderByDescending(a => a.id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private List<Hotel> GetHotelData()
        {
            try
            {
                hotel = new Hotel();
                var request = new RestRequest("api/Hotel", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Hotel>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BindAllHotel([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllHotel();
                var query = (from a in data
                             select new HotelModel
                             {
                                 id = a.id,
                                 HotelName = a.Hotel_Name,
                                 HotelNumber = a.Hotel_Number,
                                

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
        public ActionResult GetHotelById(int id)
        {
            try
            {
                hotel = new Hotel();
                var request = new RestRequest("api/Hotel/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Hotel>>(request);

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
        public ActionResult AddHotel(Hotel obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Hotel", obj).Result;
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
        public ActionResult EditingPopup_Update(Hotel hotel)
        {
            try
            {
                string result = "fail";
                var ss = GetAllHotel().ToList().Where(a => a.id == hotel.id).FirstOrDefault();
                if (ss != null)
                {
                    hotel.InsertedBy = ss.InsertedBy;
                    hotel.InsertedOn = DateTime.Now;
                    hotel.IsActive = true;
                    hotel.IsDelete = false;
                    var res = new RestRequest("api/Hotel/" + hotel.id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(hotel);
                    var response = _client.Execute<List<Hotel>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Hotel", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    hotel.InsertedBy = 1;
                    hotel.InsertedOn = DateTime.Now;
                    hotel.IsActive = true;
                    hotel.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/Hotel/" , hotel).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Hotel", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteHotel(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Hotel/" + Convert.ToInt32(id[i])).Result;
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