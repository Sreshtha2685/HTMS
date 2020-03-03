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
    public class RoomController : Controller
    {
        HttpClient client = new HttpClient();
        Room room = null;
        public List<Room> LstAllRoom = new List<Room>();
       
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public RoomController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Room
        public ActionResult Room()
        {
            return View();
        }
        public IEnumerable<Room> GetAllRoom()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Room").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllRoom = res.Content.ReadAsAsync<List<Room>>().Result.ToList();
                    return LstAllRoom.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
        public ActionResult BindAllRoom([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllRoom();
                var data1 = GetAllBedData();
                var data2 = GetAllFloorData();
                var data3 = GetAllHotelData();
                var data4 = GetAllStatusData();
                var query = (from a in data
                             join b in data1 on a.BedId equals b.id
                             join c in data2 on a.FloorId equals c.id
                             join d in data3 on a.HotelId equals d.id
                             join e in data4 on a.RoomStatusId equals e.id
                             select new RoomModel
                             {
                                 id = a.id,
                                 HotelId = a.HotelId,
                                 BedId = a.BedId,
                                 FloorId = a.FloorId,
                                 RoomTypeId = a.RoomTypeId,
                                 RoomNumber = a.RoomNumber,
                                 RoomStatusId= e.id,
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
        public ActionResult GetRoomById(int id)
        {
            try
            {
                room = new Room();
                var request = new RestRequest("api/Room/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Room>>(request);

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
        public ActionResult AddRoom(Room obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Room" +"", obj).Result;
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
        public ActionResult EditingPopup_Update(Room room)
        {
            try
            {
                string result = "fail";
                var ss = GetAllRoom().ToList().Where(a => a.id == room.id).FirstOrDefault();
                if (ss != null)
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;
                    var res = new RestRequest("api/Room/" + room.id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(room);
                    var response = _client.Execute<List<Room>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Room", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/City/" + ss.id, room).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Room", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteRoom(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Room/" + Convert.ToInt32(id[i])).Result;
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

        public List<RoomType> GetAllRoomTypeData()
        {


            HttpResponseMessage res = client.GetAsync("api/RoomType/").Result;
            if (res.IsSuccessStatusCode)
            {
                var room = res.Content.ReadAsAsync<List<RoomType>>().Result.ToList();
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
                var data = GetAllRoomTypeData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.RoomName,

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

        public List<Floor> GetAllFloorData()
        {


            HttpResponseMessage res = client.GetAsync("api/Floor/").Result;
            if (res.IsSuccessStatusCode)
            {
                var floor = res.Content.ReadAsAsync<List<Floor>>().Result.ToList();
                return floor;
            }
            else
            {
                return null;
            }
        }
        public ActionResult Floor_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllFloorData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.Floor_Number,

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

        public List<Hotel> GetAllHotelData()
        {


            HttpResponseMessage res = client.GetAsync("api/Hotel/").Result;
            if (res.IsSuccessStatusCode)
            {
                var hotel = res.Content.ReadAsAsync<List<Hotel>>().Result.ToList();
                return hotel;
            }
            else
            {
                return null;
            }
        }
        public ActionResult Hotell_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllHotelData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.Hotel_Name,

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

        public List<Bed> GetAllBedData()
        {


            HttpResponseMessage res = client.GetAsync("api/Bed/").Result;
            if (res.IsSuccessStatusCode)
            {
                var bed = res.Content.ReadAsAsync<List<Bed>>().Result.ToList();
                return bed;
            }
            else
            {
                return null;
            }
        }
        public ActionResult Beds_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllBedData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.Bed_Number,
                                

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

        public List<Status> GetAllStatusData()
        {


            HttpResponseMessage res = client.GetAsync("api/Status/").Result;
            if (res.IsSuccessStatusCode)
            {
                var status = res.Content.ReadAsAsync<List<Status>>().Result.ToList();
                return status;
            }
            else
            {
                return null;
            }
        }
        public ActionResult Status_Read([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllStatusData().ToList();
                var query = (from a in data
                             select new
                             {
                                 a.id,
                                 a.Status1,

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