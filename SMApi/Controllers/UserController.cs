using Microsoft.AspNetCore.Mvc;
using SMApi.Models;
using SMApi.ModelsDto;
using SMApi.Services;

namespace SMApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {

        private readonly UserServices _services;

        public UserController(UserServices services)
        {
            _services = services;
        }

        //Get all users
        
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return _services.Get();
        }

        //[HttpGet]
        //[Route("{id}")]
        //public ActionResult<User> GetOne(int id)
        //{
        //    var user = _services.GetOne(id);
        //    if (user == null)
        //        return NotFound();
        //    return user;
        //}

        //[HttpPost]
        //public ActionResult<User> Create(string Name, Role AccountType)
        //{
        //    var user = _services.GetByName(Name);
        //    if (user != null)
        //        return BadRequest("User with that name already exist");
        //    user = _services.Add(Name, AccountType);
        //    return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        //}

        //Edit Account type

        [HttpPut]
        public ActionResult<User> Update(string Name, Role AccountType)
        {
            var user = _services.Update(Name, AccountType);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            _services.Delete(id);
            return NoContent();
        }

    }
}
