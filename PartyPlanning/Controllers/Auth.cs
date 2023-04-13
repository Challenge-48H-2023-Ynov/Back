using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PartyPlanning.Data;
using PartyPlanning.Model;
using PartyPlanning.Model.Auth;
using PartyPlanning.Model.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PartyPlanning.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<PartyUser> _userManager;
    private readonly JWTSettings _jwtSettings;

    public AuthController(UserManager<PartyUser> userManager, JWTSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }

    /// <summary>
    /// Initialise les table avec les rôles et l'utilisateur Admin
    /// </summary>
    /// <response code="200 + Message"></response>
    [AllowAnonymous]
    [HttpPost]
    [Route("Initialize")]
    public async Task<IActionResult> Initialize([FromServices] DBInitializer dBInitializer)
    {
        var result = await dBInitializer.Initialize();
        var resultMessage = $"Initialisation DB : {(result ? "Succès" : "DB existe déja")}";

        return Ok(resultMessage);
    }

    /// <summary>
    /// Permet de login un user dans la DB
    /// </summary>
    /// <param name="dto">Model de login d'un user</param>
    /// <response code="400 + Message"></response>
    /// <response code="401">Erreur de mdp ou username</response>
    /// <response code="200">Token + date d'expiration</response>
    [AllowAnonymous]
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? user = await _userManager.FindByEmailAsync(dto.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            IList<string>? userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims = new List<Claim>
                {
                    new Claim("User", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("Roles", userRole));
            }

            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.DurationTime),
                Issuer = _jwtSettings.ValidIssuer,
                Audience = _jwtSettings.ValidAudience,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new TokenDTO(tokenHandler.WriteToken(token), token.ValidTo));
        }
        else return Unauthorized();
    }

    /// <summary>
    /// Permet de register un user dans la DB
    /// </summary>
    /// <param name="dto">Model de l'utilisateur</param>
    /// <response code="400 + Message"></response>
    /// <response code="200 + Message"></response>
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<PartyUserDTO>> Register([FromBody] RegisterDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists != null) return BadRequest("This mail is already taken");

        PartyUser user = new PartyUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.Username,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBrith = DateTime.Now,
            Biography = "Hello :)",
            HavePermis = false,
            EmailConfirmed = true,
        };

        IdentityResult? result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded == false) return BadRequest(result.Errors);

        return Ok(user.ToDTO());
    }

    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? user = await _userManager.FindByEmailAsync(dto.Identifier);
        if (user == null) return BadRequest("No account has been created with this email");

        string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        IdentityResult? result = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok("The password has been successfully changed");
    }
}