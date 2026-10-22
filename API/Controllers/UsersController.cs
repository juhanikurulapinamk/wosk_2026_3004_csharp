



using Api.Dtos;
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