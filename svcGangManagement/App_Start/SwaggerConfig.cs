using svcGangManagement;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WebActivatorEx;


[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace svcGangManagement
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            string strUser = System.Environment.UserName;
            strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1).ToUpper();
            if (strUser == "DBCTN" || strUser.Length > 5)
                strUser = "";

            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "RacfId",
                @in = "header",
                type = "string",
                @default = strUser,
                required = true
            });

            operation.parameters.Add(new Parameter
            {
                name = "Source",
                @in = "header",
                type = "string",
                @default = "Swagger",
                required = true
            });
        }
    }
    public class SortEndpointsFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
        {
            var paths = swaggerDoc.paths.OrderBy(e => e.Key).ToList();
            swaggerDoc.paths = paths.ToDictionary(e => e.Key, e => e.Value);
        }
    }
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.OperationFilter<AddRequiredHeaderParameter>();
                    c.DocumentFilter<SortEndpointsFilter>();
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    c.SingleApiVersion("v1", "svcGangManagement");
                })
                .EnableSwaggerUi(c =>
                {
                    c.DisableValidator();
                });
        }
        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\SwaggerDemoApi.XML",
                           System.AppDomain.CurrentDomain.BaseDirectory);//also change in your projectpropertiesbuild
        }
    }
}
