using SMApi.Models;
using SMApi.Services;
using SMApi.Data;
using Microsoft.EntityFrameworkCore;
using SMApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SMapi.Tests
{
    public class LocationServicesTests
    {
        [Fact]
        public void Deleting_Location_From3Only1Deleted()
        {

            //arrange

            var options = new DbContextOptionsBuilder<SMApiContext>()
                .UseInMemoryDatabase(databaseName: "SMApiTestsDB")
                .Options;

            var context = new SMApiContext(options);

            context.Locations.AddRange(
                new Location { Id = 1, Name = "Katowice" },
                new Location { Id = 2, Name = "Sosnowiec" },
                new Location { Id = 3, Name = "Będzin" }
                );

            context.Desks.AddRange(
                new Desk { Id = 1, IsAvailable = true, LocationId = 1 },
                new Desk { Id = 2, IsAvailable = true, LocationId = 1 },
                new Desk { Id = 3, IsAvailable = false, LocationId = 2 },
                new Desk { Id = 4, IsAvailable = true, LocationId = 2 }
                );

            context.SaveChanges();

            var locationServices = new LocationServices(context);
            var deskServices = new DeskServices(context);

            var locationController = new LocationController(locationServices, deskServices);


            //act

            var result1 = locationController.Delete(1);
            var result2 = locationController.Delete(2);
            var result3 = locationController.Delete(3);


            //assert

            Assert.IsType<BadRequestObjectResult>(result1);
            Assert.IsType<BadRequestObjectResult>(result2);
            Assert.IsType<NoContentResult>(result3);





        }
    }
}
