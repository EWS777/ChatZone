using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatZone.Context;
using ChatZone.Core.Models;
using ChatZone.Core.Models.Enums;
using ChatZone.Requests;
using ChatZone.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChatZone.Controllers;

[ApiController]
public class AuthController(ChatDbContext dbContext, IConfiguration configuration) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Registration(RegisterRequest registerRequest)
    {
        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(registerRequest.Password);
        
        var person = new Person()
        {
            Role = PersonRole.User,
            Email = registerRequest.Email,
            Password = hashedPasswordAndSalt.Item1,
            Username = registerRequest.Username,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)

        };
        dbContext.Persons.Add(person);
        dbContext.SaveChanges();
        
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(string Username, string Password)
    {
        Person person = dbContext.Persons.Where(x => x.Username == Username).FirstOrDefault();

        string passwordHashFromDb = person.Password;
        string currentlyHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(Password, person.Salt);

        if (passwordHashFromDb != currentlyHashedPassword) return Unauthorized();
        
        Claim[] userclaim = new[]
        {
            new Claim("UsernameOrEmail", person.Username),
            new Claim("Role", person.Role.ToString())
        };
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        person.RefreshToken = SecurityHelper.GenerateRefreshToken();
        person.RefreshTokenExp = DateTime.Now.AddDays(1);
        dbContext.SaveChanges();
        
        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = person.RefreshToken
        });
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public IActionResult Refresh(string refreshToken)
    {
        Person person = dbContext.Persons.Where(x => x.RefreshToken == refreshToken).FirstOrDefault();
        if (person == null) throw new SecurityTokenException("Invalid refresh token");

        if (person.RefreshTokenExp < DateTime.Now) throw new SecurityTokenException("Refresh token expired");
        
        Claim[] userclaim = new[]
        {
            new Claim("UsernameOrEmail", person.Username),
            new Claim("Role", person.Role.ToString())
        };
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        person.RefreshToken = SecurityHelper.GenerateRefreshToken();
        person.RefreshTokenExp = DateTime.Now.AddDays(1);
        dbContext.SaveChanges();
        
        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = person.RefreshToken
        });
    }
    
    
    [Authorize]
    [HttpGet("getall")]
    public IActionResult GetStudents()
    {
        var claimsFromAccessToken = User.Claims;
        return Ok("Secret data");
    }

    [AllowAnonymous]
    [HttpGet("anon")]
    public IActionResult GetAnonData()
    {
        return Ok("Public data");
    }
}