using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// Base controller for the application.
    /// </summary>
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// MediatR request pipe.
        /// </summary>
        public IMediator Mediator { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="mediator">MediatR request pipe.</param>
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
