using svcGangManagement.Models;
using svcGangManagement.Models.DTO;
using svcGangManagement.Services;
using svcGangManagement.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace svcGangManagement.Controllers
{
    [RoutePrefix("api/Read")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class ReadController : BaseController
    {
        private ReadService baseSvc = new ReadService();

        [HttpGet]
        [Route("SDivision")]
        [ResponseType(typeof(JsonResponse<SDivisionsDto>))]
        public HttpResponseMessage GetSDivision()
        {
            return Endpoint(() => baseSvc.GetSDivision());
        }

        [HttpGet]
        [Route("GangTypePG")]
        [ResponseType(typeof(JsonResponse<GangTypePGDto>))]
        public HttpResponseMessage GangTypePG()
        {
            return Endpoint(() => baseSvc.GangTypePG());
        }

        [HttpGet]
        [Route("GangTypeMWS")]
        [ResponseType(typeof(JsonResponse<GangTypeMWSDto>))]
        public HttpResponseMessage GetGangTypeMWS()
        {
            return Endpoint(() => baseSvc.GetGangTypeMWS());
        }

        [HttpGet]
        [Route("GangPG")]
        [ResponseType(typeof(JsonResponse<GangsPGDto>))]
        public HttpResponseMessage GetGangsPG()
        {
            return Endpoint(() => baseSvc.GetGangsPG());
        }

        [HttpGet]
        [Route("GangMWS")]
        [ResponseType(typeof(JsonResponse<GangsMWSDto>))]
        public HttpResponseMessage GetGangsMWS()
        {
            return Endpoint(() => baseSvc.GetGangsMWS());
        }

        [HttpGet]
        [Route("SupervisorByJobCode")]
        [ResponseType(typeof(JsonResponse<stpGetJobCodeInfo_Result>))]
        public HttpResponseMessage GetSupervisorByJobCode(string jobcode)
        {
            return Endpoint(() => baseSvc.GetSupervisorByJobCode(jobcode));
        }

        [HttpGet]
        [Route("GangDetailsById")]
        [ResponseType(typeof(JsonResponse<stpGetGangSupervisorInfo_Result>))]
        public HttpResponseMessage GetGangDetailsById(string id)
        {
            return Endpoint(() => baseSvc.GetGangDetailsById(id));
        }
    }
}