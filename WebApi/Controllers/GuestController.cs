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

    public class GuestController : ApiController
    {
        private readonly IGuestService _GuestService;
        private HTMEntities3 db = new HTMEntities3();

        public GuestController()
        {
            _GuestService = new GuestService();
        }
        /// <summary>
        /// Retrieves the list of values
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetGuest()
        {
            try
            {
                var lst = _GuestService.GetAllGuest();
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

        // GET: api/Room/5
        public HttpResponseMessage GetGuest(int id)
        {
            try
            {
                var Temp = _GuestService.GetGuestById(id);
                if (Temp != null)
                    return Request.CreateResponse(HttpStatusCode.OK, Temp);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Room found for this id");
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
        // POST: api/Country
        public IHttpActionResult Post(GuestModel guest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new HTMEntities3())
            {
                ctx.Guests.Add(new Guest()
                {
                    Id = guest.Id,
                    GuestName = guest.GuestName,
                    GuestContactNumber = guest.GuestContactNumber,
                    GuestAddress = guest.GuestAddress,
                    IdProof = guest.IdProof,
                    RoomId = guest.RoomId,
                    ServiceTypeId = guest.ServiceTypeId,
                    Description = guest.Description,
                    InsertedBy = guest.InsertedBy,
                    InsertedOn = guest.InsertedOn,
                    IsActive = guest.IsActive,
                    IsDelete = guest.IsDelete

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
        // PUT: api/Room/5
        public HttpResponseMessage Put(int id, [FromBody]Guest obj)
        {
            try
            {
                Guest res = null;
                if (id >= 0)
                {
                    res = _GuestService.UpdateGuest(id, obj);
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
        // DELETE: api/Room/5
        public bool Delete(int id)
        {
            if (id >= 0)
                return _GuestService.DeleteGuest(id);
            return false;
        }
    }
}
