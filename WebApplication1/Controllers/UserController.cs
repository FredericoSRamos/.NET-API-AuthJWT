using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entities;
using WebApplication1.Infrastructure.Contexts;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly JwtService _jwtService;
    private readonly PasswordService _passwordService;
    
    public UserController(DatabaseContext context, JwtService jwtService, PasswordService passwordService)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordService = passwordService;
    }
    
    [HttpPost("Login")]
    public IActionResult Login([FromBody] User user)
    {
        var dbUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);

        if (dbUser == null || _passwordService.VerifyPassword(user.Password, dbUser.Password))
        {
            return Unauthorized("Dados inválidos");
        }

        var token = _jwtService.GenerateToken(dbUser.Id.ToString(), user.Email, user.IsAdmin);
        
        return Ok(new { Token = token });
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] User user)
    {
        if (string.IsNullOrEmpty(user.Email) ||
            string.IsNullOrEmpty(user.Password) ||
            _context.Users.Any(x => x.Email == user.Email))
        {
            return BadRequest();
        }
        
        user.Password = _passwordService.HashPassword(user.Password);

        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Uma exceção ocorreu ao tentar registrar um usuário: {e.Message}");
            return BadRequest();
        }
        
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.IsAdmin);
        
        return Ok(new { Token = token });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Register/Admin")]
    public IActionResult AdminRegister([FromBody] User user)
    {
        user.IsAdmin = true;
        
        return Register(user);
    }
}