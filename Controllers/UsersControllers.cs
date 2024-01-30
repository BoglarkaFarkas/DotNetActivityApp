using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myappdotnet.DTOs;
using myappdotnet.Model;
using myappdotnet.Service;

namespace myappdotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly AuthService authService;
    public UsersController(ApplicationDbContext context)
    {
        this.context = context;
        this.authService = new AuthService(context); 
    }
    [HttpPost]
    [Route("Login")]
    public IActionResult Login([FromBody] LoginDTO loginDTO)
    {
        try
        {
            if (loginDTO == null)
            {
                return BadRequest("Invalid data");
            }
            bool isAuthenticated = authService.AuthenticateUser(loginDTO.Email, loginDTO.Password);

            if (!isAuthenticated)
            {
                return Unauthorized(new { error = "Invalid credentials" });
            }
            return Ok(new { message = "Login successful" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }       
    [HttpGet]
    [Route("users")]
    public IEnumerable<AboutUserDTO> GetUsers()
    {
        var users = context.MyUser.ToList();
        var userDTOs = new List<AboutUserDTO>();

        foreach (var user in users)
        {
            var userDTO = new AboutUserDTO
            {
                Email = user.Email,
                Surname = user.Surname,
                First_name = user.First_name
            };

            userDTOs.Add(userDTO);
        }

        return userDTOs;
    }

    [HttpPost]
    [Route("User")]
    public IActionResult CreateUser([FromBody] CreateUserDTO createUserDTO)
    {
        try{
            if (createUserDTO == null)
            {
                return BadRequest("Invalid data");
            }

            bool registrationSuccess = authService.RegisterUser(createUserDTO.Email, createUserDTO.Password, createUserDTO.Surname, createUserDTO.First_name);

            if (registrationSuccess)
            {
                return Ok(new { message = "Registration successful"  });
            }
            else
            {
                return BadRequest(new { error = "Registration failed"});
            }
        }
        catch (Exception ex){
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }

    }
    [HttpGet]
    [Route("hellobello")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult Hello()
    {
        return Ok(new { message = "Hello World!" });
    }

    [HttpGet]
    [Route("aboutuser")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult GetUserInfo()
    {
        if (HttpContext.User.Identity.IsAuthenticated){
            var username = HttpContext.User.Identity.Name;
            var user = context.MyUser.FirstOrDefault(u => u.Email == username);

            if (user == null){
                return NotFound();
            }

            var userDTO = new AboutUserDTO
            {
                Email = user.Email,
                Surname = user.Surname,
                First_name = user.First_name
            };

            return Ok(userDTO);
        }
        else{
        return Unauthorized(); 
        }
    }

    [HttpPut]
    [Route("updatepassword")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public IActionResult UpdatePassword([FromBody] PasswordChangeDTO passwordChange)
    {
        try{
            if (passwordChange == null)
            {
                return BadRequest("Invalid data");
            }
            if (HttpContext.User.Identity.IsAuthenticated){
                var username = HttpContext.User.Identity.Name;
                var user = context.MyUser.FirstOrDefault(u => u.Email == username);
                if (user == null)
                {
                    return BadRequest("Invalid data");
                }       
                bool psw = BCrypt.Net.BCrypt.Verify(passwordChange.CurrentPassword, user.Password);
                if(psw){
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordChange.NewPassword);
                    user.Password = hashedPassword;
                    context.SaveChanges();
                }else{
                    return BadRequest("Invalid data");
                }
 
                return Ok("Updated Password");
            }
            else{
                return Unauthorized(); 
            }
        }catch (Exception ex){
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("UserId/{id}")]
    public IActionResult GetUsersById(int id){
        var user = context.MyUser.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(); 
        }

        var userDTO = new AboutUserDTO
        {
            Email = user.Email,
            Surname = user.Surname,
            First_name = user.First_name
        };

        return Ok(userDTO); 
    }

    [HttpGet]
    [Route("UserEmail/{email}")]
    public IActionResult GetUsersByEmail(string email){
        var user = context.MyUser.FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return NotFound(); 
        }

        var userDTO = new AboutUserDTO
        {
            Email = user.Email,
            Surname = user.Surname,
            First_name = user.First_name
        };

        return Ok(userDTO); 
    }

    [HttpGet]
    [Route("Hello")]
    public String GetUsersasd()
    {
        return "Helloka";
    }
}
