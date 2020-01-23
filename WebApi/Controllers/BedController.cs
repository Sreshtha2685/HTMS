using BusinessService.Interface;
using BusinessService.Service;
using DataModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BedController : ApiController
    {
        private readonly IBedService _BedService;
        private HTMEntities3 db = new HTMEntities3();

        public BedController()
        {
            _BedService = new BedService();
        }
        /// <summary>
        /// Retrieves the list of values
        /// </summary>
        /// <returns></returns>
        // GET: api/Country

        public HttpResponseMessage GetBed()
        {
            try
            {
                var lst = _BedService.GetAllBed();
                if (lst != null)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JArray.FromObject(lst).ToString(), Encoding.UTF8, "application/json")
                    };
                }
                return Request.CreateResponse(HttpStatusCode.OK, lst);
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }


        /// <summary>
        /// Retrieves one value from the list of values
        /// </summary>
        /// <param name=<em>"id"</em>>The id of the item to be retrieved</param>
        /// <returns></returns>
        ///   // GET: api/RoomType/5
        public HttpResponseMessage GetBed(int id)
        {
            try
            {
                var Temp = _BedService.GetBedById(id);
                if (Temp != null)
                    return Request.CreateResponse(HttpStatusCode.OK, Temp);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Customer found for this id");
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Insert a new value in the list
        /// </summary>
        /// <param name=<em>"value"</em>>New value to be inserted</param>
        // POST: api/Bed


        public IHttpActionResult Post(BedModel country)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new HTMEntities3())
            {
                ctx.Beds.Add(new Bed()
                {
                    id = country.id,
                    Bed_Code = country.BedCode,
                    Description = country.Description,
                    Bed_Number = country.BedNumber,
                    
                    //InsertedOn = country.InsertedOn,
                    //InsertedBy = country.InsertedBy,
                    //IsActive = country.IsActive,
                    //IsDelete = country.IsDelete

                });

                ctx.SaveChanges();
            }

            return Ok();
        }


        /// <summary>
        /// Change a single value in the list
        /// </summary>
        /// <param name=<em>"id"</em>>The id of the value to be changed</param>
        /// <param name=<em>"value"</em>>The new value</param>
        // PUT: api/Bed/5
        public HttpResponseMessage Put(int id, [FromBody]Bed obj)
        {
            try
            {
                Bed res = null;
                if (id >= 0)
                {
                    res = _BedService.UpdateBed(id, obj);
                }
                if (res != null)
                    return Request.CreateResponse(HttpStatusCode.OK, res);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data Not Updated");
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }



        /// <summary>
        /// Delete an item from the list
        /// </summary>
        /// <param name=<em>"id"</em>>id of the item to be deleted</param>
        // DELETE: api/Bed/5
        public bool Delete(int id)
        {
            if (id >= 0)
                return _BedService.DeleteBed(id);
            return false;
        }
    }
}
