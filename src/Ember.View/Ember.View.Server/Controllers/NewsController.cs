using Ember.Application.Features.News.Commands.Create;
using Ember.Application.Features.News.Commands.Delete;
using Ember.Application.Features.News.Commands.Update;
using Ember.Application.Features.News.Queries;
using Ember.Application.Features.News.Queries.GetPage;
using Ember.Domain;
using Ember.Domain.Contracts;
using Ember.Shared;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Editor")]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IResult<GetPageNewsResponse>>> GetAll([FromBody] GetPageNewsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<News>> Get(int id)
        {
            return Ok(await _mediator.Send(new GetNewsQuery { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<IResult>> Create([FromBody] CreateNewsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateNewsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteNewsCommand { Id = id }));
        }
    }
}
