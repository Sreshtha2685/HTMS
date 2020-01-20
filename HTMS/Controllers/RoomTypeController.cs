

using BusinessService.Service;
using DataModel;
using HTMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;



namespace HTMS.Controllers
{


    public class RoomTypeController : Controller
    {
        HttpClient client = new HttpClient();
         RoomType roomtype = null;
        public List<RoomType> LstAllRoomType = new List<RoomType>();
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public RoomTypeController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: RoomType
        public ActionResult RoomType()
        {
            return View();
        }

        public ActionResult RoomTypeWindow()
        {
            return View();
        }
        public IEnumerable<RoomType> GetAllRoomType()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/RoomType").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllRoomType = res.Content.ReadAsAsync<List<RoomType>>().Result.ToList();
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

        public ActionResult GetAllRoomTypeList([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var skilldetails = GetAllRoomType().OrderByDescending(a => a.id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                throw ex;

            }
            

        }
        private List<RoomType> GetRoomTypeData()
        {
            try
            {
                roomtype = new RoomType();
                var request = new RestRequest("api/RoomType", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<RoomType>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BindAllRoomType([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllRoomType();
                var query = (from a in data
                             select new RoomTypes
                             {
                                 id = a.id,
                                 RoomName = a.RoomName,
                                 BedId = a.BedId,
                                 RoomStatusId = a.RoomStatusId,
                                 Max_Adult_No = a.Max_Adult_No,
                                 Description = a.Description,
                                 Max_Child_No = a.Max_Child_No

                             }).OrderByDescending(x => x.id).ToList();


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
        public ActionResult GetRoomTypeById(int id)
        {
            try
            {
                roomtype = new RoomType();
                var request = new RestRequest("api/RoomType/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<RoomType>>(request);

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
        public ActionResult AddRoomType(RoomType obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/RoomType", obj).Result;
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
        public ActionResult EditingPopup_Update(RoomType room)
        {
            try
            {
                string result = "fail";
                var ss = GetAllRoomType().ToList().Where(a => a.id == room.id).FirstOrDefault();
                if (ss != null)
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;
                    var res = new RestRequest("api/RoomType/" + room.id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(room);
                    var response = _client.Execute<List<RoomType>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "RoomType", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    room.InsertedBy = ss.InsertedBy;
                    room.InsertedOn = ss.InsertedOn;
                    room.IsActive = true;
                    room.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/RoomType/" + ss.id, room).Result;
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

        public JsonResult DeleteRoomType(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/RoomType/" + Convert.ToInt32(id[i])).Result;
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
        //public JsonResult AddRoomType(string RoomName)
        //{
        //    RoomTypes person = new RoomTypes
        //    {
        //        RoomTypeName = RoomName,
        //       // DateTime = DateTime.Now.ToString()
        //    };
        //    return Json(person);
        //}





        //[AllowAnonymous]
        //public string AddRoomType(string RoomName, string Description)
        //{
        //    if (!String.IsNullOrEmpty(RoomName) && !String.IsNullOrEmpty(Description))
        //        //TODO: Save the data in database  
        //        return "Thank you " + RoomName + ". Record Saved.";
        //    else
        //        return "Please complete the form.";
        //}
















        //public ActionResult AddRoomType(RoomType obj)
        //{
        //    try
        //    {
        //         var roomtype = new RoomType();

        //        //object of student class to take input
        //        RoomType stu = new RoomType();

        //            stu.id = obj.id;
        //            stu.RoomName = obj.RoomName;
        //            stu.RoomStatusId = obj.RoomStatusId;
        //            stu.Code = obj.Code;
        //            stu.BedId = obj.BedId;
        //            stu.Description = obj.Description;
        //            stu.Max_Adult_No = obj.Max_Adult_No;
        //            stu.Max_Child_No = obj.Max_Child_No;

        //        //add entity to the add method
        //        db.RoomTypes.Add(stu);

        //        //insert it into table
        //        int res = db.SaveChanges();

        //        if (res > 0)
        //        {

        //            Response.Write("Data Inserted Successfully");

        //        }
        //        else
        //        {

        //            Response.Write("Try Again!!!");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //    }
        //    return RedirectToAction("Index");
        //}
        //Post method to add details    
        //[HttpPost]
        //public ActionResult AddRoomType([Bind(Include = "id,RoomName,Max_Adult_No,Max_Child_No,Code,BedId,Description,RoomStatusId")]RoomType obj)
        //{
        //    if (ModelState.IsValid == true)
        //    {
        //        db.RoomTypes.Add(obj);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");

        //    }
        //    return View();
        //}



        //    //var roomtype = new RoomType();

        //    //object of student class to take input
        //    //RoomType stu = new RoomType();

        //    //stu.id = obj.id;
        //    //stu.RoomName = obj.RoomName;
        //    //stu.RoomStatusId = obj.RoomStatusId;
        //    //stu.Code = obj.Code;
        //    //stu.BedId = obj.BedId;
        //    //stu.Description = obj.Description;
        //    //stu.Max_Adult_No = obj.Max_Adult_No;
        //    //stu.Max_Child_No = obj.Max_Child_No;


        //    ////add entity to the add method
        //    //db.RoomTypes.Add(stu);

        //    ////insert it into table
        //    //int res = db.SaveChanges();

        //    //if (res > 0)
        //    //{

        //    //    Response.Write("Data Inserted Successfully");

        //    //}
        //    //else
        //    //{

        //    //    Response.Write("Try Again!!!");

        //    //}

        //    //{
        //    //    id = obj.id,
        //    //    RoomName = obj.RoomTypeName,
        //    //    RoomStatusId = obj.RoomStatusId,
        //    //    Code = obj.Code,
        //    //    BedId = obj.BedId,
        //    //    Description = obj.Description,
        //    //    Max_Adult_No = obj.Max_Adult_No,
        //    //    Max_Child_No = obj.Max_Child_No
        //    //};
        //    //db.RoomTypes.Add(roomtype);
        //    //db.SaveChanges();
        //   





        //[HttpPost]
        //private ActionResult AddDetails(RoomType objdata)
        //{
        //    try
        //    {


        //        string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
        //        SqlConnection con = new SqlConnection(constr);
        //        SqlCommand com = new SqlCommand("AddRoomType", con);

        //        com.CommandType = CommandType.StoredProcedure;

        //        com.Parameters.AddWithValue("@RoomTypeName", objdata.RoomTypeName);
        //        com.Parameters.AddWithValue("@Max_Adult_No", objdata.Max_Adult_No);
        //        com.Parameters.AddWithValue("@Max_Child_No", objdata.Max_Child_No);
        //        com.Parameters.AddWithValue("@Code", objdata.Code);
        //        com.Parameters.AddWithValue("@BedId", objdata.BedId);
        //        com.Parameters.AddWithValue("@Description", objdata.Description);
        //        com.Parameters.AddWithValue("@RoomStatusId", objdata.RoomStatusId);

        //        con.Open();
        //        com.ExecuteNonQuery();
        //        con.Close();



        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return View("Index");
        //}
    }
    }