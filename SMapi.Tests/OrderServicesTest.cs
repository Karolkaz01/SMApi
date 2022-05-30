using SMApi.Models;
using SMApi.Services;
using SMApi.Data;
using Microsoft.EntityFrameworkCore;
using SMApi.ModelsDto;
using SMApi.Controllers;

namespace SMapi.Tests
{
    public class OrderSercicesTests
    {
        [Fact]
        public void Add_Ordesrs_AddedOrdersToDB()
        {

            //arrange

            var options = new DbContextOptionsBuilder<SMApiContext>()
                .UseInMemoryDatabase(databaseName: "SMApiTestsDB")
                .Options;

            var context = new SMApiContext(options);

            context.Users.AddRange(
                new User { Id = 1, Name = "Monika", PasswordHash = new byte[0], PasswordSalt = new byte[0], AccountType = Role.Employee },
                new User { Id = 2, Name = "Karol", PasswordHash = new byte[0], PasswordSalt = new byte[0], AccountType = Role.Admin },
                new User { Id = 3, Name = "Aleks", PasswordHash = new byte[0], PasswordSalt = new byte[0], AccountType = Role.Employee }
                );

            context.SaveChanges();

            context.Locations.AddRange(
                new Location { Id = 1, Name = "Katowice" },
                new Location { Id = 2, Name = "Sosnowiec" },
                new Location { Id = 3, Name = "Będzin" }
                );

            context.SaveChanges();

            context.Desks.AddRange(
                new Desk { Id = 1, IsAvailable = true, LocationId = 1 },
                new Desk { Id = 2, IsAvailable = true, LocationId = 1 },
                new Desk { Id = 3, IsAvailable = false, LocationId = 2 },
                new Desk { Id = 4, IsAvailable = true, LocationId = 3 }
                );

            context.SaveChanges();

            var orderDto1 = new OrderDto { Date = new DateTime(2022, 06, 02), DeskId = 2, NumberOfDays = 5 };
            var orderDto2 = new OrderDto { Date = new DateTime(2022, 06, 14), DeskId = 2, NumberOfDays = 2 };
            var orderDto3 = new OrderDto { Date = new DateTime(2022, 06, 02), DeskId = 4, NumberOfDays = 1 };

            var orderServices = new OrderServices(context);
            var userServices = new UserServices(context);
            var deskServices = new DeskServices(context);
            var orderController = new OrderController(orderServices, userServices, deskServices);


            //act

            orderServices.Add(orderDto1, 1);
            orderServices.Add(orderDto2, 2);
            orderServices.Add(orderDto3, 3);

            //assert

            var order1 = orderServices.GetOne(1);
            var order2 = orderServices.GetOne(2);
            var order3 = orderServices.GetOne(3);

            Assert.Equal(1, order1.Id);
            Assert.Equal(2, order2.Id);
            Assert.Equal(3, order3.Id);
            
        }
    }
}
