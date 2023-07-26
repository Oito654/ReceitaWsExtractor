using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReceitaWsExtractor.Application.Commands;
using ReceitaWsExtractor.Application.DTO;
using ReceitaWsExtractor.Application.Queries;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.WebApi.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace ReceitaWsExtractor.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
    private readonly UserManager<Client> _userManager;
    private readonly IMediator _mediator;

    public ClientController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<Client> userManager, IMediator mediator)
    {
        _jwtBearerTokenSettings = jwtTokenOptions.Value;
        _userManager = userManager;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #region Post

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] InUser user)
    {
        try
        {
            if (!ModelState.IsValid || user.Email == null || user.Password == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var identityUser = new Client() { UserName = $"{user.Name}.{user.Surname}", Email = user.Email, Name = user.Name, Surname = user.Surname, CreatedIn = DateTime.Now };
            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (!result.Succeeded)
            {
                string errorMessage = "";
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                    errorMessage = error.Description;
                }

                return new BadRequestObjectResult(new { Message = errorMessage, Errors = dictionary });
            }

            return Ok(new { Message = "User Reigistration Successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }


    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] InLogin login)
    {
        try
        {
            Client identityUser;

            if (!ModelState.IsValid
                || login.Email == null
                || login.Password == null
                || (identityUser = await ValidateUser(login.Email, login.Password)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }

            var token = GenerateToken(identityUser);

            var command = new AddTokenCommand();
            command.Token = token.ToString();
            command.Client = identityUser;

            await _mediator.Send(command);

            return Ok(new { Token = token, Id = identityUser.Id, Message = "Success" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("create-order")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand createOrder)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();
            var client = await _userManager.FindByIdAsync(createOrder.ClientId.ToString());

            if (client == null)
            {
                return NotFound("Client doesn't exist");
            }

            var token = client.Token;

            if (token.IsValid == false || token.Token != accessToken)
            {
                return Unauthorized("Error 401 Unauthorized. Token is Invalid");
            }

            var result = await _mediator.Send(createOrder);

            return Ok(result);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    #endregion

    #region Put

    [HttpPut("update-client")]
    [Authorize]
    public async Task<IActionResult> UpdateClient([FromBody]UpdateCommand updateClientCommand)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();
            var client = await _userManager.FindByIdAsync(updateClientCommand.Id.ToString());

            if (client == null)
            {
                return NotFound("Client doesn't exist");
            }

            var token = client.Token;

            if (token.IsValid == false || token.Token != accessToken)
            {
                return Unauthorized("Error 401 Unauthorized. Token is Invalid");
            }

            var result = await _mediator.Send(updateClientCommand);
            await _userManager.ChangePasswordAsync(client, updateClientCommand.CurrentPassword, updateClientCommand.Password);

            return Ok(result);
        } 
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }

    [HttpPut]
    [Route("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromQuery] Guid id)
    {
        try
        {
            Client identityUser;

            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            if (!ModelState.IsValid
                || id == null
                || (identityUser = await _userManager.FindByIdAsync(id.ToString())) == null)
            {
                return new BadRequestObjectResult(new { Message = "Logout failed" });
            }

            var token = identityUser.Token;

            if (token.IsValid == false || token.Token != accessToken)
            {
                return Unauthorized("Error 401 Unauthorized. Token is Invalid");
            }

            var command = new InvalidTokenCommand();
            command.Client = identityUser;

            await _mediator.Send(command);

            return Ok(new { Message = "Logout Success" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    #endregion

    #region Delete

    [HttpDelete]
    [Route("delete-client")]
    [Authorize]
    public async Task<IActionResult> DeleteClient([FromQuery] Guid id)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();
            var client = await _userManager.FindByIdAsync(id.ToString());

            if(client == null)
            {
                return NotFound("Client doesn't exist");
            }

            var token = client.Token;

            if (token.IsValid == false || token.Token != accessToken)
            {
                return Unauthorized("Error 401 Unauthorized. Token is Invalid");
            }

            await _userManager.DeleteAsync(client);

            return Ok("User deleted");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }


    }

    #endregion

    #region Get

    [HttpGet("get-order")]
    [Authorize]
    public async Task<IActionResult> GetOrder([FromQuery] GetOrderQuery query)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();
            var client = await _userManager.FindByIdAsync(query.ClientId.ToString());

            if (client == null)
            {
                return NotFound("Client doesn't exist");
            }

            var token = client.Token;

            if (token.IsValid == false || token.Token != accessToken)
            {
                return Unauthorized("Error 401 Unauthorized. Token is Invalid");
            }

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("get-client-all-orders")]
    [Authorize]
    public async Task<IActionResult> GetAllClientOrders([FromQuery] GetAllOrdersClientQuery query)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();
            var client = await _userManager.FindByIdAsync(query.Id.ToString());

            if (client == null)
            {
                return NotFound("Client doesn't exist");
            }

            var token = client.Token;

            if (token.IsValid == false || token.Token != accessToken)
            {
                return Unauthorized("Error 401 Unauthorized. Token is Invalid");
            }

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    #endregion

    #region Others
    private async Task<Client> ValidateUser(string email, string password)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser != null)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, password);
            return result == PasswordVerificationResult.Failed ? null : identityUser;
        }

        return null;
    }


    private object GenerateToken(Client identityUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email)
            }),

            Expires = DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtBearerTokenSettings.Audience,
            Issuer = _jwtBearerTokenSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    #endregion
}