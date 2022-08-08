using svcGangManagement.Models;
using svcGangManagement.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace svcGangManagement.Services
{
    public class BaseService
    {
        protected ProdGangEntities prodGangEntities = new ProdGangEntities();
        protected MWSTRackEntities mWSTRackEntities = new MWSTRackEntities();
        protected ErrorLoggerEntities errorLoggerEntities = new ErrorLoggerEntities();
        protected Rail_TestingEntities railTestingEntities = new Rail_TestingEntities();
        protected CHRISEntities cHRISEntities = new CHRISEntities();

        private Settings settings = new Settings();
        public BaseService()
        {
            try
            {
                switch (settings.AppLevel.ToUpper())
                {
                    case "D":
                        prodGangEntities.Database.Connection.ConnectionString = "data source=engdevsql.nscorp.com;initial catalog=ProdGang;user id=PrdGngDV;password=sqcfw52n;multipleactiveresultsets=True;App=svcThermiteWelding";
                        errorLoggerEntities.Database.Connection.ConnectionString = "data source=engdevsql.nscorp.com;initial catalog=ErrorLogger;user id=RailTsDV;password=ybegs9ad;multipleactiveresultsets=True;App=svcThermiteWelding";
                        railTestingEntities.Database.Connection.ConnectionString = "data source=engdevsql.nscorp.com;initial catalog=Rail_Testing;user id=RailTsDv;password=ybegs9ad;multipleactiveresultsets=True;App = svcThermiteWelding";
                        cHRISEntities.Database.Connection.ConnectionString = "Data Source=engDEVsql.nscorp.com;Initial Catalog=CHRIS;Persist Security Info=True;User Id=dbctn;Password=ESas2004; Connection Timeout=300";
                        break;
                    case "Q":
                        prodGangEntities.Database.Connection.ConnectionString = "data source=engqasql.nscorp.com;initial catalog=ProdGang;user id=PrdGngQA;password=afb8j7e3;multipleactiveresultsets=True;App=svcThermiteWelding";
                        errorLoggerEntities.Database.Connection.ConnectionString = "data source=engqasql.nscorp.com;initial catalog=ErrorLogger;user id=RailTsQA;password=dbzkwevf;multipleactiveresultsets=True;App=svcThermiteWelding";
                        railTestingEntities.Database.Connection.ConnectionString = "data source=engqasql.nscorp.com;initial catalog=Rail_Testing;user id=RailTsQA;password=dbzkwevf;multipleactiveresultsets=True;App =svcThermiteWelding";
                        cHRISEntities.Database.Connection.ConnectionString = "Data Source=engQAsql.nscorp.com;Initial Catalog=CHRIS;Persist Security Info=True;User Id=dbctn;Password=ESas2004; Connection Timeout=300";
                        break;
                    case "P":
                        prodGangEntities.Database.Connection.ConnectionString = "data source=engsql.nscorp.com;initial catalog=ProdGang;user id=esPrdGng;password=tbzyj6zu;multipleactiveresultsets=True;App=svcThermiteWelding";
                        errorLoggerEntities.Database.Connection.ConnectionString = "data source=engsql.nscorp.com;initial catalog=ErrorLogger;user id=esRailTs;password=tr4ru9rw;multipleactiveresultsets=True;App=svcThermiteWelding";
                        railTestingEntities.Database.Connection.ConnectionString = "data source=engsql.nscorp.com;initial catalog=Rail_Testing;user id=esRailTs;password=tr4ru9rw;multipleactiveresultsets=True;App =svcThermiteWelding";
                        cHRISEntities.Database.Connection.ConnectionString = "Data Source=engsql.nscorp.com;Initial Catalog=CHRIS;Persist Security Info=True;User Id=dbctn;Password=ESas2004; Connection Timeout=300";
                        break;
                    default:
                        throw new Exception($"Invalid AppLevel setting:{ settings.AppLevel.ToUpper()}");
                }

                errorLoggerEntities.Database.CommandTimeout = 2000;
                prodGangEntities.Database.CommandTimeout = 2000;
                mWSTRackEntities.Database.CommandTimeout = 2000;
                railTestingEntities.Database.CommandTimeout = 2000;
                cHRISEntities.Database.CommandTimeout = 2000;
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public void InsertDataLog(string interfaceName, string clientJson)
        {
            var urlParts = HttpContext.Current.Request.Url.ToString().Split('/').ToList();
            int apiIndex = urlParts.IndexOf(interfaceName);
            string verb = null, method = null;
            if (apiIndex != -1 && urlParts.Count > apiIndex + 2)
            {
                verb = urlParts[apiIndex + 1];
                method = urlParts[apiIndex + 2];
            }

            if (verb != null && (verb.ToLower() == "create" || verb.ToLower() == "update" || verb.ToLower() == "destroy" || verb.ToLower() == "error" || method.ToLower().Contains("approvewelds")))
            {
                ThermiteWelding_JsonDataLog api = new ThermiteWelding_JsonDataLog
                {
                    CreatedBy = HttpContext.Current.Request.Headers["RacfId"],
                    CreatedDate = DateTime.Now,
                    JsonData = clientJson,
                    RequestType = verb,
                    Source = HttpContext.Current.Request.Headers["Source"],
                    Method = method
                };
                prodGangEntities.ThermiteWelding_JsonDataLog.Add(api);
                prodGangEntities.SaveChanges();
            }
        }

        protected HttpClient BuildApiClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}