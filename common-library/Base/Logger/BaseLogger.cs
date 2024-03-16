using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base.Logger
{
    public class BaseLogger<T> : IBaseLogger<T>
    {
        private readonly ILogger<T> _logger;

        public BaseLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
        public object? PrepareData(object item)
        {
            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(item));
        }

    }
}
