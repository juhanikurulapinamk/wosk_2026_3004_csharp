



using Api.Dtos;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]

// api/Users
public class UsersController : ControllerBase
{
    // api/User
    [HttpGet]
    
    public async Task<ActionResult<List<AppUser>>> GetUsers()
    {
        try
        {
            var users = await AppUser.GetAll();
            return Ok(users);
        } catch(Exception e)
        {
            return Problem(detail: e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }       
    }

    [HttpPost(Name = "AddUser")]
    public async Task<ActionResult<AppUser>> AddNewUser(AddUserRequest request)
    {
        // jos jokin pakollinen tieto puuttuu request bodysta,
        // lähetetään vastaus HTTP-statuksella 400
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) || string.IsNullOrWhiteSpace(request.Username))
        {
            return BadRequest("First name, last name, and username are required.");
        }

        try
        {
            var user = new AppUser
            {
                Username = request.Username,
                Firstname = request.FirstName,
                Lastname = request.LastName
            };

            bool success = await user.Save();
            if (success)
            {
                // CreatedAtRoute palauttaa statuskoodin 201 ja Location-headerin,
                // joka osoittaa GetUserById-routeen
                return Ok(user);
            }

            return Problem($"Error creating user", statusCode: StatusCodes.Status500InternalServerError);
        } catch (Exception ex)
        {
            return Problem($"Database error: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    // api/Users/x
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetById(long id)
    {
        try
        {
            var user = await AppUser.GetById(id);
            if(user == null)
            {
                return NotFound("user not found");
            }

            return Ok(user);
        } catch(Exception e)
        {
            return Problem(detail: e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

}