using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myappdotnet.DTOs;
using myappdotnet.Model;
using myappdotnet.Service;
using System.Net.Mail;

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
    [Route("login")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Login([FromBody] LoginDTO loginDTO)
    {
        try
        {
            if (loginDTO == null || loginDTO.Email == null || loginDTO.Password == null)
            {
                return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Invalid data" });
            }
            bool isAuthenticated = authService.AuthenticateUser(loginDTO.Email, loginDTO.Password);

            if (!isAuthenticated)
            {
                return Unauthorized(new ErrorResponseDTO { Status = 401, Error = "Invalid credentials" });
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
    [Route("all-users")]
    [ProducesResponseType(typeof(List<AboutUserDTO>), StatusCodes.Status200OK)]
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
    [Route("create-user")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult CreateUser([FromBody] CreateUserDTO createUserDTO)
    {
        try
        {
            if (createUserDTO == null || createUserDTO.Email == null || createUserDTO.Password == null || createUserDTO.Surname == null || createUserDTO.First_name == null)
            {
                return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Invalid data" });
            }

            try
            {
                MailAddress mailAddress = new MailAddress(createUserDTO.Email);
            }
            catch (FormatException)
            {
                return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Invalid email address" });
            }
            bool registrationSuccess = authService.RegisterUser(createUserDTO.Email, createUserDTO.Password, createUserDTO.Surname, createUserDTO.First_name);

            if (registrationSuccess)
            {
                return Ok(new { message = "Registration successful" });
            }
            else
            {
                return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Registration failed" });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }

    }

    [HttpGet]
    [Route("about-user")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AboutUserDTO), StatusCodes.Status200OK)]
    public IActionResult GetUserInfo()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            var username = HttpContext.User.Identity.Name;
            var user = context.MyUser.FirstOrDefault(u => u.Email == username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist" });
            }

            var userDTO = new AboutUserDTO
            {
                Email = user.Email,
                Surname = user.Surname,
                First_name = user.First_name
            };

            return Ok(userDTO);
        }
        else
        {
            return Unauthorized(new ErrorResponseDTO { Status = 401, Error = "Unauthorized access." });
        }
    }

    [HttpPut]
    [Route("update-password")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdatePassword([FromBody] PasswordChangeDTO passwordChange)
    {
        try
        {
            if (passwordChange == null)
            {
                return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Invalid data" });
            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Identity.Name;
                var user = context.MyUser.FirstOrDefault(u => u.Email == username);
                if (user == null)
                {
                    return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist" });
                }
                bool psw = BCrypt.Net.BCrypt.Verify(passwordChange.CurrentPassword, user.Password);
                if (psw)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordChange.NewPassword);
                    user.Password = hashedPassword;
                    context.SaveChanges();
                }
                else
                {
                    return BadRequest(new ErrorResponseDTO { Status = 400, Error = "Invalid data" });
                }

                return Ok(new { message = "Updated password" });
            }
            else
            {
                return Unauthorized(new ErrorResponseDTO { Status = 401, Error = "Unauthorized access." });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet]
    [Route("user-id/{id}")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(AboutUserDTO), StatusCodes.Status200OK)]
    public IActionResult GetUsersById(int id)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist" });
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
    [Route("user-email/{email}")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(AboutUserDTO), StatusCodes.Status200OK)]
    public IActionResult GetUsersByEmail(string email)
    {
        var user = context.MyUser.FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist" });
        }

        var userDTO = new AboutUserDTO
        {
            Email = user.Email,
            Surname = user.Surname,
            First_name = user.First_name
        };

        return Ok(userDTO);
    }

    [HttpDelete]
    [Route("delete-user")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteUser()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            var username = HttpContext.User.Identity.Name;
            var user = context.MyUser.FirstOrDefault(u => u.Email == username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO { Status = 404, Error = "User do not exist" });
            }
            int id_user = user.Id;
            var userActivities = context.MyUser_Activities.Where(ua => ua.UserId == id_user).ToList();
            if (userActivities.Any())
            {
                context.MyUser_Activities.RemoveRange(userActivities);
            }
            var userToRemove = context.MyUser.FirstOrDefault(u => u.Id == id_user);
            if (userToRemove != null)
            {
                context.MyUser.Remove(userToRemove);
            }

            context.SaveChanges();

            return Ok(new { message = "The user was deleted." });
        }
        else
        {
            return Unauthorized(new ErrorResponseDTO { Status = 401, Error = "Unauthorized access." });
        }
    }
}
