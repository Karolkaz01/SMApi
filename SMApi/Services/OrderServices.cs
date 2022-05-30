using SMApi.Models;
using SMApi.ModelsDto;

namespace SMApi.Services
{
    public class OrderServices
    {

        private readonly SMApiContext _context;

        public OrderServices(SMApiContext context)
        {
            _context = context;
        }

        public List<Order> Get() => _context.Orders.ToList();

        public Order? GetOne(int id) => _context.Orders.Find(id);

        public List<OrderDto> GetOrdersDto()
        {
            var orders = _context.Orders.ToList();
            var ordersDto = new List<OrderDto>();
            OrderDto singleOrder;

            foreach(var order in orders)
            {
                singleOrder = new OrderDto();

                singleOrder.NumberOfDays = order.NumberOfDays;
                singleOrder.DeskId = order.DeskId;
                singleOrder.Date = order.Date;

                ordersDto.Add(singleOrder);
            }

            return ordersDto;
        }

        public List<Order> GetOrdersById(int id)
        {
            var userOrders = _context.Orders.Where(o => o.UserId == id).ToList();

            return userOrders;
        }

        public List<Order> GetDeskAtDays(int id,DateTime date, int NumberOfDays)
        {
            List <Order> orders = _context.Orders
                .Where(o => o.Id == id)
                .Where(o => ((o.Date > date && date.AddDays(NumberOfDays) > o.Date) ||
                (o.Date < date && o.Date.AddDays(o.NumberOfDays) > date))).ToList();
            return orders;
        }

        public bool CanBeDeleted(int id)
        {
            var desk = _context.Orders
                .Where(o => o.DeskId == id)
                .FirstOrDefault(o => o.Date.AddDays(o.NumberOfDays) > DateTime.Now);
            
            return desk == null;
        }

        public Order? Add(OrderDto orderDto, int userId)
        {
            var order = new Order();

            order.UserId = userId;
            order.DeskId = orderDto.DeskId;
            order.Date = orderDto.Date;
            order.NumberOfDays = orderDto.NumberOfDays;

            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order? Update(OrderDto orderDto, int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return null;

            order.DeskId = orderDto.DeskId;
            order.Date = orderDto.Date;
            order.NumberOfDays = orderDto.NumberOfDays;

            _context.Orders.Update(order);
            _context.SaveChanges();

            return order;
        }

        public Order? UpdateDesk(int deskId, int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return null;

            order.DeskId = deskId;
            return order;
        }


        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return;

            _context.Remove(order);
            _context.SaveChanges();
        }

        /*
        private static List<Order> ListOfOrders = new List<Order>
        {
            new Order
            {
                Id = 1,
                UserId = 2,
                DeskId = 3,
                Date = new DateTime(2022, 6, 15)
            },
            new Order
            {
                Id = 2,
                UserId = 4,
                DeskId = 7,
                Date = new DateTime(2022, 6, 1)
            },
            new Order
            {
                Id = 3,
                UserId = 2,
                DeskId = 4,
                Date = new DateTime(2022, 5, 30)
            }
        };

        static int nextId = 3;

        public static List<Order> getAll() => ListOfOrders;

        public static Order? get(int id) => ListOfOrders.Find(p => p.Id == id);

        public static void add(Order order)
        {
            var desk = DeskServices.get(order.DeskId);
            if (desk == null)
                return;

            order.Id = nextId++;
            ListOfOrders.Add(order);
        }

        public static void update(Order newOrder)
        {
            var order = get(newOrder.Id);
            if (order == null)
                return;

            order.DeskId = newOrder.DeskId;
            order.NumberOfDays = newOrder.NumberOfDays;
        }

        public static void delete(int id)
        {
            var order = get(id);
            if (order == null)
                return;

            ListOfOrders.Remove(order);
        }
        */
    }
}
