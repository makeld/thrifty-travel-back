using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LikesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Like, App.BLL.DTO.Like> _mapper;

        public LikesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Like, App.BLL.DTO.Like>(autoMapper);
        }

        /// <summary>
        /// Return all likes
        /// </summary>
        /// <returns>List of Likes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Like>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Like>>> GetLikes()
        {
            var bllResult = await _bll.LikeService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Return Like by its ID
        /// </summary>
        /// <param name="id">Like ID</param>
        /// <returns>Like</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Like), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Like>> GetLike(Guid id)
        {
            var like = await _bll.LikeService.FirstOrDefaultAsync(id);

            if (like == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(like);

            return Ok(res);
        }

        
        /// <summary>
        /// Update Like
        /// </summary>
        /// <param name="id">Like ID</param>
        /// <param name="like">Like</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutLike(Guid id, App.DTO.v1_0.Like like)
        {
            if (id != like.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the like data.");
            }

            if (!await _bll.LikeService.ExistsAsync(id))
            {
                return NotFound("The like with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(like);

            _bll.LikeService.Update(res);
            
            return NoContent();
        }

        
        /// <summary>
        /// Create new Like
        /// </summary>
        /// <param name="like"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Like>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Like>> PostLike(App.DTO.v1_0.Like like)
        {
            var mappedLike = _mapper.Map(like);
            mappedLike!.Id = new Guid();
            _bll.LikeService.Add(mappedLike);

            return CreatedAtAction("GetLike", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = like.Id
            }, like);
        }

        /// <summary>
        /// Delete Like by ID
        /// </summary>
        /// <param name="id">Like ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            var like = await _bll.LikeService.FirstOrDefaultAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            await _bll.LikeService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if Like exists
        /// </summary>
        /// <param name="id">Like ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool LikeExists(Guid id)
        {
            return _bll.LikeService.Exists(id);
        }
        
        /// <summary>
        /// Get Likes by appUser id.
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns>list of Likes.</returns>
        [HttpGet("GetAllLikesByAppUserId/{appUserId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Like>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Like>>> GetAllLikesByAppUserId(Guid appUserId)
        {
            var likes =
                await _bll.LikeService.GetAllLikesByAppUserId(appUserId);
            
            if (likes.IsNullOrEmpty())
            {
                return NotFound();
            }
            likes =  likes.ToList();
            
            return Ok(likes);
        }
        
        /// <summary>
        /// Get Likes by trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>list of Likes.</returns>
        [HttpGet("GetAllLikesByTripId/{tripId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Like>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Like>>> GetAllLikesByTripId(Guid tripId)
        {
            var likes =
                await _bll.LikeService.GetAllLikesByTripId(tripId);
            
            if (likes.IsNullOrEmpty())
            {
                return NotFound();
            }
            likes =  likes.ToList();
            
            return Ok(likes);
        }
    }
}
