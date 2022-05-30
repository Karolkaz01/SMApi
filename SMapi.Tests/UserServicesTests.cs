using SMApi.Models;
using SMApi.Services;
using SMApi.Data;
using Microsoft.EntityFrameworkCore;

namespace SMapi.Tests
{
    public class UserServicesTests
    {
        [Fact]
        public void GetByNameAndGetOne_User_ReturnUsers()
        {
            //arrange
            var options = new DbContextOptionsBuilder<SMApiContext>()
                .UseInMemoryDatabase(databaseName: "SMApiTestsDB")
                .Options;

            var context = new SMApiContext(options);



            context.Users.AddRange(
                new User { Id = 1, Name = "Employee" , PasswordHash = new byte[0], PasswordSalt = new byte[0] ,AccountType = Role.Employee },
                new User { Id = 2, Name = "Karol" , PasswordHash = new byte[0], PasswordSalt = new byte[0] ,AccountType = Role.Admin },
                new User { Id = 3, Name = "Aleks" , PasswordHash = new byte[0], PasswordSalt = new byte[0] ,AccountType = Role.Employee }
                );

            context.SaveChanges();

            var userService = new UserServices(context);

            //act


            var user11 = userService.GetByName("Employee").Name;
            var user12 = userService.GetByName("Karol").Name;
            var user13 = userService.GetByName("Aleks").Name;

            var user21 = userService.GetOne(1).Name;
            var user22 = userService.GetOne(2).Name;
            var user23 = userService.GetOne(3).Name;

            //assert

            Assert.Equal(user11, user21);
            Assert.Equal(user12, user22);
            Assert.Equal(user13, user23);

        }

        



    }
}