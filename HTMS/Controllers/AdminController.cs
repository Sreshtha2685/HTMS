using System.Configuration;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using DataModel;
using System.Data;

namespace HTMS.Controllers
{
    public class AdminController : Controller
    {
        HttpClient client = new HttpClient();
        private SqlConnection con;

        HTMEntities3 db = new HTMEntities3();
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        
        public ActionResult State()
        {
            return View();
        }
        public ActionResult City()
        {
            return View();
        }

        public ActionResult Reception()
        {


            return View();
        }
        //public ActionResult RoomType()
        //{


        //    return View();
        //}



        //Post method to add details    
        //[HttpPost]
        //public ActionResult RoomType(RoomType obj)
        //{
        //    AddDetails(obj);

        //    return View();
        //}
        //To Handle connection related activities    
        //private void connection()
        //{
        //    string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
        //    con = new SqlConnection(constr);

        //}
        //To add Records into database     
        //private void AddDetails(RoomType obj)
        //{
        //    connection();
        //    SqlCommand com = new SqlCommand("AddRoomType", con);
        //    com.CommandType = System.Data.CommandType.StoredProcedure;
        //   // com.Parameters.AddWithValue("@RoomTypeName", obj.RoomTypeName);
        //    com.Parameters.AddWithValue("@Max_Adult_No", obj.Max_Adult_No);
        //    com.Parameters.AddWithValue("@Max_Child_No", obj.Max_Child_No);
        //    com.Parameters.AddWithValue("@Code", obj.Code);
        //    com.Parameters.AddWithValue("@BedId", obj.BedId);
        //    com.Parameters.AddWithValue("@Description", obj.Description);
        //    com.Parameters.AddWithValue("@RoomStatusId", obj.RoomStatusId);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();

        //}
        
        // POST: Country/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Country1([Bind(Include ="Id,CountryName,Description")] Country countryDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Countries.Add(countryDetail);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(countryDetail);
        //}
    }
}