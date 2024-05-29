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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PhotosController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Photo, App.BLL.DTO.Photo> _mapper;
        
        public PhotosController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Photo, App.BLL.DTO.Photo>(autoMapper);
        }

        /// <summary>
        /// Return all photos
        /// </summary>
        /// <returns>List of Photos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Photo>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Photo>>> GetPhotos()
        {
            var bllResult = await _bll.PhotoService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }
        

        /// <summary>
        /// Return Photo by its ID
        /// </summary>
        /// <param name="id">Photo ID</param>
        /// <returns>Photo</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Photo), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Photo>> GetPhoto(Guid id)
        {
            var photo = await _bll.PhotoService.FirstOrDefaultAsync(id);

            if (photo == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(photo);

            return Ok(res);
        }


        /// <summary>
        /// Update Photo
        /// </summary>
        /// <param name="id">Photo ID</param>
        /// <param name="photo">Photo</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutPhoto(Guid id, App.DTO.v1_0.Photo photo)
        {
            if (id != photo.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the photo data.");
            }

            if (!await _bll.PhotoService.ExistsAsync(id))
            {
                return NotFound("The photo with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(photo);

            _bll.PhotoService.Update(res);
            
            return NoContent();
        }


        /// <summary>
        /// Create new Photo
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Photo>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Photo>> PostPhoto(App.DTO.v1_0.Photo photo)
        {
            var mappedPhoto = _mapper.Map(photo);
            mappedPhoto!.Id = new Guid();
            _bll.PhotoService.Add(mappedPhoto);

            return CreatedAtAction("GetPhoto", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = photo.Id
            }, photo);
        }

        /// <summary>
        /// Delete Photo by ID
        /// </summary>
        /// <param name="id">Photo ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            var photo = await _bll.PhotoService.FirstOrDefaultAsync(id);
            
            if (photo == null)
            {
                return NotFound();
            }

            await _bll.PhotoService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if Photo exists
        /// </summary>
        /// <param name="id">Photo ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool PhotoExists(Guid id)
        {
            return _bll.PhotoService.Exists(id);
        }
        
        /// <summary>
        /// Get Photos by trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>List of Photos.</returns>
        [HttpGet("GetAllPhotosByTripId/{tripId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Photo>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Photo>>> GetAllPhotosByTripId(Guid tripId)
        {
            var photos =
                await _bll.PhotoService.GetAllPhotosByTripId(tripId);

            if (photos.IsNullOrEmpty())
            {
                return NotFound();
            }
            photos =  photos.ToList();

            return Ok(photos);
        }

        /// <summary>
        /// Get Photos by expense id.
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns>List of Photos.</returns>
        [HttpGet("GetAllPhotosByExpenseId/{expenseId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Photo>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Photo>>> GetAllPhotosByExpenseId(Guid expenseId)
        {
            var photos =
                await _bll.PhotoService.GetAllPhotosByExpenseId(expenseId);

            if (photos.IsNullOrEmpty())
            {
                return NotFound();
            }
            photos =  photos.ToList();

            return Ok(photos);
        }
    }
}
