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
    public class FloorController : Controller
    {
        HttpClient client = new HttpClient();
        Floor floor = null;
        public List<Floor> LstAllFloor = new List<Floor>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];



        public FloorController()
        {
            // _client = new RestClient(_url);

            var url1 = ConfigurationManager.AppSettings["url"];
            client.BaseAddress = new Uri(url1);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Floor
        public ActionResult Floor()
        {
            return View();
        }
        public IEnumerable<Floor> GetAllFloor()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Floor").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllFloor = res.Content.ReadAsAsync<List<Floor>>().Result.ToList();
                    return LstAllFloor.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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

        public ActionResult BindAllFloor([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllFloor();
                var query = (from a in data
                             select new FloorModel
                             {
                                 Id = a.id,
                                 FloorNumber = a.Floor_Number,
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



        public ActionResult GetFloorById(int id)
        {
            try
            {
                floor = new Floor();
                var request = new RestRequest("api/Floor/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Floor>>(request);

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
        public ActionResult AddFloor(Floor obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Floor", obj).Result;
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
        public ActionResult EditingPopup_Update(Floor room)
        {
            try
            {
                string result = "fail";
                var ss = GetAllFloor().ToList().Where(a => a.id == room.id).FirstOrDefault();
                if (ss != null)
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;
                    var res = new RestRequest("api/Floor/" + room.id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(room);
                    var response = _client.Execute<List<Floor>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Floor", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/Floor/" + ss.id, room).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Floor", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public JsonResult DeleteFloor(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Floor/" + Convert.ToInt32(id[i])).Result;
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