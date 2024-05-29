using System.Net;
using App.Contracts.BLL;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Comment, App.BLL.DTO.Comment> _mapper;

        public CommentsController(IAppBLL bll, UserManager<AppUser> userManager,
            IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Comment, App.BLL.DTO.Comment>(autoMapper);
        }

        /// <summary>
        /// Return all comments
        /// </summary>
        /// <returns>List of Comments</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Comment>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Comment>>> GetComments()
        {
            var bllResult = await _bll.CommentService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }
        

        /// <summary>
        /// Return Comment by its ID
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>Comment</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Comment), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Comment>> GetComment(Guid id)
        {
            var comment = await _bll.CommentService.FirstOrDefaultAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(comment);

            return Ok(res);
        }
        

        /// <summary>
        /// Update Comment
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <param name="comment">Comment</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutComment(Guid id, App.DTO.v1_0.Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the comment data.");
            }

            if (!await _bll.CommentService.ExistsAsync(id))
            {
                return NotFound("The comment with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(comment);

            _bll.CommentService.Update(res);
            
            return NoContent();
        }

        /// <summary>
        /// Create new Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Comment>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Comment>> PostComment(App.DTO.v1_0.Comment comment)
        {
            var mappedComment = _mapper.Map(comment);
            mappedComment!.Id = new Guid();
            _bll.CommentService.Add(mappedComment);

            return CreatedAtAction("GetComment", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = comment.Id
            }, comment);
        }

        /// <summary>
        /// Delete Comment by ID
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _bll.CommentService.FirstOrDefaultAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _bll.CommentService.RemoveAsync(id);

            return NoContent();
        }

        
        /// <summary>
        /// Check if Comment exists
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool CommentExists(Guid id)
        {
            return _bll.CommentService.Exists(id);
        }
        
        /// <summary>
        /// Get Comments by trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>List of Comments.</returns>
        [HttpGet("GetAllCommentsByTripId/{tripId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Comment>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Comment>>> GetAllCommentsByTripId(Guid tripId)
        {
            var comments =
                await _bll.CommentService.GetAllCommentsByTripId(tripId);

            if (comments.IsNullOrEmpty())
            {
                return NotFound();
            }
            comments = comments.ToList();

            return Ok(comments);
        }

        /// <summary>
        /// Get Comments by appUser id.
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns>List of Comments.</returns>
        [HttpGet("GetAllCommentsByAppUserId/{appUserId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Comment>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Comment>>> GetAllCommentsByAppUserId(Guid appUserId)
        {
            var photos =
                await _bll.CommentService.GetAllCommentsByAppUserId(appUserId);

            if (photos.IsNullOrEmpty())
            {
                return NotFound();
            }
            photos = photos.ToList();

            return Ok(photos);
        }
    }
}
