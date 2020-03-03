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
    public class GuestController : Controller
    {
        HttpClient client = new HttpClient();
        Guest guest = null;
        public List<Guest> LstAllGuest = new List<Guest>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public GuestController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Guest
        public ActionResult Guest()
        {
            return View();
        }
        public IEnumerable<Guest> GetAllGuest()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Guest").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllGuest = res.Content.ReadAsAsync<List<Guest>>().Result.ToList();
                    return LstAllGuest.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
        public ActionResult BindAllGuest([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllGuest();
                var data1 = GetAllRoomData();
                var data2 = GetAllServiceTypeData();
               
                var query = (from a in data
                             join b in data1 on a.RoomId equals b.id
                             join c in data2 on a.ServiceTypeId equals c.id
                            
                             select new GuestModel
                             {
                                 Id  = a.Id,
                                 RoomId = a.RoomId,
                                 ServiceTypeId = a.ServiceTypeId,
                                 GuestName = a.GuestName,
                                 GuestContactNumber = a.GuestContactNumber,
                                 GuestAddress = a.GuestAddress,
                                 IdProof = a.IdProof,
                                 Description = a.Description

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
        public ActionResult GetGuestById(int id)
        {
            try
            {
                guest = new Guest();
                var request = new RestRequest("api/Guest/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Guest>>(request);

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
        public ActionResult AddGuest(Guest obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Guest" + "", obj).Result;
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
        public ActionResult EditingPopup_Update(Guest guest)
        {
            try
            {
                string result = "fail";
                var ss = GetAllGuest().ToList().Where(a => a.Id == guest.Id).FirstOrDefault();
                if (ss != null)
                {
                    guest.InsertedBy = ss.InsertedBy;
                    guest.InsertedOn = ss.InsertedOn;
                    guest.IsActive = true;
                    guest.IsDelete = false;
                    var res = new RestRequest("api/Guest/" + guest.Id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(guest);
                    var response = _client.Execute<List<Guest>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Guest", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    guest.InsertedBy = ss.InsertedBy;
                    guest.InsertedOn = ss.InsertedOn;
                    guest.IsActive = true;
                    guest.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/Guest/" + ss.Id, guest).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Guest", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteGuest(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Guest/" + Convert.ToInt32(id[i])).Result;
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
        public List<Room> GetAllRoomData()
        {


            HttpResponseMessage res = client.GetAsync("api/Room/").Result;
            if (res.IsSuccessStatusCode)
            {
                var room = res.Content.ReadAsAsync<List<Room>>().Result.ToList();
                return room;
            }
            else
            {
                return null;
            }
        }
        public ActionResult Room_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllRoomData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.RoomNumber,

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

        public List<ServiceType> GetAllServiceTypeData()
        {


            HttpResponseMessage res = client.GetAsync("api/ServiceType/").Result;
            if (res.IsSuccessStatusCode)
            {
                var room = res.Content.ReadAsAsync<List<ServiceType>>().Result.ToList();
                return room;
            }
            else
            {
                return null;
            }
        }
        public ActionResult ServiceType_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllServiceTypeData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.ServiceTypeName,

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