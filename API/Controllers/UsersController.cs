



using Api.Dtos;
using API.Dtos;
using API.Factories;
using API.Models;
using API.Repositories;
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
            using var repo = UsersRepositoryFactory.Create();
            var users = await repo.GetAll();
            return Ok(users);
        } catch(Exception e)
        {
            return Problem(detail: e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }       
    }

    [HttpPut("{id}")]
    [HttpPatch("{id}", Name = "UpdateUserById")]
    public async Task<ActionResult<AppUser>> UpdateUser(long id, AddUserRequest request)
    {
        try
        {
            using var repo = new UsersSQLiteRepository();

            var user = await repo.GetById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            // jos request body ei sisällä first_name-avainta ja sen arvoa, käytetään käyttäjän oo. tietokannasta löytyvää etunimeä
            user.Firstname = string.IsNullOrWhiteSpace(request.FirstName) ? user.Firstname : request.FirstName;
            // jos request body ei sisällä last_name-avainta ja sen arvoa, käytetään käyttäjän oo. tietokannasta löytyvää sukunimeä
            user.Lastname = string.IsNullOrWhiteSpace(request.LastName) ? user.Lastname : request.LastName;
            // jos request body ei sisällä username-avainta ja sen arvoa, käytetään käyttäjän oo. tietokannasta löytyvää käyttäjänimeä
            user.Username = string.IsNullOrWhiteSpace(request.Username) ? user.Username : request.Username;

            bool success = await repo.Save(user);
            if (success)
            {
                return Ok(user);
            }

            return Problem($"Error updating user", statusCode: StatusCodes.Status500InternalServerError);

        } catch (Exception ex)
        {
            return Problem($"Database error: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
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
            using var repo = new UsersSQLiteRepository();
            bool success = await repo.Save(user);
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
            using var repo = new UsersSQLiteRepository();
            var user = await repo.GetById(id);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveById(long id)
    {
        try
        {
            using var repo = new UsersSQLiteRepository();
            var user = await repo.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            var removed = await repo.Remove(user);
            if(removed)
            {
                return NoContent();
            }

            return Problem(detail: "Error removing user", statusCode: StatusCodes.Status500InternalServerError);
        } catch(Exception e)
        {
            return Problem(detail: e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

}