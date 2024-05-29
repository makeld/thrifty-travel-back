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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpensesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Expense, App.BLL.DTO.Expense> _mapper;

        public ExpensesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Expense, App.BLL.DTO.Expense>(autoMapper);
        }

        /// <summary>
        /// Return all expenses
        /// </summary>
        /// <returns>List of Expenses</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Expense>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Expense>>> GetExpenses()
        {
            var bllResult = await _bll.ExpenseService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Return Expense by its ID
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <returns>Expense</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Expense), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Expense>> GetExpense(Guid id)
        {
            var expense = await _bll.ExpenseService.FirstOrDefaultAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(expense);

            return Ok(res);
        }


        /// <summary>
        /// Update Expense
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <param name="expense">Expense</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutExpense(Guid id, App.DTO.v1_0.Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the expense data.");
            }

            if (!await _bll.ExpenseService.ExistsAsync(id))
            {
                return NotFound("The expense with the specified ID does not exist.");
            }

            var res = _mapper.Map(expense);

            _bll.ExpenseService.Update(res);

            return NoContent();
        }


        /// <summary>
        /// Create new Expense
        /// </summary>
        /// <param name="expense"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Expense>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Expense>> PostExpense(App.DTO.v1_0.Expense expense)
        {
            var mappedExpense = _mapper.Map(expense);
            mappedExpense!.Id = new Guid();
            _bll.ExpenseService.Add(mappedExpense);

            return CreatedAtAction("GetExpense", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = expense.Id
            }, expense);
        }

        /// <summary>
        /// Delete Expense by ID
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expense = await _bll.ExpenseService.FirstOrDefaultAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            await _bll.ExpenseService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if Expense exists
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool ExpenseExists(Guid id)
        {
            return _bll.ExpenseService.Exists(id);
        }
        
        /// <summary>
        /// Get Expenses by trip id.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>list of Expenses.</returns>
        [HttpGet("GetExpenseByTripId/{tripId}")]
        [ProducesResponseType<List<App.DTO.v1_0.Expense>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Expense>>> GetExpenseByTripId(Guid tripId)
        {
            var expenses =
                await _bll.ExpenseService.GetExpenseByTripId(tripId);
            
            if (expenses.IsNullOrEmpty())
            {
                return NotFound();
            }
            expenses =  expenses.ToList();
            
            return Ok(expenses);
        }
        
        
        
    }
}
