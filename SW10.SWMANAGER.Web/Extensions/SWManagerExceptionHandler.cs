using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Microsoft.Ajax.Utilities;
using SW10.SWMANAGER.Web.Areas.Mpa.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SW10.SWMANAGER.Web.Extensions
{
    public class SWManagerExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency
    {
        public void HandleEvent(AbpHandledExceptionData eventData)
        {

            var app = HttpContext.Current; // eventData; // (MvcApplication)sender;
            var context = HttpContext.Current;// app.Context;
            //var ex = app.Server.GetLastError();
            context.Response.Clear();
            context.ClearError();

            ////var httpException = ex as HttpException;


            var routeData = new RouteData();
          //  routeData.Values.Add("controller", "Mpa/sWManagerExceptionHandler");
            routeData.Values.Add("exception", eventData.Exception);
            routeData.Values.Add("action", "exibirErros");



         

            SWManagerExceptionHandlerController controller = new SWManagerExceptionHandlerController();

            //  var result = controller.ExibirErros(eventData.Exception);

            controller.Request.RequestContext.RouteData.Values["action"] = "exibirErros";//   Execute(new System.Web.Routing.RequestContext( new HttpContextWrapper(context), routeData));
        }
    }
}