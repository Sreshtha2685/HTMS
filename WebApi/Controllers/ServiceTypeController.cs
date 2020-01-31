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
    public class ServiceTypeController : ApiController
    {
        private readonly IServiceTypeService _ServiceTypeService;
        private HTMEntities3 db = new HTMEntities3();

        public ServiceTypeController()
        {
            _ServiceTypeService = new ServiceTypeService();
        }
        /// <summary>
        /// Retrieves the list of values
        /// </summary>
        /// <returns></returns>
        // GET: api/State
        public HttpResponseMessage GetServiceType()
        {
            try
            {
                var lst = _ServiceTypeService.GetAllServiceType();
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

        // GET: api/State/5
        public HttpResponseMessage GetServiceType(int id)
        {
            try
            {
                var Temp = _ServiceTypeService.GetServiceTypeById(id);
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
        // POST: api/ServiceType
        public IHttpActionResult Post(ServiceTypeModel country)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new HTMEntities3())
            {
                ctx.ServiceTypes.Add(new ServiceType()
                {
                    id = country.id,
                    ServiceTypeName = country.ServiceTypeName,
                    Description = country.Description,
                    InsertedBy = country.InsertedBy,
                    InsertedOn = country.InsertedOn,
                    IsActive = country.IsActive,
                    IsDelete = country.IsDelete

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
        // PUT: api/ServiceType/5
        public HttpResponseMessage Put(int id, [FromBody]ServiceType obj)
        {
            try
            {
                ServiceType res = null;
                if (id >= 0)
                {
                    res = _ServiceTypeService.UpdateServiceType(id, obj);
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
        // DELETE: api/ServiceType/5
        public bool Delete(int id)
        {
            if (id >= 0)
                return _ServiceTypeService.DeleteServiceType(id);
            return false;
        }

    }
}
