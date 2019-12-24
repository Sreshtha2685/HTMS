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
        //Country country = null;
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
        public List<Country> GetAllCountry()
        {
            HttpResponseMessage GetCompany = client.GetAsync("api/Country").Result;
            if (GetCompany.IsSuccessStatusCode)
            {
                LstAllCountry = GetCompany.Content.ReadAsAsync<List<Country>>().Result.ToList();
                return LstAllCountry;/*.Where(a => a.IsActive == true && a.IsDelete == false).OrderByDescending(a => a.InsertedOn).ToList();*/
            }
            else
            {
                return null;
            }
        }
        public ActionResult CountryNameBinds([DataSourceRequest] DataSourceRequest req)
        {
            try
            {
                var data = GetAllCountry().ToList();
                var query = (from a in data
                             select new CountryModel
                             {
                                 Id = a.Id,
                                 CountryName = a.CountryName,
                                 Description = a.Description,

                             }).OrderByDescending(x => x.Id).ToList();
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
        //public ActionResult AddCountry(Country comp)
        //{
        //    try
        //    {
        //        //comp.InsertedBy = 1;
        //        //comp.InsertedOn = DateTime.Now;
        //        //comp.IsActive = true;
        //        //comp.IsDelete = false;
        //        var response = client.PostAsJsonAsync("api/Country", comp).Result;
        //        if (response.IsSuccessStatusCode)
        //        {

        //            return Json(new { result = "Country", res = "" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //    }
        //}
    }
}