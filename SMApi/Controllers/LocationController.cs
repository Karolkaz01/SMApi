using Microsoft.AspNetCore.Mvc;
using SMApi.Models;
using SMApi.ModelsDto;
using SMApi.Services;

namespace SMApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationServices _services;
        private readonly DeskServices _deskServices;

        public LocationController(LocationServices services, DeskServices deskServices)
        {
            _services = services;
            _deskServices = deskServices;
        }

        //Get locations name and id

        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<List<Location>> Get()
        {
            return _services.Get();
        }

        //Get locations

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<List<Desk>> GetDesksInLocation(int id)
        {
            var desks = _deskServices.GetFromLocation(id);
            return Ok(desks);
        }

        //Add Location

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Location> Create(LocationDto locationDto)
        {
            var location = _services.Add(locationDto);
            return CreatedAtAction(nameof(Create), new { id = location.Id }, location);
        }

        //Edit location name

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Location> Update(LocationDto locationDto,int id)
        {
            var location = _services.Update(locationDto, id);
            if (location == null)
                return NotFound();

            return Ok(location);
        }

        //Delete Location

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            List<Desk> desks = _deskServices.Get();
            
            foreach(Desk desk in desks)
            {
                if (desk.LocationId == id)
                    return BadRequest("You can't delete location with desks");
            }
          
            _services.Delete(id);
            return NoContent();
        }

    }
}
