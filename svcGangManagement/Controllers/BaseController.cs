using Newtonsoft.Json;
using svcGangManagement.Models;
using svcGangManagement.Services;
using svcGangManagement.Utility;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace svcGangManagement.Controllers
{
    public class BaseController : ApiController
    {
        protected bool SkipUserValidation { get; set; }
        public BaseController()
        {
            SkipUserValidation = false;
        }
        protected HttpResponseMessage Endpoint<T>(Func<T> svcCall)
        {
            var request = BeginRequest();
            var response = new JsonResponse<T>();

            try
            {
                var baseSvc = new BaseService();
                baseSvc.InsertDataLog("api", ReadRequestContent());

                if (SkipUserValidation || ValidateUser())
                {
                    response.addResult(svcCall());
                    return Success(request, response);
                }
                else
                    return AuthenticationFailed(request, response);
            }
            catch (Exception e)
            {
                return ExceptionOccurred(request, response, e);
            }
        }

        private HttpRequestMessage BeginRequest()
        {
            var configuration = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            return request;
        }

        private bool ValidateUser()
        {
            string racfId = HttpContext.Current.Request.Headers["RacfId"];
            string source = HttpContext.Current.Request.Headers["Source"];

            var procedureSvc = new ProcedureService();
            return procedureSvc.ValidateUser(racfId, source);
        }

        private HttpResponseMessage Success<T>(HttpRequestMessage request, JsonResponse<T> response)
        {
            response.processResults();
            return request.CreateResponse(HttpStatusCode.OK, response);
        }

        private HttpResponseMessage AuthenticationFailed<T>(HttpRequestMessage request, JsonResponse<T> response)
        {
            response.addException(new Exception("You are not authorized to submit this request. Please contact Engineering Systems to request access."));
            response.processResults();
            return request.CreateResponse(HttpStatusCode.Unauthorized, response);
        }

        private string ReadRequestContent()
        {
            using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
        }

        private HttpResponseMessage ExceptionOccurred<T>(HttpRequestMessage request, JsonResponse<T> response, Exception e)
        {
            string messages = e.Message;
            Exception current = e;
            while (current.InnerException != null)
            {
                current = current.InnerException;
                messages += " " + current.Message;
            }

            var errorSvc = new ErrorService();
            var error = new Error_Log
            {
                App_Name = "svcThermiteWelding",
                Computer = Environment.MachineName,
                Error_Date = DateTime.Today,
                Error_Time = DateTime.Now,
                Error_Message = messages,
                Method = e.TargetSite.Name,
                Source = e.Source,
                Stack_Trace = e.StackTrace,
            };
            errorSvc.Log(error);

            response.addException(e);
            response.processResults();
            return request.CreateResponse(HttpStatusCode.InternalServerError, response);
        }
    }
}