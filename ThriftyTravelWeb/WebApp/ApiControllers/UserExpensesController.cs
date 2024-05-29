using System.Net;
using App.Contracts.BLL;
using Asp.Versioning;
using AutoMapper;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserExpensesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserExpense, App.BLL.DTO.UserExpense> _mapper;
        
        public UserExpensesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.UserExpense, App.BLL.DTO.UserExpense>(autoMapper);
        }

        /// <summary>
        /// Return all User Expenses
        /// </summary>
        /// <returns>List of UserExpenses</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.UserExpense>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.UserExpense>>> GetUserExpenses()
        {
            var bllResult = await _bll.UserExpenseService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        
        /// <summary>
        /// Return UserExpense by its ID
        /// </summary>
        /// <param name="id">UserExpense ID</param>
        /// <returns>UserExpense</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.UserExpense), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.UserExpense>> GetUserExpense(Guid id)
        {
            var userExpense = await _bll.UserExpenseService.FirstOrDefaultAsync(id);

            if (userExpense == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(userExpense);

            return Ok(res);
        }

        

        /// <summary>
        /// Update UserExpense
        /// </summary>
        /// <param name="id">UserExpense ID</param>
        /// <param name="userExpense">UserExpense</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutUserExpense(Guid id, App.DTO.v1_0.UserExpense userExpense)
        {
            if (id != userExpense.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the userExpense data.");
            }

            if (!await _bll.UserExpenseService.ExistsAsync(id))
            {
                return NotFound("The userExpense with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(userExpense);

            _bll.UserExpenseService.Update(res);
            
            return NoContent();
        }

        /// <summary>
        /// Create new UserExpense
        /// </summary>
        /// <param name="userExpense"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.UserExpense>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.UserExpense>> PostUserExpense(App.DTO.v1_0.UserExpense userExpense)
        {
            var mappedUserExpense = _mapper.Map(userExpense);
            mappedUserExpense!.Id = new Guid();
            _bll.UserExpenseService.Add(mappedUserExpense);

            return CreatedAtAction("GetUserExpense", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = userExpense.Id
            }, userExpense);
        }

         
        /// <summary>
        /// Delete UserExpense by ID
        /// </summary>
        /// <param name="id">UserExpense ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteUserExpense(Guid id)
        {
            var userExpense = await _bll.UserExpenseService.FirstOrDefaultAsync(id);
            
            if (userExpense == null)
            {
                return NotFound();
            }

            await _bll.UserExpenseService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if UserExpense exists
        /// </summary>
        /// <param name="id">UserExpense ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool UserExpenseExists(Guid id)
        {
            return _bll.UserExpenseService.Exists(id);
        }
        
        /// <summary>
        /// Get UserExpenses by expense id.
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns>list of UserExpenses.</returns>
        [HttpGet("GetAllUserExpensesByExpenseId/{expenseId}")]
        [ProducesResponseType<List<App.DTO.v1_0.UserExpense>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.UserExpense>>> GetAllUserExpensesByExpenseId(Guid expenseId)
        {
            var userExpenses =
                await _bll.UserExpenseService.GetAllUserExpensesByExpenseId(expenseId);
            
            if (userExpenses.IsNullOrEmpty())
            {
                return NotFound();
            }
            userExpenses =  userExpenses.ToList();
            
            return Ok(userExpenses);
        }
        
        /// <summary>
        /// Get UserExpenses by tripUser Id.
        /// </summary>
        /// <param name="tripUserId"></param>
        /// <returns>list of UserExpenses.</returns>
        [HttpGet("GetAllUserExpensesByTripUserId/{tripUserId}")]
        [ProducesResponseType<List<App.DTO.v1_0.UserExpense>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.UserExpense>>> GetAllUserExpensesByTripUserId(Guid tripUserId)
        {
            var userExpenses =
                await _bll.UserExpenseService.GetAllUserExpensesByTripUserId(tripUserId);
            
            if (userExpenses.IsNullOrEmpty())
            {
                return NotFound();
            }
            userExpenses =  userExpenses.ToList();
            
            return Ok(userExpenses);
        }
    }
}
