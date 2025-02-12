using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AutoMapper;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CategoriesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Category, App.BLL.DTO.Category> _mapper;
        
        public CategoriesController(IAppBLL bll, UserManager<AppUser> userManager,
            IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Category, App.BLL.DTO.Category>(autoMapper);
        }

        
        /// <summary>
        /// Return all categories
        /// </summary>
        /// <returns>List of Categories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DTO.v1_0.Category>>> GetCategories()
        {
            var bllCategoriesResult = await _bll.CategoryService.GetAllAsync();
            var bllCategories = bllCategoriesResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(bllCategories);
        }


        /// <summary>
        /// Return Category by its ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Category), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DTO.v1_0.Category>> GetCategory(Guid id)
        {
            var category = await _bll.CategoryService.FirstOrDefaultAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(category);

            return Ok(res);
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="category">Category</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutCategory(Guid id, App.DTO.v1_0.Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the category data.");
            }

            if (!await _bll.CategoryService.ExistsAsync(id))
            {
                return NotFound("The category with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(category);

            _bll.CategoryService.Update(res);
            await _bll.SaveChangesAsync();
            
            return NoContent();
        }
        

        /// <summary>
        /// Create new Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Category>((int) HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DTO.v1_0.Category>> PostCategory(App.DTO.v1_0.Category category)
        {
            var mappedCategory = _mapper.Map(category);
            var res = _bll.CategoryService.Add(mappedCategory!);
            await _bll.SaveChangesAsync();
            
            var publicCategory = _mapper.Map(res);

            return CreatedAtAction("GetCategory", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = publicCategory!.Id
            }, publicCategory);
        }

        /// <summary>
        /// Delete Category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _bll.CategoryService.FirstOrDefaultAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _bll.CategoryService.RemoveAsync(id);

            return NoContent();
        }
    }
}