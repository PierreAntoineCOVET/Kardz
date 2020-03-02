using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Behavior
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private ILogger Logger;

        public LoggingBehavior(ILoggerFactory plop)
        {
            Logger = plop.CreateLogger("LoggingBehavior");

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Logger.LogInformation($"{request.GetType().Name} : {JsonConvert.SerializeObject(request)}");
            var response = await next();
            Logger.LogInformation($"Response : {JsonConvert.SerializeObject(response)}");

            return response;
        }
    }
}
