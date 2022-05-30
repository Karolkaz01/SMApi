using Microsoft.AspNetCore.Mvc;
using SMApi.Models;
using SMApi.ModelsDto;
using SMApi.Services;

namespace SMApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeskController : ControllerBase
    {
        private readonly DeskServices _services;
        private readonly OrderServices _orderServices;

        public DeskController(DeskServices services, OrderServices orderServices)
        {
            _services = services;
            _orderServices = orderServices;
        }

        //Get all desks

        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<List<Desk>> Get()
        {
            return _services.Get();
        }

        //[HttpGet]
        //[Route("{id}")]
        //[Authorize(Roles = "Employee,Admin")]
        //public ActionResult<Desk> GetOne(int id)
        //{
        //    var desk = _services.GetOne(id);
        //    if(desk == null)
        //        return NotFound();
        //    return desk;
        //}

        //Create new desk

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Desk> Create(DeskDto deskDto)
        {
            var desk = _services.Add(deskDto);
            if (desk == null)
                return NotFound();
            return CreatedAtAction(nameof(Create), new { id = desk.Id }, desk);
        }

        //Make desk available, uanavailabne or change location

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Desk> Update(DeskDto deskDto,int id)
        {
            var desk = _services.Update(deskDto, id);
            if (desk == null)
                return NotFound();

            return Ok(desk);
        }

        //Delete desk

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (!_orderServices.CanBeDeleted(id))
                return BadRequest("You can't delete reserved desk");

            _services.Delete(id);
            return NoContent();
        }

    }
}
