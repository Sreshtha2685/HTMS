using DataModel;
using HTMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace HTMS.Controllers
{
    public class BedController : Controller
    {
        HttpClient client = new HttpClient();
        Bed bed = null;
      
        public List<Bed> LstAllBed = new List<Bed>();
        private readonly HTMEntities3 dbcontext;
        
        private readonly RestClient _client = new RestClient();
        private readonly string _url = ConfigurationManager.AppSettings["url"];
        HTMEntities3 db = new HTMEntities3();
        public BedController()
        {
            _client = new RestClient(_url);
            client.BaseAddress = new Uri(_url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Bed
        public ActionResult Bed()
        {
            //var autogenerate = dbcontext.AutoGenerates.SingleOrDefault(a => a.IdName == "Bed");
            //BedModel beds = new BedModel();
            //{
            //    // beds.BedCode = (autogenerate.Id + 1).ToString();
            //}
            return View();
        }





        public IEnumerable<Bed> GetAllBed()
        {

            try
            {


                HttpResponseMessage res = client.GetAsync("api/Bed").Result;
                if (res.IsSuccessStatusCode)
                {
                    LstAllBed = res.Content.ReadAsAsync<List<Bed>>().Result.ToList();
                    return LstAllBed.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList(); ;
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
                var skilldetails = GetAllBed().OrderByDescending(a => a.id).ToList();
                return Json(skilldetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private List<Bed> GetBedData()
        {
            try
            {
                bed = new Bed();
                var request = new RestRequest("api/Bed", Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Bed>>(request);
                if (response.Data == null)
                    throw new Exception(response.ErrorMessage);
                return response.Data.Where(a => a.IsActive == true && a.IsDelete == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BindAllBed([DataSourceRequest] DataSourceRequest req)
        {
            try
            {

                var data = GetBedData();
                var query = (from a in data
                             select new BedModel
                             {
                                
                                 //BedCode = a.Bed_Code,
                                 BedNo = a.Bed_Number,
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

        [HttpGet]
        public ActionResult GetBedById(int id)
        {
            try
            {
                bed = new Bed();
                var request = new RestRequest("api/Bed/" + id, Method.GET) { RequestFormat = DataFormat.Json };

                var response = _client.Execute<List<Bed>>(request);

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
        public ActionResult AddBed(Bed obj)
        {


            obj.InsertedBy = 1;
            obj.InsertedOn = DateTime.Now;
            obj.IsActive = true;
            obj.IsDelete = false;

            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Bed", obj).Result;
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
        public ActionResult EditingPopup_Update(Bed bed)
        {
            try
            {
                string result = "fail";
                var ss = GetAllBed().ToList().Where(a => a.id == bed.id).FirstOrDefault();
                if (ss != null)
                {
                    bed.InsertedBy = ss.InsertedBy;
                    bed.InsertedOn = ss.InsertedOn;
                    bed.IsActive = true;
                    bed.IsDelete = false;
                    var res = new RestRequest("api/Bed/" + bed.id, Method.PUT) { RequestFormat = DataFormat.Json };
                    res.AddJsonBody(bed);
                    var response = _client.Execute<List<Bed>>(res);

                    if (response.Data == null)
                        throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Bed", res = "" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    bed.InsertedBy = ss.InsertedBy;
                    bed.InsertedOn = ss.InsertedOn;
                    bed.IsActive = true;
                    bed.IsDelete = false;

                    HttpResponseMessage clientRequest = client.PutAsJsonAsync("api/Bed/" + ss.id, bed).Result;
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    //throw new Exception(response.ErrorMessage);
                    return Json(new { result = "Bed", res = "" }, JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult DeleteBed(string[] id)
        {

            //for (int i = 0; i < id.Length; i++)
            //{
            int i = 0;
            while (i < id.Length)
            {


                HttpResponseMessage clientRequest = client.DeleteAsync("api/Bed/" + Convert.ToInt32(id[i])).Result;
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