using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace timeSheetApplication
{
    public class TimeSheetViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) 
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var descriptor = (context.ActionContext.ActionDescriptor as ControllerActionDescriptor);
            if (descriptor == null) { return viewLocations; }
            
            return new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/HR/{0}.cshtml",
                "~/Views/Employee/{0}.cshtml",
                "~/Views/_Layout.cshtml",
                "~/Identity/Account/{0}.cshtml",
                "/{2}/{1}/{0}.cshtml"
            };
        }
    }
}