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
        private readonly PublicDTOBllMapper<App.DTO.v1_0.AddExpense, App.BLL.DTO.AddExpense> _addExpenseMapper;

        public ExpensesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Expense, App.BLL.DTO.Expense>(autoMapper);
            _addExpenseMapper = new PublicDTOBllMapper<App.DTO.v1_0.AddExpense, App.BLL.DTO.AddExpense>(autoMapper);
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
        /// <param name="expenseData"></param>
        /// <returns>Expense</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Expense>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Expense>> PostExpense(App.DTO.v1_0.AddExpense expenseData)
        {
            var mappedExpense = _addExpenseMapper.Map(expenseData);
            
            var res= await _bll.ExpenseService.CreateExpenseWithAttributesAsync(mappedExpense!);
            await _bll.SaveChangesAsync();
            
            var publicExpense = _mapper.Map(res);
            return CreatedAtAction("GetExpense", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = publicExpense!.Id
            }, publicExpense);
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
            
            var photos = await _bll.PhotoService.GetAllPhotosByExpenseId(id);
            foreach (var photo in photos)
            {
                await _bll.PhotoService.RemoveAsync(photo!.Id);
            }

            await _bll.ExpenseService.RemoveAsync(id);

            return NoContent();
        }
        
        
        /// <summary>
        /// Get Location by TripLocation Id
        /// </summary>
        /// <param name="tripLocationId"></param>
        /// <returns></returns>
        [HttpGet("GetLocationByTripLocationId/{tripLocationId}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Location), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Location>> GetLocationByTripLocationId(Guid tripLocationId)
        {
            try
            {
                var location = await _bll.ExpenseService.GetLocationByTripLocationId(tripLocationId);
                return Ok(location);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
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
        
        /// <summary>
        /// Calculate the total sum of expenses in a trip.
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>Total sum of expenses.</returns>
        [HttpGet("CalculateExpensesTotal/{tripId}")]
        [ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> CalculateExpensesTotal(Guid tripId)
        {
            try
            {
                var total = await _bll.ExpenseService.CalculateExpensesTotal(tripId);
                return Ok(total);
            }
            catch (Exception)
            {
                return NotFound("Could not calculate total expenses or trip not found.");
            }
        }
        
        
        
    }
}
