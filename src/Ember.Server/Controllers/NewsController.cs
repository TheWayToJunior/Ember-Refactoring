using Ember.Server.Helpers;
using Ember.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Editor")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService newsServece;

        public NewsController(INewsService newsServece)
        {
            this.newsServece = newsServece ?? throw new ArgumentNullException(nameof(newsServece));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NewsPost>>> GetAll([FromQuery] PaginationDTO pagination,
            CategoryMode category = CategoryMode.All)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var postasDescending = newsServece.GetAll().OrderByDescending(news => news.Id);

            IQueryable<NewsPost> posts = category == CategoryMode.All
                ? postasDescending
                : postasDescending.Where(news => news.Category == category);

            await HttpContext.InsertPaginationsPerPage(posts, pagination.QuantityPerPage)
                .ConfigureAwait(true);

            return Ok(posts.Pagination(pagination)
                 .ToList());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<NewsPost>> Get(int id)
        {
            NewsPost newsPost = await newsServece.GetById(id)
                .ConfigureAwait(true);

            if (newsPost == null)
            {
                return NotFound();
            }

            return Ok(newsPost);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NewsPost value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityEntry = await newsServece.AddAsync(value)
                .ConfigureAwait(true);

            return CreatedAtAction("Get", new { Id = entityEntry.Id }, entityEntry);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] NewsPost value)
        {
            if (value == null || value.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await newsServece.UpdateAsync(value)
                    .ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool newsPostExists = await newsServece.AnyAsync(id)
                    .ConfigureAwait(true);

                if (!newsPostExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            NewsPost newsPost = await newsServece.GetById(id)
                .ConfigureAwait(true);

            if (newsPost == null)
            {
                return NotFound();
            }

            await newsServece.DeleteAsync(newsPost)
                .ConfigureAwait(true);

            return NoContent();
        }
    }
}
