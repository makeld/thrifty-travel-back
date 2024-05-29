using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.DAL.EF;
using Domain.Identity;
using App.DTO;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Asp.Versioning;
using Domain.Identity;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// Controller for account management
/// </summary>
[ApiVersion("1.0")]
[ApiVersion("0.9", Deprecated = true)]
[ApiController]
[Route("/api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly Random _rnd = new();

    /// <summary>
    /// Constructor for AccountController
    /// </summary>
    /// <param name="userManager">UserManager</param>
    /// <param name="logger">ILogger</param>
    /// <param name="signInManager">SignInManager</param>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="context">AppDbContext</param>
    public AccountController(UserManager<AppUser> userManager,
        ILogger<AccountController> logger,
        SignInManager<AppUser> signInManager, 
        IConfiguration configuration, 
        AppDbContext context)
    {
        _userManager = userManager;
        _logger = logger;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    /// <summary>   
    /// Log the user into the app.   
    /// </summary>  
    /// <param name="loginInfo">email, password.</param>
    /// <param name="expiresInSeconds">Time it takes for a jwt to expire.</param>
    /// <returns>The resulting JWTResponse</returns>
    /// <response code="200">The person was successfully logged in.</response>
    /// <response code="404">The person wasn't logged in.</response>  
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<JWTResponse>((int) HttpStatusCode.OK)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult<JWTResponse>> Login([FromBody] LoginInfo loginInfo, 
        [FromQuery] 
        int expiresInSeconds)
    {
        
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        
        var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
        
        // user
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginInfo.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/password problem");
        }
        
        // password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
        if (!result.Succeeded)
        {            
            _logger.LogWarning("WebApi login failed, password {} for email {} was wrong",
                loginInfo.Password, loginInfo.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/password problem");
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null!)
        {
            _logger.LogWarning("WebApi login failed, claimsPrincipal null");
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/password problem");
        }

        appUser.RefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.RefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();

        // remove expired tokens
        foreach (var userRefreshToken in appUser.RefreshTokens)
        {
            if (
                userRefreshToken.ExpirationDT < DateTime.UtcNow &&
                (
                    userRefreshToken.PreviousExpirationDT == null ||
                    userRefreshToken.PreviousExpirationDT < DateTime.UtcNow
                )
            )
            {
                _context.RefreshTokens.Remove(userRefreshToken);
            }
        }

        var refToken = new AppRefreshToken()
        {
            AppUserId = appUser.Id
        };
        _context.RefreshTokens.Add(refToken);
        await _context.SaveChangesAsync();

        // JWT
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims, 
            _configuration.GetValue<string>("JWT:key")!,
            _configuration.GetValue<string>("JWT:issuer")!,
            _configuration.GetValue<string>("JWT:audience")!,
            expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
                ? expiresInSeconds
                : _configuration.GetValue<int>("JWT:ExpiresInSeconds")
        );

        var response = new JWTResponse()
        {
            Jwt = jwt,
            RefreshToken = refToken.RefreshToken
        };

        return Ok(response);
    }

    /// <summary>   
    /// Log the user out from the app.   
    /// </summary>  
    /// <param name="logout">RefreshToken.</param>
    /// <returns>The result of the action</returns>
    /// <response code="200">The user was successfully logged out.</response>
    /// <response code="404">The user couldn't be logged out.</response>  
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<JWTResponse>((int) HttpStatusCode.OK)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.BadRequest)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult> Logout(
        [FromBody] LogoutInfo logout)
    {

        // delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt - that would require pipeline modification and checking against db on every request
        // so client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)

        var userIdStr = _userManager.GetUserId(User);
        if (userIdStr == null)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Invalid refresh token"
                }
            );
        }

        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest("Deserialization error");
        }

        var user = await _context.Users
            .Where(x => x.Id == userId)
            .SingleOrDefaultAsync();
        if (user == null)
        {
            var errorResponse = new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            };

            return NotFound(errorResponse);
        }

        await _context.Entry(user)
            .Collection(x => x.RefreshTokens!)
            .Query()
            .Where(y =>
                (y.RefreshToken == logout.RefreshToken) ||
                (y.PreviousRefreshToken == logout.RefreshToken)
            )
            .ToListAsync();

        foreach (var token in user.RefreshTokens!)
        {
            _context.RefreshTokens.Remove(token);
        }

        var deleteCount = await _context.SaveChangesAsync();
        
        return Ok(new { TokenDeleteCount = deleteCount });
    }

    /// <summary>   
    /// Register new local user into app.   
    /// </summary>  
    /// <param name="registrationData">Email and password.</param>
    /// <param name="expiresInSeconds">optional, override default value</param>
    /// <returns>JWTResponse - jwt and refresh token.</returns>    
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<JWTResponse>((int) HttpStatusCode.OK)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult<JWTResponse>> Register([FromBody] RegisterInfo registrationData,
        [FromQuery]
        int expiresInSeconds
    )
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;

        // verify user
        var user = await _userManager.FindByEmailAsync(registrationData.Email);
        if (user != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            var errorResponse = new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"Email {registrationData.Email} already registered"
            };
            return BadRequest(errorResponse);
        }

        var refreshToken = new AppRefreshToken();
        user = new AppUser()
        {
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<AppRefreshToken>()
            {
                refreshToken
            }
        };
        
        
        // Create the new user.
        var result = await _userManager.CreateAsync(user, registrationData.Password);
        if (!result.Succeeded)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = result.Errors.First().Description
                }
            );
        }

        user = await _userManager.FindByEmailAsync(user.Email);
        if (user == null)
        {
            _logger.LogWarning("User with email {} is not found after registering", registrationData.Email);
            return BadRequest(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"User with email {registrationData.Email} is not found after registration"
                }
            );
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        if (claimsPrincipal == null!)
        {
            _logger.LogWarning("Couldn't get ClaimsPrincipal for user {} ", registrationData.Email);
            return BadRequest($"Couldn't get ClaimsPrincipal for user {registrationData.Email}");
        }
        
        // Generate JWT
        var generatedJwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"]!,
            _configuration["JWT:Issuer"]!,
            _configuration["JWT:Issuer"]!, 
            expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
                ? expiresInSeconds
                : _configuration.GetValue<int>("JWT:ExpiresInSeconds")

        );
        
        var id = user.Id;

        var response = new JWTResponse()
        {
            Jwt = generatedJwt,
            RefreshToken = refreshToken.RefreshToken
        };

        return Ok(response);
    }

    /// <summary>   
    /// Refresh the jwt token.   
    /// </summary>  
    /// <param name="tokenRefreshInfo">Jwt, RefreshToken.</param>
    /// <param name="expiresInSeconds">Time it takes for a token to lose validity.</param>
    /// <returns>The JWTResponse - jwt, refreshToken.</returns>    
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<JWTResponse>((int) HttpStatusCode.OK)]
    [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.BadRequest)]
    [HttpPost]
    public async Task<ActionResult<JWTResponse>> RefreshToken(
        [FromBody] TokenRefreshInfo tokenRefreshInfo,
        [FromQuery]
        int expiresInSeconds
    )
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        

        JwtSecurityToken? jwt;
        try
        {
            jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenRefreshInfo.Jwt);
            if (jwt == null)
            {
                return BadRequest(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Error = "No token"
                    }
                );
            }
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No token"
                }
            );
        }
        // validate jwt, ignore expiration

        var isValid = IdentityHelpers.ValidateJWT(tokenRefreshInfo.Jwt,
            _configuration.GetValue<string>("JWT:key")!,
            _configuration.GetValue<string>("JWT:issuer")!,
            _configuration.GetValue<string>("JWT:audience")!);

        if (!isValid)
        {
            return BadRequest("Invalid token");
        }
        
        
        // extract userId or username from jwt
        var userEmail = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No email in jwt"
                }
            );
        }
        
        // get user
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound($"User with email {userEmail} not found");
        }

        // validate refresh token
        await _context.Entry(user).Collection(x => x.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == tokenRefreshInfo.RefreshToken && x.ExpirationDT > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == tokenRefreshInfo.RefreshToken && x.PreviousExpirationDT > DateTime.UtcNow))
            .ToListAsync();

        if (user.RefreshTokens == null)
        {
            return Problem("RefreshTokens is null");
        }

        if (user.RefreshTokens.Count == 0)
        {
            return Problem("RefreshTokens is empty");
        }

        if (user.RefreshTokens.Count != 1)
        {
            return Problem("More than one valid RefreshToken found");
        }

        // generate jwt
        
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        if (claimsPrincipal == null!)
        {
            _logger.LogWarning("Failed to get ClaimsPrincipal for user {}", userEmail);
            return NotFound("User/password problem");
        }
        
        var generatedJwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"]!,
            _configuration["JWT:Issuer"]!,
            _configuration["JWT:Issuer"]!,
            expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
                ? expiresInSeconds
                : _configuration.GetValue<int>("JWT:ExpiresInSeconds")
        );

        // mark refresh token in db as expired, generate new values
        var refreshToken = user.RefreshTokens.First();
        if (refreshToken.RefreshToken == tokenRefreshInfo.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpirationDT = DateTime.UtcNow.AddMinutes(1);

            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.ExpirationDT = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }
        
        // return data
        var response = new JWTResponse()
        {
            Jwt = generatedJwt,
            RefreshToken = refreshToken.RefreshToken
        };
        return Ok(response);
    }
}