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
    public class CountryController : Controller
    {
        HttpClient client = new HttpClient();
        Country country = null;
        public List<Country> LstAllCountry = new List<Country>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];



        public CountryController()
        {
            // _client = new RestClient(_url);

            var url1 = ConfigurationManager.AppSettings["url"];
            client.BaseAddress = new Uri(url1);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Country
        public ActionResult Country1()
        {
            return View();
        }
        public ActionResult Country()
        {
            return View();
        }
        //public ActionResult BindAllCountry([DataSourceRequest]DataSourceRequest req)
        //{
        //    try
        //    {
        //        var data1 = GetAllCountry().ToList();
        //        var _detail = (from a in data1
        //                       select new CountryModel
        //                       {
        //                           Id = a.Id,
        //                           CountryName = a.CountryName,
        //                           Description = a.Description,

        //                       }).OrderByDescending(x => x.Id).ToList();
        //        if (_detail.ToList().Count() > 0)
        //        {
        //            return Json(_detail.ToDataSourceResult(req), JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}
        public IEnumerable<Country> GetAllCountry()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Country").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllCountry = res.Content.ReadAsAsync<List<Country>>().Result.ToList();
                    return LstAllCountry.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); 
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

        public ActionResult BindAllCountry([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllCountry();
                var query = (from a in data
                             select new CountryModel
                             {
                                Id = a.Id,
                                 CountryName = a.CountryName,
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



        public ActionResult GetCountryById(int id)
        {
            try
            {
                country = new Country();
                var request = new RestRequest("api/Country/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Country>>(request);

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
        public ActionResult AddCountry(Country obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Country", obj).Result;
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
        public ActionResult EditingPopup_Update(Country room)
        {
            try
            {
                string result = "fail";
                var ss = GetAllCountry().ToList().Where(a => a.Id == room.Id).FirstOrDefault();
                if (ss != null)
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;
                    var res = new RestRequest("api/Country/" + room.Id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(room);
                    var response = _client.Execute<List<Country>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Country", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/Country/" + ss.Id, room).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "RoomType", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public JsonResult DeleteCountry(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Country/" + Convert.ToInt32(id[i])).Result;
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