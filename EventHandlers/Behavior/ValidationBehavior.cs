using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace EventHandlers.Behavior
{
    /// <summary>
    /// Validation bahavior (fluent validation).
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        /// <summary>
        /// List of all validators for the current request type.
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="validators"></param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <param name="cancellationToken">Cancelation token.</param>
        /// <param name="next">Next handler for the request.</param>
        /// <returns><see cref="TResponse"/></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
