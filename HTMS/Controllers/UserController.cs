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
    public class UserController : Controller
    {
        HttpClient client = new HttpClient();
        User user = null;
        public List<User> LstAllUser = new List<User>();
       
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public UserController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: User
        public ActionResult User()
        {
            return View();
        }
        public IEnumerable<User> GetAllUser()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/User").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllUser = res.Content.ReadAsAsync<List<User>>().Result.ToList();
                    return LstAllUser.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
        public IEnumerable<User> GetAllUserDetails()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/User").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllUser = res.Content.ReadAsAsync<List<User>>().Result.ToList();
                    return LstAllUser.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
       
        private List<User> GetUserData()
        {
            try
            {
                user = new User();
                var request = new RestRequest("api/User", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<User>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult BindAllUser([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetAllUser();
                var query = (from a in data
                             
                             select new UserModel
                             {
                                 id = a.UserId,
                                 UserName = a.UserName,
                                 UserRole = a.UserRole,
                                 Description = a.Description,
                                 Password = a.Password
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
        public ActionResult GetUserById(int id)
        {
            try
            {
                user = new User();
                var request = new RestRequest("api/User/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<User>>(request);

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
        public ActionResult AddUser(User obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/User", obj).Result;
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
        public ActionResult EditingPopup_Update(User state)
        {
            try
            {
                string result = "fail";
                var ss = GetAllUser().ToList().Where(a => a.UserId== state.UserId).FirstOrDefault();
                if (ss != null)
                {
                    state.InsertedBy = ss.InsertedBy;
                    state.InsertedOn = ss.InsertedOn;
                    state.IsActive = true;
                    state.IsDelete = false;
                    var res = new RestRequest("api/User/" + state.UserId, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(state);
                    var response = _client.Execute<List<User>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "User", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    state.InsertedBy = ss.InsertedBy;
                    state.InsertedOn = ss.InsertedOn;
                    state.IsActive = true;
                    state.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/User/" + ss.UserId, state).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "User", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteUser(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/User/" + Convert.ToInt32(id[i])).Result;
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