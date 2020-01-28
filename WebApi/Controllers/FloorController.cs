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
    public class FloorController : ApiController
    {
        private readonly IFloorService _FloorService;
        private HTMEntities3 db = new HTMEntities3();

        public FloorController()
        {
            _FloorService = new FloorService();
        }
        /// <summary>
        /// Retrieves the list of values
        /// </summary>
        /// <returns></returns>
        // GET: api/Status
        public HttpResponseMessage GetFloor()
        {
            try
            {
                var lst = _FloorService.GetAllFloor();
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

        // GET: api/Floor/5
        public HttpResponseMessage GetFloor(int id)
        {
            try
            {
                var Temp = _FloorService.GetFloorById(id);
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
        // POST: api/Floor


        public IHttpActionResult Post(FloorModel status)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new HTMEntities3())
            {
                ctx.Floors.Add(new Floor()
                {
                    id = status.Id,
                    Floor_Number = status.Floor_Number,
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
        public HttpResponseMessage Put(int id, [FromBody]Floor obj)
        {
            try
            {
                Floor res = null;
                if (id >= 0)
                {
                    res = _FloorService.UpdateFloor(id, obj);
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
        // DELETE: api/Floor/5
        public bool Delete(int id)
        {
            if (id >= 0)
                return _FloorService.DeleteFloor(id);
            return false;
        }
    }
}
