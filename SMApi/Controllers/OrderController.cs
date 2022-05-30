using Microsoft.AspNetCore.Mvc;
using SMApi.Models;
using SMApi.ModelsDto;
using SMApi.Services;
using System.Security.Claims;

namespace SMApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly OrderServices _services;
        private readonly UserServices _userServices;
        private readonly DeskServices _deskServices;

        public OrderController(OrderServices services, UserServices userServices, DeskServices deskServices)
        {
            _services = services;
            _userServices = userServices;
            _deskServices = deskServices;
        }

        //Get all information in all orders

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Orders")]
        public ActionResult<List<Order>> Get()
        {
            return _services.Get();
        }

        //Get orders of current user

        [HttpGet]
        [Route("My")]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<List<Order>> EmpGet()
        {
            var user = _userServices.GetByName(User.FindFirstValue(ClaimTypes.Name));
            var orders = _services.GetOrdersById(user.Id);
            return Ok(orders);
        }

        //Get all orders as employy

        [HttpGet]
        [Route("OrdersEmp")]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<OrderDto> GetOne()
        {
            var ordersDto = _services.GetOrdersDto();
            return Ok(ordersDto);
        }

        //Create order

        [HttpPost]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<Order> Create(OrderDto orderDto)
        {
            var desk = _deskServices.GetOne(orderDto.DeskId);
            if(desk == null)
                return BadRequest("Desk doesn't exist");

            if (desk.IsAvailable == false)
                return BadRequest("This desk in unavailable");

            List<Order> orders = _services.GetDeskAtDays(orderDto.DeskId,orderDto.Date,orderDto.NumberOfDays);

            if (orders.Count != 0)
                return BadRequest("Desk is reserved that days");

            var userName = _userServices.GetByName(User.FindFirstValue(ClaimTypes.Name));

            var newOrder = _services.Add(orderDto,userName.Id);
            return CreatedAtAction(nameof(Create), new { id = newOrder.Id }, newOrder);
        }

        //Change desk in order

        [HttpPut]
        [Route("{id}-{deskId}")]
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult<Order> UpdateDesk(int id,int deskId)
        {
            var order = _services.GetOne(id);
            if(order == null || _userServices.GetByName(User.FindFirstValue(ClaimTypes.Name)).Id == order.UserId)
                return BadRequest("You can edit only your orders");

            if (order.Date < DateTime.Now.AddDays(1))
                return BadRequest("You can't change desk 24h before reservation");

            if (null == _deskServices.GetOne(id))
                return BadRequest("Desk doesn't exist");

            var newOrder = _services.UpdateDesk(deskId, id);
            if(newOrder == null)
                return NotFound();
            return Ok(newOrder);
        }

        //Edit order

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Order> Update(OrderDto orderDto, int id)
        {
            var order = _services.GetOne(id);
            if (order == null)
                return BadRequest("Desk doesn't exist");

            if (order.Date < DateTime.Now.AddDays(1))
                return BadRequest("You can't change desk 24h before reservation");

            List<Order> orders = _services.GetDeskAtDays(orderDto.DeskId, orderDto.Date, orderDto.NumberOfDays);

            if (orders.Count != 0)
                return BadRequest("Desk is reserved that days");

            var newOrder = _services.Update(orderDto, id);
            if (newOrder == null)
                return NotFound();

            return Ok(order);
        }

        //Delete order

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _services.Delete(id);
            return NoContent();
        }
    }
}
