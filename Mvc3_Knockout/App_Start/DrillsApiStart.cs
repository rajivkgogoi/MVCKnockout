using System;
using System.Web.Routing;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Activation;
using Microsoft.ApplicationServer.Http.Description;
using BasketballPlaybook.DependencyResolution;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BasketballPlaybook.App_Start.DrillsApiStart), "Start")]

namespace BasketballPlaybook.App_Start {
    public static class DrillsApiStart {
        public static void Start() {
			var iocContainer = IoC.Initialize();
            var config = new WebApiConfiguration();
            config.CreateInstance = (type, instance, message) => { return iocContainer.GetInstance(type); };
            config.EnableTestClient = true;
            RouteTable.Routes.SetDefaultHttpConfiguration(config);
            RouteTable.Routes.MapServiceRoute<DrillsApi>("drills");
        }
    }
}