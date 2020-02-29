using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// Base controller for the application.
    /// </summary>
    public abstract class BaseController : Controller
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
