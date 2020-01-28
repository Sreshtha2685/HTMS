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
    public class StatusController : ApiController
    {

        private readonly IStatusService _StatusService;
        private HTMEntities3 db = new HTMEntities3();

        public StatusController()
        {
            _StatusService = new StatusService();
        }
        /// <summary>
        /// Retrieves the list of values
        /// </summary>
        /// <returns></returns>
        // GET: api/Status
        public HttpResponseMessage GetStatus()
        {
            try
            {
                var lst = _StatusService.GetAllStatus();
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

        // GET: api/Status/5
        public HttpResponseMessage GetStatus(int id)
        {
            try
            {
                var Temp = _StatusService.GetStatusById(id);
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
        // POST: api/Status


        public IHttpActionResult Post(StatusModel status)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new HTMEntities3())
            {
                ctx.Status.Add(new Status()
                {
                    id = status.id,
                    Status1 = status.Status,
                    Description = status.Description,
                    InsertedOn = status.InsertedOn,
                    InsertedBy = status.InsertedBy,
                    IsActive = status.IsActive,
                    IsDelete = status.IsDelete

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
        // PUT: api/Status/5
        public HttpResponseMessage Put(int id, [FromBody]Status obj)
        {
            try
            {
                Status res = null;
                if (id >= 0)
                {
                    res = _StatusService.UpdateStatus(id, obj);
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
        // DELETE: api/Status/5
        public bool Delete(int id)
        {
            if (id >= 0)
                return _StatusService.DeleteStatus(id);
            return false;
        }
    }
}
