using Ember.Application.Interfaces;
using Ember.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IEmailService emailService;

        public FeedbackController(IEmailService emailService)
        {
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage([FromBody] MessageRequest message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await emailService.SendMessage(message)
                    .ConfigureAwait(true);

                return Ok("The message is sent!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred, see logging entries");
            }
        }
    }
}