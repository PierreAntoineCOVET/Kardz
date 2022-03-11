using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Behavior
{
    /// <summary>
    /// Behavior for logging all incoming requests and response.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Log4net logger.
        /// </summary>
        private ILogger Logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">Log4net logger factory.</param>
        public LoggingBehavior(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("LoggingBehavior");
        }

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next">Next handler for the request.</param>
        /// <returns>Response.</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Logger.LogInformation($"{request.GetType().Name} : {JsonSerializer.Serialize(request)}");
            var response = await next();
            Logger.LogInformation($"Response : {JsonSerializer.Serialize(response)}");

            return response;
        }
    }
}
