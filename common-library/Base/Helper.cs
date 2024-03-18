using common_library.Base.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base
{
    public static class Helper
    {
        public static CommonResponse ResponseWrapper(object? result, HttpContext context, object? exception = null)
        {
            var requestUrl = context.Request.GetDisplayUrl();
            var data = result;
            var error = exception != null ? "Internal Server Error" : null;
            var status = result != null && (HttpStatusCode)context.Response.StatusCode == HttpStatusCode.OK;
            var httpStatusCode = (HttpStatusCode)context.Response.StatusCode;


            return new CommonResponse(requestUrl, data, error, status, httpStatusCode);
        }

    }
}
