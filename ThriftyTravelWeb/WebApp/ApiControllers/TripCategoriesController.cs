using System.Net;
using App.Contracts.BLL;
using Asp.Versioning;
using AutoMapper;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TripCategoriesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.TripCategory, App.BLL.DTO.TripCategory> _mapper;
        
        public TripCategoriesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.TripCategory, App.BLL.DTO.TripCategory>(autoMapper);
        }

        /// <summary>
        /// Return all trip categories
        /// </summary>
        /// <returns>List of TripCategories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.TripCategory>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.TripCategory>>> GetTripCategories()
        {
            var bllResult = await _bll.TripCategoryService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Return TripCategory by its ID
        /// </summary>
        /// <param name="id">TripCategory ID</param>
        /// <returns>TripCategory</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.TripCategory), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.TripCategory>> GetTripCategory(Guid id)
        {
            var tripCategory = await _bll.TripCategoryService.FirstOrDefaultAsync(id);

            if (tripCategory == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(tripCategory);

            return Ok(res);
        }

        /// <summary>
        /// Update TripCategory
        /// </summary>
        /// <param name="id">TripCategory ID</param>
        /// <param name="tripCategory">TripCategory</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutTripCategory(Guid id, App.DTO.v1_0.TripCategory tripCategory)
        {
            if (id != tripCategory.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the tripCategory data.");
            }

            if (!await _bll.TripCategoryService.ExistsAsync(id))
            {
                return NotFound("The tripCategory with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(tripCategory);

            _bll.TripCategoryService.Update(res);
            
            return NoContent();
        }

        
        /// <summary>
        /// Create new TripCategory
        /// </summary>
        /// <param name="tripCategory"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.TripCategory>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.TripCategory>> PostTripCategory(App.DTO.v1_0.TripCategory tripCategory)
        {
            var mappedTripCategory = _mapper.Map(tripCategory);
            _bll.TripCategoryService.Add(mappedTripCategory);

            return CreatedAtAction("GetTripCategory", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = tripCategory.Id
            }, tripCategory);
        }

        /// <summary>
        /// Delete TripCategory by ID
        /// </summary>
        /// <param name="id">TripCategory ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteTripCategory(Guid id)
        {
            var tripCategory = await _bll.TripCategoryService.FirstOrDefaultAsync(id);
            
            if (tripCategory == null)
            {
                return NotFound();
            }

            await _bll.TripCategoryService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if TripCategory exists
        /// </summary>
        /// <param name="id">TripCategory ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool TripCategoryExists(Guid id)
        {
            return _bll.TripCategoryService.Exists(id);
        }
    }
}
