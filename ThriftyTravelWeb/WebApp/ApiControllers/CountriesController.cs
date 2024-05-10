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
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public class CountriesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Country, App.BLL.DTO.Country> _mapper;

        public CountriesController(IAppBLL bll, UserManager<AppUser> userManager,
            IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Country, App.BLL.DTO.Country>(autoMapper);
        }

        /// <summary>
        /// Return all countries
        /// </summary>
        /// <returns>List of Countries</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Country>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Country>>> GetCountries()
        {
            var bllResult = await _bll.CountryService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Return Country by its ID
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>Country</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Country), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Country>> GetCountry(Guid id)
        {
            var country = await _bll.CountryService.FirstOrDefaultAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(country);

            return Ok(res);
        }

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <param name="country">Country</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutCountry(Guid id, App.DTO.v1_0.Country country)
        {
            if (id != country.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the country data.");
            }

            if (!await _bll.CountryService.ExistsAsync(id))
            {
                return NotFound("The country with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(country);

            _bll.CountryService.Update(res);
            
            return NoContent();
        }

        /// <summary>
        /// Create new Country
        /// </summary>
        /// <param name="country"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Country>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Country>> PostCountry(App.DTO.v1_0.Country country)
        {
            var mappedCountry = _mapper.Map(country);
            _bll.CountryService.Add(mappedCountry);

            return CreatedAtAction("GetCountry", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = country.Id
            }, country);
        }

        /// <summary>
        /// Delete Country by ID
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            var country = await _bll.CountryService.FirstOrDefaultAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _bll.CountryService.RemoveAsync(id);

            return NoContent();
        }

        
        /// <summary>
        /// Check if Country exists
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool CountryExists(Guid id)
        {
            return _bll.CountryService.Exists(id);
        }
    }
}
