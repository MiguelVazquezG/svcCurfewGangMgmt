using svcGangManagement.Models.DTO;
using svcGangManagement.Services;
using svcGangManagement.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace svcGangManagement.Controllers
{
    [RoutePrefix("api/Create")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class CreateController : BaseController
    {
        CreateService baseSvc = new CreateService();

        [HttpPost]
        //[Route("SDivision")]
        [ResponseType(typeof(JsonResponse<bool>))]
        public HttpResponseMessage Post(List<NewGang> gangs)
        {
            return Endpoint(() => baseSvc.Post(gangs));
        }

        [HttpPost]
        [Route("EditGang")]
        [ResponseType(typeof(JsonResponse<bool>))]
        public HttpResponseMessage EditGang(List<NewGang> gangs)
        {
            return Endpoint(() => baseSvc.EditGang(gangs));
        }
    }
}