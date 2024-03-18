using common_library.Base.Exceptions;
using common_library.Base.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace common_library.Base.Middleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var pathRequest = context.Request.Path.Value;
            string rawContent = await ReadRequestBody(context.Request);
            var currentBody = context.Response.Body;
            //var endpoint = context
            var urls = context.Request.GetDisplayUrl();
            var requestMethod = context.Request.Method;
            var memoryStream = new MemoryStream();
            ObjectProperties obj = new ObjectProperties()
            {
                Request = rawContent,
            };

            try
            {

                context.Response.Body = memoryStream;
                await _next(context);

                context.Response.Body = currentBody;

                memoryStream.Seek(0, SeekOrigin.Begin);

                var readToEnd = new StreamReader(memoryStream).ReadToEnd();

                var result = JsonConvert.DeserializeObject(readToEnd);

                var response = Helper.ResponseWrapper(result, context);

                obj.Response = readToEnd;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            }
            catch (Exception ex)
            {

                context.Response.Body = currentBody;

                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                CustomProblem problem = new();

                switch (ex)
                {
                    case BadRequestException badRequestException:
                        statusCode = HttpStatusCode.BadRequest;
                        problem = new CustomProblem
                        {
                            Title = badRequestException.Message,
                            Status = (int)statusCode,
                            Detail = badRequestException.InnerException?.Message,
                            Type = nameof(BadRequestException),
                            Errors = badRequestException.ValidationErrors

                        };
                        obj.Response = problem.Detail;
                        break;
                    case NotFoundException NotFound:
                        statusCode = HttpStatusCode.NotFound;
                        problem = new CustomProblem
                        {
                            Title = NotFound.Message,
                            Status = (int)statusCode,
                            Detail = NotFound.InnerException?.Message,
                            Type = nameof(NotFoundException),
                        };
                        obj.Response = problem.Detail;
                        break;
                    case UnauthorizedException Unauthorized:
                        statusCode = HttpStatusCode.Unauthorized;
                        problem = new CustomProblem
                        {
                            Title = Unauthorized.Message,
                            Status = (int)statusCode,
                            Detail = Unauthorized.InnerException?.Message,
                            Type = nameof(NotFoundException),
                        };
                        obj.Response = problem.Detail;
                        break;
                    case ForbiddenException Forbidden:
                        statusCode = HttpStatusCode.Forbidden;
                        problem = new CustomProblem
                        {
                            Title = Forbidden.Message,
                            Status = (int)statusCode,
                            Detail = Forbidden.InnerException?.Message,
                            Type = nameof(ForbiddenException),
                        };
                        obj.Response = problem.Detail;
                        break;
                    case NoContentException NoContent:
                        statusCode = HttpStatusCode.NoContent;
                        problem = new CustomProblem
                        {
                            Title = NoContent.Message,
                            Status = (int)statusCode,
                            Detail = NoContent.InnerException?.Message,
                            Type = nameof(NoContentException),
                        };
                        obj.Response = problem.Detail;
                       // Logger.Queue("WARN", $"{requestMethod} {urls}", obj);
                        break;
                    default:
                        problem = new CustomProblem
                        {
                            Title = ex.Message,
                            Status = (int)statusCode,
                            Type = nameof(HttpStatusCode.InternalServerError),
                            Detail = ex.StackTrace,
                        };

                        obj.Response = problem.Detail;

                        break;
                }

                var response = new CommonResponse(context.Request.GetDisplayUrl(), problem, problem.Title, false, (HttpStatusCode)problem.Status);

                context.Response.StatusCode = problem.Status.Value;
                var resp = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsJsonAsync(response);

            }

        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }

        public class ObjectProperties
        {
            public string Request { get; set; } = string.Empty;
            public string Response { get; set; } = string.Empty;
        }
    }
}
