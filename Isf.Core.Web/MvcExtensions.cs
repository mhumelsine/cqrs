using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isf.Core.Web
{
    public static class MvcExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, Notification notification)
        {
            foreach (var prop in notification.ErrorDictionary)
            {
                foreach (var error in prop.Value)
                {
                    modelState.AddModelError(prop.Key, error);
                }

            }
        }
    }
}
