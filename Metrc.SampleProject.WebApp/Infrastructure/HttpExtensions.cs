using Microsoft.AspNetCore.Http;
using System;

namespace Metrc.SampleProject.WebApp.Infrastructure
{
    public static class HttpExtensions
    {
        public static Boolean IsModal(this HttpContext context)
        {
            var isModal = context.Request.QueryString.Value.Contains("isModal");
            return isModal;
        }
    }
}
