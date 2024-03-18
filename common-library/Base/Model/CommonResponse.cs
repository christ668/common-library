using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base.Model
{
    public class CommonResponse
    {

        public object? result { get; }
        public string? error { get; }
        public bool status { get; }
        public HttpStatusCode httpStatusCode { get; }

        public CommonResponse(string requestUrl,
            object? _result,
            string? _error,
            bool _status = false,
            HttpStatusCode _httpStatusCode = HttpStatusCode.InternalServerError)
        {
            result = _status ? _result : string.Empty;
            error = _error;
            status = _status;
            httpStatusCode = _httpStatusCode;
        }
    }
}
